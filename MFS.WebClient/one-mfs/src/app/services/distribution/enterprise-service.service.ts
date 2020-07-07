import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { MfsSettingService } from '../mfs-setting.service';

@Injectable({
  providedIn: 'root'
})
export class EnterpriseServiceService {
    
    constructor(private http: HttpClient, private environment: MfsSettingService) {

    }

    save(agentModel: any, isEditMode: boolean, event: string) {
        return this.http.post<any>(this.environment.distributionApiServer + '/enterprise/save?isEdit=' + isEditMode + '&evnt=' + event, agentModel)
            .pipe(map(model => {
                return model;
            }));
    }
}
