import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MfsSettingService } from '../mfs-setting.service';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ProcessService {   
    constructor(private http: HttpClient, private transactionService: MfsSettingService) { }

    executeEOD(todayDate: any, userName: string): any {
        return this.http.get<any>(this.transactionService.transactionApiServer + '/TransactionMaster/ExecuteEOD?todayDate=' + todayDate + '&userName=' + userName)
            .pipe(map(model => {
                return model;
            }))
    }

}
