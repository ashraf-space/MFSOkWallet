import { Injectable } from '@angular/core';
import { MfsSettingService } from '../mfs-setting.service';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class KycReportService {
    
   

    constructor(private http: HttpClient, private settings: MfsSettingService) {
    }
    getAccountCategoryList(): any {
        return this.http.get<any>(this.settings.reportingApiServer + '/Kyc/GetAccountCategory')
            .pipe(map(data => {
                return data;
            }));
    }
    getCashBackList() {
        return this.http.get<any>(this.settings.reportingApiServer + '/Kyc/GetCashbackCategory')
            .pipe(map(data => {
                return data;
            }));
    }
    getSubAccountCategoryDDL() {
        return this.http.get<any>(this.settings.reportingApiServer + '/Kyc/GetSubAccountCategory')
            .pipe(map(data => {
                return data;
            }));
    }

}
