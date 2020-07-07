import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MfsSettingService } from '../mfs-setting.service';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class RateConfigService {
    
    constructor(private http: HttpClient, private transaction: MfsSettingService) { }

    getRateConfigByConfigId(configId) {
        return this.http.get<any>(this.transaction.transactionApiServer + '/RateconfigMst/GetRateConfigByConfigId?configId=' + configId)
            .pipe(map(data => {
                return data;
            }));
    }
    
    save(model: any) {
        return this.http.post<any>(this.transaction.transactionApiServer + '/RateconfigMst/Save', model)
            .pipe(map(model => {
                return model;
            }));
    }

}
