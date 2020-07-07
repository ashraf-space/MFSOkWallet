import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { MfsSettingService } from '../mfs-setting.service';


@Injectable({
  providedIn: 'root'
})
export class RoleService {

    constructor(private http: HttpClient, private setting: MfsSettingService) {

    }

    save(roleModel: any) {
        return this.http.post<any>(this.setting.securityApiServer + '/Role/Save', roleModel)
            .pipe(map(model => {
                return model;
            }));
    }

    delete(roleModel: any) {
        return this.http.post<any>(this.setting.securityApiServer + '/Role/Delete', roleModel)
            .pipe(map(model => {
                return model;
            }));
    }

    getRoleWorklist() {
        return this.http.get<any>(this.setting.securityApiServer + '/Role/GetAllRoleList')
            .pipe(map(roleList => {
                return roleList;
            }));
    }

    getRoleById(entityId: number) {
        return this.http.get<any>(this.setting.securityApiServer + '/Role/GetRoleById?id=' + entityId)
            .pipe(map(model => {
                return model;
            }));
    }

    getRoleListForDDL() {
        return this.http.get<any>(this.setting.securityApiServer + '/Role/GetRoleListForDDL')
            .pipe(map(roleList => {
                return roleList;
            }));
    }
}
