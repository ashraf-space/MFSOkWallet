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

    CheckBillInfo(billCollectionCommonModel: any): any {
        return this.http.post<any>(this.transactionService.transactionApiServer + '/BillCollectionCommon/CheckBillInfo', billCollectionCommonModel)
            .pipe(map(model => {
                return model;
            }));
    }
   
}
