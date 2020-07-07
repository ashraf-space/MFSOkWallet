import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MfsSettingService } from '../mfs-setting.service';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class TransactionMasterService { 

    constructor(private http: HttpClient, private transactionService: MfsSettingService) { }

    approveOrRejectBankDepositStatus(tblBdStatusModel: any, roleName: any, userName: any, event: any): any {
        return this.http.post<any>(this.transactionService.transactionApiServer + '/TransactionMaster/approveOrRejectBankDepositStatus?roleName=' + roleName + '&userName=' + userName + '&evnt=' + event, tblBdStatusModel)
            .pipe(map(model => {
                return model;
            }))
    }

    getBankDepositStatusForChecking(fromDate: any, toDate: any, balanceType: string, roleName: any): any {
        return this.http.get<any>(this.transactionService.transactionApiServer + '/TransactionMaster/GetBankDepositStatus?fromDate=' + fromDate + '&toDate=' + toDate + '&balanceType=' + balanceType + '&roleName=' + roleName)
            .pipe(map(model => {
                return model;
            }))
    }

}
