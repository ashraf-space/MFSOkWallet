import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MfsSettingService } from '../mfs-setting.service';
import { map } from 'rxjs/operators';

@Injectable({
    providedIn: 'root'
})
export class BranchCashInService {


    constructor(private http: HttpClient, private transactionService: MfsSettingService) { }

    saveBranchCashIn(branchCashInModel: any, isEditMode: boolean): any {
        return this.http.post<any>(this.transactionService.transactionApiServer + '/FundTransfer/saveBranchCashIn?isEditMode=' + isEditMode, branchCashInModel)
            .pipe(map(model => {
                return model;
            }));
    }

    getDetailsByMphone(mphone: string): any {
        return this.http.get<any>(this.transactionService.distributionApiServer + '/Distributor/getRegInfoDetailsByMphone?mphone=' + mphone)
            .pipe(map(model => {
                return model;
            }));
    }

    getReginfoCashoutByMphone(mphone: string): any {
        return this.http.get<any>(this.transactionService.distributionApiServer + '/Distributor/getReginfoCashoutByMphone?mphone=' + mphone)
            .pipe(map(model => {
                return model;
            }));
    }

    getAmountByTransNo(mobile: string, transNo: any): any {
        return this.http.get<any>(this.transactionService.transactionApiServer + '/FundTransfer/getAmountByTransNo?transNo=' + transNo + '&mobile=' + mobile)
            .pipe(map(model => {
                return model;
            }));
    }

    AproveOrRejectBranchCashout(tblPortalCashoutModel: any, event: any): any {
        return this.http.post<any>(this.transactionService.transactionApiServer + '/FundTransfer/AproveOrRejectBranchCashout?evnt=' + event, tblPortalCashoutModel)
            .pipe(map(model => {
                return model;
            }));
    }

    CheckData(transNo: any, mphone: any, amount: any): any {
        return this.http.get<any>(this.transactionService.transactionApiServer + '/FundTransfer/CheckData?transNo=' + transNo + '&mphone=' + mphone + '&amount' + amount)
            .pipe(map(model => {
                return model;
            }));
    }

}
