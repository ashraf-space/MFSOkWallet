import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { MfsSettingService } from '../mfs-setting.service';

@Injectable({
    providedIn: 'root'
})
export class InterestTaxVatRateSetupService {

    constructor(private mfsSettingService: MfsSettingService,
        private http: HttpClient) { }

    getGlobalInfos(): any {
        return this.http.get<any>(this.mfsSettingService.environmentApiServer + '/GlobalConfig/GetGlobalConfigs')
            .pipe(map(globalStatus => {
                return globalStatus;
            }));
    }
}
