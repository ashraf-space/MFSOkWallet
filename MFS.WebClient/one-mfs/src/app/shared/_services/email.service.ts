import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { MfsSettingService } from 'src/app/services/mfs-setting.service';

@Injectable({
    providedIn: 'root'
})
export class EmailService {
   
    constructor(private http: HttpClient, private setting: MfsSettingService) {

    }


    sendVeriCodeToEmail(emailId: any) {
        return this.http.get<any>(this.setting.clientApiServer + '/Email/SendVeriCodeToEmail?toEmailId=' + emailId)
            .pipe(map(model => {
                return model;
            }));
    }

    sendVeriCodeToEmailAfterChecking(forgotPassResetModel: any) {
        return this.http.post<any>(this.setting.clientApiServer + '/Email/SendVeriCodeToEmailAfterChecking', forgotPassResetModel)
            .pipe(map(model => {
                return model;
            }));
    }

    getMd5Password(verificationCode: any) {
        return this.http.get<any>(this.setting.clientApiServer + '/Email/GetMd5Password?verificationCode=' + verificationCode)
            .pipe(map(model => {
                return model;
            }));
    }

    resetPassword(forgotPassResetModel: any) {
        return this.http.post<any>(this.setting.securityApiServer + '/ApplicationUser/ResetPasswordForForgot', forgotPassResetModel)
            .pipe(map(model => {
                return model;
            }));
    }

    ifExistsField(field: string, userName: string, employeeId: string, mobileNo: string) {
        //ifExistsField(field: any, forgotPassResetModel: any) {
        return this.http.get<any>(this.setting.securityApiServer + '/ApplicationUser/CheckingFields?field=' + field + '&userName=' + userName + '&employeeId=' + employeeId + '&mobileNo=' + mobileNo)
            .pipe(map(model => {
                return model;
            }));
    }


  
   

}
