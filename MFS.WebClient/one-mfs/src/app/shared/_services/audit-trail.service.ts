import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { MfsSettingService } from '../../services/mfs-setting.service';

@Injectable({
    providedIn: 'root'
})
export class AuditTrailService {
   
  
    
    constructor(private http: HttpClient, private setting: MfsSettingService) {

    }

    getWhatActionId(action: string) {
        var whatActionId;
        switch (action) {
            case 'VISIT':
                whatActionId = 1;
                break;
            case 'SEARCH':
                whatActionId = 2;
                break;
            case 'ADD':
                whatActionId = 3;
                break;
            default:
                whatActionId = 4;
        }
        return whatActionId;
    }

    getWhichParentMenuId(ParentMenu: string) {
        var whichParentMenuId;
        switch (ParentMenu) {
            //case 'Dashboard':
            case 'Home':
                whichParentMenuId = 1;
                break;
            case 'Customer Care':
                whichParentMenuId = 2;
                break;
            case 'Channels':
                whichParentMenuId = 3;
                break;
            case 'Environment':
                whichParentMenuId = 4;
                break;
            case 'Merchant':
                whichParentMenuId = 5;
                break;
            case 'Reports':
                whichParentMenuId = 6;
                break;
            case 'Settings':
                whichParentMenuId = 7;
                break;
            case 'Tools':
                whichParentMenuId = 8;
                break;
            case 'Transaction':
                whichParentMenuId = 9;
                break;
            case 'Process':
                whichParentMenuId = 10;
                break;        
            case 'Utility Bill Collection':
                whichParentMenuId = 11;
                break;
            case 'Tuition Fee Collection':
                whichParentMenuId = 12;
                break;
            case 'Credit Card Bill Collection':
                whichParentMenuId = 13;
                break;
            case 'Other Bill/Fee Collection':
                whichParentMenuId = 14;
                break;
            case 'Dashboard':
                whichParentMenuId = 15;
                break;
            default:
                whichParentMenuId = 16;
        }
        return whichParentMenuId;
    }

    insertIntoAuditTrail(model: any): any {
        return this.http.post<any>(this.setting.securityApiServer + '/AuditTrail/InsertIntoAuditTrail', model)
            .pipe(map(model => {
                return model;
            }));
    }

    getUserListDdl() {
        return this.http.get<any>(this.setting.securityApiServer + '/AuditTrail/getUserListDdl')
            .pipe(map(model => {
                return model;
            }));
    }
    getParentMenuList() {
        return this.http.get<any>(this.setting.securityApiServer + '/AuditTrail/getParentMenuList')
            .pipe(map(model => {
                return model;
            }));
    }
}
