import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { MfsSettingService } from '../mfs-setting.service';

@Injectable({
  providedIn: 'root'
})
export class ApplicationUserService {
   
    
    constructor(private http: HttpClient, private setting: MfsSettingService) {

    }

    save(applicationUserModel: any) {
        return this.http.post<any>(this.setting.securityApiServer + '/ApplicationUser/Save', applicationUserModel)
            .pipe(map(model => {
                return model;
            }));
    }

    delete(applicationUserModel: any) {
        return this.http.post<any>(this.setting.securityApiServer + '/ApplicationUser/Delete', applicationUserModel)
            .pipe(map(model => {
                return model;
            }));
    }

    getApplicationUserWorklist() {
        return this.http.get<any>(this.setting.securityApiServer + '/ApplicationUser/GetAllApplicationUserList')
            .pipe(map(featureCategoryList => {
                return featureCategoryList;
            }));
    }

    getApplicationUserById(entityId: number) {
        return this.http.get<any>(this.setting.securityApiServer + '/ApplicationUser/GetApplicationUserById?id=' + entityId)
            .pipe(map(model => {
                return model;
            }));
    }

    changePassword(changePasswordModel: any): any {
        return this.http.post<any>(this.setting.securityApiServer + '/ApplicationUser/ChangePassword', changePasswordModel)
            .pipe(map(model => {
                return model;
            }));
    }

    resetPassword(model: any): any {
        return this.http.post<any>(this.setting.securityApiServer + '/ApplicationUser/ResetPassword', model)
            .pipe(map(model => {
                return model;
            }));
    }

    changePasswordStatus(model: any): any {
        return this.http.post<any>(this.setting.securityApiServer + '/ApplicationUser/ChangePasswordStatus', model)
            .pipe(map(model => {
                return model;
            }));
    }

    checkExistingUserName(model: any): any {
        return this.http.post<any>(this.setting.securityApiServer + '/ApplicationUser/CheckExistingUserName', model)
            .pipe(map(model => {
                return model;
            }));
    }
    getAppUserListDdl() {
        return this.http.get<any>(this.setting.securityApiServer + '/ApplicationUser/getAppUserListDdl')
            .pipe(map(model => {
                return model;
            }));
    }
    
}
