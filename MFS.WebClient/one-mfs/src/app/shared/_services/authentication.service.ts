import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { Router, ActivatedRoute } from '@angular/router';

import { User } from '../_models';
import { MfsSettingService } from '../../services/mfs-setting.service';

@Injectable({ providedIn: 'root' })
export class AuthenticationService {
    private currentUserSubject: BehaviorSubject<any>;
    public currentUser: Observable<any>;

    constructor(private http: HttpClient, private setting: MfsSettingService, private route: ActivatedRoute) {
        this.currentUserSubject = new BehaviorSubject<any>(JSON.parse(sessionStorage.getItem('authUser')));
        //this.currentUserSubject = new BehaviorSubject<any>(JSON.parse(localStorage.getItem('authUser')));
        this.currentUser = this.currentUserSubject.asObservable();
    }

    public get currentUserValue() {
        return this.currentUserSubject.value;
    }

    login(loginModel) {
        return this.http.post<any>(this.setting.securityApiServer + '/Security/Login', loginModel)
            .pipe(map(user => {
                if (user && user.bearerToken && user.isAuthenticated) {  
                    user.logTime = Date(); 
                    sessionStorage.setItem('authUser', JSON.stringify(user));
                    //localStorage.setItem('authUser', JSON.stringify(user));
                    this.currentUserSubject.next(user);
                }
                return user;
            }));
    }

    lockAccount(model) {
        return this.http.post<any>(this.setting.securityApiServer + '/Security/LockAccount', model)
            .pipe(map(response => {
                return response;
            }));
    }

    getDataForDashboard() {
        return this.http.get(this.setting.clientApiServer + '/Dashboard/GetDataForDashboard')
            .pipe(map(response => {
                return response;
            }));
    }

    getTransactionAnalysis() {
        return this.http.get(this.setting.reportingApiServer + '/Transaction/TransactionAnalysis')
            .pipe(map(response => {
                return response;
            }));
    }

    logout() {
        // remove user from local storage to log user out
        sessionStorage.removeItem('authUser');
        //localStorage.removeItem('authUser');
        this.currentUserSubject.next(null);
    }

    getCurrentStatePermission(targetRoute) {
        var statePermission;
        var targetRoutePath = targetRoute.split('/');

        this.currentUserSubject.value.featureList.filter(x => x.FEATURELINK.split('/')[0] == targetRoutePath[0]).forEach(obj => {
            var authorizedRoutePath = obj.FEATURELINK.split('/');

            if (targetRoutePath[0] == authorizedRoutePath[0]) {
                if (targetRoutePath[targetRoutePath.length - 1] == ':id') {
                    statePermission = obj;
                }
                else {
                    if (targetRoutePath[1] == authorizedRoutePath[1]) {
                        statePermission = obj;
                    }
                }
               
            }
        });

        return statePermission;
    }

    checkEditPermissionAccess(targetRoute) {
        var statePermission = this.getCurrentStatePermission(targetRoute);

        if (statePermission) {
            if (statePermission.ISEDITPERMITTED == 1) {
                return true;
            }
        }

        return false;
    }

    checkRegisterPermissionAccess(targetRoute) {
        var statePermission = this.getCurrentStatePermission(targetRoute);

        if (statePermission) {
            if (statePermission.ISREGISTRATIONPERMITTED == 1) {
                return true;
            }
        }

        return false;
    }
    checkIsSecuredViewPermitted(path: string): boolean {
        var statePermission = this.getCurrentStatePermission(path);

        if (statePermission) {
            if (statePermission.ISSECUREDVIEWPERMITTED == 1) {
                return true;
            }
        }

        return false;
    }

    getGlobalSearchResult(model: any): any {
        return this.http.get<any>(this.setting.clientApiServer + '/Dashboard/GetGlobalSearchResult?option=' + model.option + '&criteria=' + model.criteria + '&filter=' + model.filter)
            .pipe(map(model => {
                return model;
            }));
    }

    GetBillCollectionMenus(userId: number): any {
        return this.http.get<any>(this.setting.clientApiServer + '/Dashboard/GetBillCollectionMenus?userId=' + userId)
            .pipe(map(model => {
                return model;
            }));
    }
}