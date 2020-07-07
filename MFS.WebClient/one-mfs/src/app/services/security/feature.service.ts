import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { MfsSettingService } from '../mfs-setting.service';

@Injectable({
  providedIn: 'root'
})
export class FeatureService {
    
    constructor(private http: HttpClient, private setting: MfsSettingService) {
        
    }

    save(featureModel: any) {
        return this.http.post<any>(this.setting.securityApiServer + '/Feature/Save', featureModel)
            .pipe(map(model => {
                return model;
            }));
    }

    delete(featureModel: any) {
        return this.http.post<any>(this.setting.securityApiServer + '/Feature/Delete', featureModel)
            .pipe(map(model => {
                return model;
            }));
    }

    getFeatureWorklist() {
        return this.http.get<any>(this.setting.securityApiServer + '/Feature/GetFeatureWorklist')
            .pipe(map(featureCategoryList => {
                return featureCategoryList;
            }));
    }

    getFeatureById(entityId: number) {
        return this.http.get<any>(this.setting.securityApiServer + '/Feature/GetFeatureById?id=' + entityId)
            .pipe(map(model => {
                return model;
            }));
    }

}
