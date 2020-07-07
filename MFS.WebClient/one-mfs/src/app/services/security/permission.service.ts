import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { MfsSettingService } from '../mfs-setting.service';

@Injectable({
  providedIn: 'root'
})
export class PermissionService {

    constructor(private http: HttpClient, private setting: MfsSettingService) {

    }    

    getPermissionWorklist(): any {
        return this.http.get<any>(this.setting.securityApiServer + '/permission/GetPermissionWorklist')
            .pipe(map(permssionList => {
                return permssionList;
            }));
    }
}
