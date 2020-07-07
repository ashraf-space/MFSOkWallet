import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { MfsSettingService } from '../mfs-setting.service';

@Injectable({
  providedIn: 'root'
})
export class FeatureCategoryService {

    constructor(private http: HttpClient, private setting: MfsSettingService) {

    }

    getFeatureCategoryListDDL() {
        return this.http.get<any>(this.setting.securityApiServer + '/FeatureCategory/GetFeatureCategoryListForDDL')
            .pipe(map(featureCategoryList => {
                return featureCategoryList;
            }));
    }

    save(featureCategoryModel: any) {
        return this.http.post<any>(this.setting.securityApiServer + '/FeatureCategory/Save', featureCategoryModel)
            .pipe(map(model => {
                return model;
            }));
    }

    delete(featureCategoryModel: any) {
        return this.http.post<any>(this.setting.securityApiServer + '/FeatureCategory/Delete', featureCategoryModel)
            .pipe(map(model => {
                return model;
            }));
    }

    getFeatureCategoryWorklist() {
        return this.http.get<any>(this.setting.securityApiServer + '/FeatureCategory/GetFeatureCategoryWorklist')
            .pipe(map(featureCategoryList => {
                return featureCategoryList;
            }));
    }

    getFeatureCategoryById(entityId: number) {
        return this.http.get<any>(this.setting.securityApiServer + '/FeatureCategory/GetFeatureCategoryById?id='+entityId)
            .pipe(map(model => {
                return model;
            }));
    }
}
