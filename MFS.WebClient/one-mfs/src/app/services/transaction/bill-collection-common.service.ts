import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MfsSettingService } from '../mfs-setting.service';
import { map } from 'rxjs/operators';

@Injectable({
    providedIn: 'root'
})
export class BillCollectionCommonService {
   
   
    constructor(private http: HttpClient, private transactionService: MfsSettingService) { }

    GetFeaturePayDetails(featureId: number): any {
        return this.http.get<any>(this.transactionService.transactionApiServer + '/BillCollectionCommon/GetFeaturePayDetails?featureId=' + featureId)
            .pipe(map(model => {
                return model;
            }));
    }

    GetMonthYearList(): any {
        return this.http.get<any>(this.transactionService.transactionApiServer + '/BillCollectionCommon/GetMonthYearList')
            .pipe(map(model => {
                return model;
            }));
    }

    GetSubMenuDDL(featureId: number): any {
        return this.http.get<any>(this.transactionService.transactionApiServer + '/BillCollectionCommon/GetSubMenuDDL?featureId=' + featureId)
            .pipe(map(model => {
                return model;
            }));
    }

    GetBillPayCategoriesDDL(userId: number): any {
        return this.http.get<any>(this.transactionService.transactionApiServer + '/BillCollectionCommon/GetBillPayCategoriesDDL?userId=' + userId)
            .pipe(map(model => {
                return model;
            }));
    }


    CheckBillInfo(billCollectionCommonModel: any): any {
        return this.http.post<any>(this.transactionService.transactionApiServer + '/BillCollectionCommon/CheckBillInfo', billCollectionCommonModel)
            .pipe(map(model => {
                return model;
            }));
    }

    GetFeeInfo(billCollectionCommonModel: any): any {
        return this.http.post<any>(this.transactionService.transactionApiServer + '/BillCollectionCommon/GetFeeInfo', billCollectionCommonModel)
            .pipe(map(model => {
                return model;
            }));
    }

    confirmBill(billCollectionCommonModel: any): any {
        return this.http.post<any>(this.transactionService.transactionApiServer + '/BillCollectionCommon/ConfirmBill', billCollectionCommonModel)
            .pipe(map(model => {
                return model;
            }));
    }

    GetDataForCommonGrid(username: any, MethodName: any, countLimit: any, billNo: string = null) {
        return this.http.get<any>(this.transactionService.transactionApiServer + '/BillCollectionCommon/GetDataForCommonGrid?username=' + username + '&MethodName=' + MethodName + '&countLimit=' + countLimit + '&billNo=' + billNo)
            .pipe(map(model => {
                return model;
            }));
    }

    GenerateReceipt(branchPortalReceipt: any) {
        return this.http.get<any>('http://10.20.32.118/NEW/ok_api/receipt/view.php?mphone=' + branchPortalReceipt.ref_Phone + '&Trans_ID=' + branchPortalReceipt.trans_No);
       
    }

    GetTitleSubmenuTitleByMethod(methodName: string) {
        return this.http.get<any>(this.transactionService.transactionApiServer + '/BillCollectionCommon/GetTitleSubmenuTitleByMethod?methodName=' + methodName)
            .pipe(map(model => {
                return model;
            }));
    }

   
}
