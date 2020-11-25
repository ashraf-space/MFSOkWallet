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

}
