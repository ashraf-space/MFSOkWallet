import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { MfsSettingService } from '../mfs-setting.service';

@Injectable({
  providedIn: 'root'
})
export class AreaService {

    constructor(private http: HttpClient, private setting: MfsSettingService) { }

    getRegionsDDL() {
        return this.http.get<any>(this.setting.environmentApiServer + '/location/GetRegions')
            .pipe(map(regionCategoryList => {
                return regionCategoryList;
            }));
    }
    save(areaModel: any) {
        return this.http.post<any>(this.setting.environmentApiServer + '/location/SaveArea', areaModel)
            .pipe(map(model => {
                return model;
            }));
    }

    getAreas() {
        return this.http.get<any>(this.setting.environmentApiServer + '/location/getareas')
            .pipe(map(areaList => {
                return areaList;
            }));
    }
    getAreaById(areaCode: string) {
        return this.http.get<any>(this.setting.environmentApiServer + '/location/getareabyid?code=' + areaCode)
            .pipe(map(model => {
                return model;
            }));
    }
}
