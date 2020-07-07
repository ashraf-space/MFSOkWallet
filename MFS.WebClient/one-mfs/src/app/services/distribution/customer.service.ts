import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { MfsSettingService } from '../mfs-setting.service';
@Injectable({
  providedIn: 'root'
})
export class CustomerService {

    constructor(private http: HttpClient, private environment: MfsSettingService) {

    }

    save(agentModel: any, isEditMode: boolean, event:string) {
        return this.http.post<any>(this.environment.distributionApiServer + '/customer/SaveCustomer?isEdit=' + isEditMode + '&evnt=' + event, agentModel)
            .pipe(map(model => {
                return model;
            }));
    }
    getCustomerByMphone(mPhone: string) {
        return this.http.get<any>(this.environment.distributionApiServer + '/customer/getCustomerByMphone?mPhone=' + mPhone)
            .pipe(map(regInfoModel => {
                return regInfoModel;
            }));
    }
}
