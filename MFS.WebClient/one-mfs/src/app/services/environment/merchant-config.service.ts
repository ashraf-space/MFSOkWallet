import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { MfsSettingService } from '../mfs-setting.service';

@Injectable({
  providedIn: 'root'
})
export class MerchantConfigService {
    
    constructor(private http: HttpClient, private environment: MfsSettingService)  { }   

        getMerchantConfigListForDDL(): any {
            return this.http.get<any>(this.environment.environmentApiServer + '/MerchantConfig/GetMerchantConfigListForDDL')
            .pipe(map(model => {
                return model;
            }))
        }
    
    GetMerchantConfigDetails(mphone: any): any {
        return this.http.get<any>(this.environment.environmentApiServer + '/MerchantConfig/GetMerchantConfigDetails?mphone=' + mphone)
            .pipe(map(model => {
                return model;
            }))
    }

    save(merchantConfigModel: any) {
        return this.http.post<any>(this.environment.environmentApiServer + '/MerchantConfig/SaveMerchantConfig', merchantConfigModel)
            .pipe(map(model => {
                return model;
            }))
    }
}
