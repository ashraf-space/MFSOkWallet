import { Injectable } from '@angular/core';
import { MfsSettingService } from '../mfs-setting.service';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class TransactionDetailService {

    constructor(private http: HttpClient, private transaction: MfsSettingService) { }

    getTransactionMasterByTransNo(transNo) {
        return this.http.get<any>(this.transaction.transactionApiServer + '/TransactionMaster/GetTransactionMasterByTransNo?transNo=' + transNo)
            .pipe(map(data => {
                return data;
            }));
    }
}
