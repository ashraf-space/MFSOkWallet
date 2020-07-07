import { Injectable } from '@angular/core';
import { MfsSettingService } from '../mfs-setting.service';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class DistributorDepositService {  
         

    constructor(private http: HttpClient, private transactionService: MfsSettingService) { }

    GetCompanyAndHolderName(acNo: any): any {
        return this.http.get<any>(this.transactionService.distributionApiServer + '/Distributor/GetCompanyAndHolderName?acNo=' + acNo)
            .pipe(map(model => {
                return model;
            }))
    }
    

    save(cashEntryModel: any, isEditMode: boolean, event: string) {
        return this.http.post<any>(this.transactionService.transactionApiServer + '/DistributorDeposit/Save?isEditMode=' + isEditMode + '&evnt=' + event, cashEntryModel)
            .pipe(map(model => {
                return model;
            }))
    }

    GetTransAmtLimit(createUser: any): any {
        return this.http.get<any>(this.transactionService.securityApiServer + '/ApplicationUser/GetTransAmtLimit?createUser=' + createUser)
            .pipe(map(model => {
                return model;
            }))
    } 

    GetAmountInWords(amount: any): any {
        return this.http.get<any>(this.transactionService.transactionApiServer + '/DistributorDeposit/GetAmountInWords?amount=' + amount)
            .pipe(map(model => {
                return model;
            }))
    } 

    getDestributorDepositByTransNo(transNo: string): any {
        return this.http.get<any>(this.transactionService.transactionApiServer + '/DistributorDeposit/GetDestributorDepositByTransNo?transNo=' + transNo)
            .pipe(map(tblCashEntryModel => {
                return tblCashEntryModel;
            }));
    }
    getDistributorAcList(): any {
        return this.http.get<any>(this.transactionService.distributionApiServer + '/Distributor/GetDistributorAcList')
            .pipe(map(model => {
                return model;
            }))
    } 
}
