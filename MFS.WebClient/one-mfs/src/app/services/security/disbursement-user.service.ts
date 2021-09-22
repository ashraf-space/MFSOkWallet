import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MfsSettingService } from '../mfs-setting.service';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class DisbursementUserService {


    constructor(private http: HttpClient, private setting: MfsSettingService) {

    }

    save(disbursementUserModel: any) {
        return this.http.post<any>(this.setting.securityApiServer + '/DisbursementUser/Save', disbursementUserModel)
            .pipe(map(model => {
                return model;
            }));
    }

    delete(disbursementUserModel: any) {
        return this.http.post<any>(this.setting.securityApiServer + '/DisbursementUser/Delete', disbursementUserModel)
            .pipe(map(model => {
                return model;
            }));
    }

    getDisbursementUserWorklist(roleName: string) {
        return this.http.get<any>(this.setting.securityApiServer + '/DisbursementUser/GetAllDisbursementUserList?roleName=' + roleName)
            .pipe(map(featureCategoryList => {
                return featureCategoryList;
            }));
    }

    getDisbursementUserById(entityId: number) {
        return this.http.get<any>(this.setting.securityApiServer + '/DisbursementUser/GetDisbursementUserById?id=' + entityId)
            .pipe(map(model => {
                return model;
            }));
    }

    changePassword(changePasswordModel: any, passwordChangedBy: string): any {
        return this.http.post<any>(this.setting.securityApiServer + '/DisbursementUser/ChangePassword?passwordChangedBy=' + passwordChangedBy, changePasswordModel)
            .pipe(map(model => {
                return model;
            }));
    }

    resetPassword(model: any): any {
        return this.http.post<any>(this.setting.securityApiServer + '/DisbursementUser/ResetPassword', model)
            .pipe(map(model => {
                return model;
            }));
    }

    changePasswordStatus(model: any): any {
        return this.http.post<any>(this.setting.securityApiServer + '/DisbursementUser/ChangePasswordStatus', model)
            .pipe(map(model => {
                return model;
            }));
    }

    checkExistingUserName(model: any): any {
        return this.http.post<any>(this.setting.securityApiServer + '/DisbursementUser/CheckExistingUserName', model)
            .pipe(map(model => {
                return model;
            }));
    }
    getAppUserListDdl() {
        return this.http.get<any>(this.setting.securityApiServer + '/DisbursementUser/getAppUserListDdl')
            .pipe(map(model => {
                return model;
            }));
    }

    CheckDisbursementUserAlreadyExist(username: any) {
        return this.http.get<any>(this.setting.securityApiServer + '/DisbursementUser/CheckDisbursementUserAlreadyExist?username=' + username)
            .pipe(map(merchant => {
                return merchant;
            }));
    }
}
