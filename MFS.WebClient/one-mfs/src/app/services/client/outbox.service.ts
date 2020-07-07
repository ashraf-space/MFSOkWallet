import { Injectable } from '@angular/core';
import { MfsSettingService } from '../mfs-setting.service';
import { map, first } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';


@Injectable({
  providedIn: 'root'
})
export class OutboxService {
    
    constructor(private http: HttpClient, private mfsSettingService: MfsSettingService) {
    }

    resendMessage(model: any): any {
        return this.http.post(this.mfsSettingService.clientApiServer + '/Outbox/ResendMessage', model)
            .pipe(map(data => {
                return data;
            }))
    }

}
