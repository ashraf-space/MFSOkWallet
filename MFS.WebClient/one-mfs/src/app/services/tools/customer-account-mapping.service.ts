import { Injectable } from '@angular/core';
import { MfsSettingService } from '../mfs-setting.service';
import { map, first } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
@Injectable({
  providedIn: 'root'
})
export class CustomerAccountMappingService {
   
   
    constructor(private http: HttpClient, private mfsSettings: MfsSettingService) {

    }

    getNameByMphone(mblNo: string): any {
        return this.http.get<any>(this.mfsSettings.transactionApiServer + '/CbsMappedAccount/getNameByMphone?mblNo=' + mblNo)
            .pipe(map(name => {
                return name;
            }));
    }

    getCbsInfoByAccNo(accno: any, reqType) {
        return this.http.get<any>(this.mfsSettings.transactionApiServer + '/CbsMappedAccount/GetCbsCustomerInfo?accNo=' + accno + '&reqType=' + reqType)
            .pipe(map(data => {
                return data;
            }));
    }

    checkIsAccountValid(mblAccNo: any, accNo: any): any {
        return this.http.get<any>(this.mfsSettings.transactionApiServer + '/CbsMappedAccount/checkIsAccountValid?mblNo=' + mblAccNo + '&accNo=' + accNo)
            .pipe(map(data => {
                return data;
            }));
    }

    checkAccountValidityByCount(mblAcc) {
        return this.http.get<any>(this.mfsSettings.transactionApiServer + '/CbsMappedAccount/checkAccountValidityByCount?mblNo=' + mblAcc)
            .pipe(map(data => {
                return data;
            }));
    }

    checkAccNoIsMappedByMblNo(mblAcc, accno) {
        return this.http.get<any>(this.mfsSettings.transactionApiServer + '/CbsMappedAccount/CheckAccNoIsMappedByMblNo?mblNo=' + mblAcc + '&accno=' + accno)
            .pipe(map(data => {
                return data;
            }));
    }

    SaveMapOrRemapCbsAccount(data: any) {
        return this.http.post<any>(this.mfsSettings.transactionApiServer + '/CbsMappedAccount/SaveMapOrRemapCbsAccount', data)
            .pipe(map(data => {
                return data;
            }));
    }

    checkPendingAccountByMphone(mblAcc: any) {
        return this.http.get<any>(this.mfsSettings.transactionApiServer + '/CbsMappedAccount/checkPendingAccountByMphone?mblNo=' + mblAcc)
            .pipe(map(data => {
                return data;
            }));
    }

    checkActivatdAccountByMphone(mblAcc: any) {
        return this.http.get<any>(this.mfsSettings.transactionApiServer + '/CbsMappedAccount/CheckActivatdAccountByMphone?mblNo=' + mblAcc)
            .pipe(map(data => {
                return data;
            }));
    }

    checkIfCbsClassValid(acc: any) {
        return this.http.get<any>(this.mfsSettings.transactionApiServer + '/CbsMappedAccount/CheckActivatdAccountByMphone?mblNo=' + acc)
            .pipe(map(data => {
                return data;
            }));
    }

    onCbsSearch(accno: any, mblAcc: any) {
        return this.http.get<any>(this.mfsSettings.transactionApiServer + '/CbsMappedAccount/onCbsSearch?accno=' + accno + '&mblAcc=' + mblAcc)
            .pipe(map(data => {
                return data;
            }));
    }

    
}

