import { Injectable } from '@angular/core';
import { MfsSettingService } from '../mfs-setting.service';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';

@Injectable({
    providedIn: 'root'
})
export class ChartOfAccountsService {

    constructor(private http: HttpClient, private transaction: MfsSettingService) { }

    getChartOfAccountsList() {
        return this.http.get<any>(this.transaction.transactionApiServer + '/ChartOfAccounts/GetChartOfAccountsList')
            .pipe(map(data => {
                return data;
            }));
    }

    save(model: any) {
        return this.http.post<any>(this.transaction.transactionApiServer + '/ChartOfAccounts/Save', model)
            .pipe(map(model => {
                return model;
            }));
    }
}
