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

    getApplicationUserWorklist(roleName: string) {
        return this.http.get<any>(this.setting.securityApiServer + '/ApplicationUser/GetAllApplicationUserList?roleName=' + roleName)
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

    changePassword(changePasswordModel: any, passwordChangedBy: string): any {
        return this.http.post<any>(this.setting.securityApiServer + '/ApplicationUser/ChangePassword?passwordChangedBy=' + passwordChangedBy, changePasswordModel)
            .pipe(map(model => {
                return model;
            }));
    }

    changeEmail(changeEmailIdModel: any, passwordChangedBy: string): any {
        return this.http.post<any>(this.setting.securityApiServer + '/ApplicationUser/ChangeEmail?passwordChangedBy=' + passwordChangedBy, changeEmailIdModel)
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
    checkExistingEmailId(emailId: any): any {
        return this.http.get<any>(this.setting.securityApiServer + '/ApplicationUser/CheckExistingEmailId?emailId=' + emailId)
            .pipe(map(model => {
                return model;
            }));
    }
    checkExistingMobileNo(mobileNo: any): any {
        return this.http.get<any>(this.setting.securityApiServer + '/ApplicationUser/CheckExistingMobileNo?mobileNo=' + mobileNo)
            .pipe(map(model => {
                return model;
            }));
    }

    checkExistingEmployeeId(employeeId: any): any {
        return this.http.get<any>(this.setting.securityApiServer + '/ApplicationUser/CheckExistingEmployeeId?employeeId=' + employeeId)
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

    getAppUserListDdlForStingValue(branchCode: string) {
        return this.http.get<any>(this.setting.securityApiServer + '/ApplicationUser/GetAppUserListDdlForStingValue?branchCode=' + branchCode)
            .pipe(map(model => {
                return model;
            }));
    }

}
