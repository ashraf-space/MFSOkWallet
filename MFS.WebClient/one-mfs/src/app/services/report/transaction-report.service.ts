import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MfsSettingService } from '../mfs-setting.service';
import { map } from 'rxjs/operators';

@Injectable({
    providedIn: 'root'
})
export class TransactionReportService {
     
    

    constructor(private http: HttpClient, private settings: MfsSettingService) { }

    getGlCoaCodeNameLevelDDL(assetType: any): any {
        return this.http.get<any>(this.settings.reportingApiServer + '/Transaction/GetGetGlCoaCodeNameLevelDDL?assetType=' + assetType)
            .pipe(map(data => {
                return data;
            }));
    }

    getOkServicesDDL(): any {
        return this.http.get<any>(this.settings.reportingApiServer + '/Transaction/GetOkServicesDDL')
            .pipe(map(data => {
                return data;
            }));
    }

    getParticularDDL(): any {
        return this.http.get<any>(this.settings.reportingApiServer + '/Transaction/GetParticularDDL')
            .pipe(map(data => {
                return data;
            }));
    }

    getTransactionDDLByParticular(particular: any): any {
        return this.http.get<any>(this.settings.reportingApiServer + '/Transaction/GetTransactionDDLByParticular?particular=' + particular)
            .pipe(map(data => {
                return data;
            }));
    }
   
}
