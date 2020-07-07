import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { MfsSettingService } from '../mfs-setting.service';

@Injectable({
  providedIn: 'root'
})
export class CustomerRequestService {
    requestList: any = [
        { label: 'Dispute', value: 'Dispute' },
        { label: 'Lien', value: 'Lien' },
        { label: 'Lock', value: 'Lock' },
        { label: 'Pin Reset', value: 'Pin Reset' },
        { label: 'SMS Query', value: 'SMS Query' },
        { label: 'Transaction Query', value: 'Transaction Query' },
        { label: 'Unlock', value: 'Unlock' }
    ];

    constructor(private http: HttpClient, private setting: MfsSettingService) {

    }

    save(model: any) {
        return this.http.post<any>(this.setting.clientApiServer + '/CustomerRequest/Save', model)
            .pipe(map(model => {
                return model;
            }));
    }
}
