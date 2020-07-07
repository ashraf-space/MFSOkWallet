import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { MfsSettingService } from '../mfs-setting.service';
@Injectable({
  providedIn: 'root'
})
export class ClusterService {

    constructor(private http: HttpClient, private setting: MfsSettingService) { }

    save(clusterModel: any) {
        return this.http.post<any>(this.setting.environmentApiServer + '/Cluster/SaveCluster', clusterModel)
            .pipe(map(model => {
                return model;
            }));
    }

    GetClusterById(clusterCode: string) {
        return this.http
            .get<any>(this.setting.environmentApiServer + '/Cluster/GetClusterById?code=' + clusterCode)
            .pipe(map(model => {
                return model;
            }));
    }
    GetAllClusters() {
        return this.http.get<any>(this.setting.environmentApiServer + '/Cluster/GetAllClusters')
            .pipe(map(clusterList => {
                return clusterList;
            }));
    }
    
    getTerritoryDDL() {
        return this.http.get<any>(this.setting.environmentApiServer + '/Cluster/GetTerritoryDDL')
            .pipe(map(clusterList => {
                return clusterList;
            }));
    }
    GenerateClusterCode(value: string) {
        return this.http.get<any>(this.setting.environmentApiServer + '/Cluster/GetClusterCode?code=' + value)
            .pipe(map(clusterCode => {
                return clusterCode;
            }));
    }
}
