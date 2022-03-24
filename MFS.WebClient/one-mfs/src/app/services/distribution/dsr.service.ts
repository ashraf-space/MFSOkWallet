import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MfsSettingService } from '../mfs-setting.service';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class DsrService {
   
   


    constructor(private http: HttpClient, private distribution: MfsSettingService) {

    }

    GetDistributorDataByDistributorCode(distributorCode: string): any {
        return this.http.get<any>(this.distribution.distributionApiServer + '/Dsr/GetDistributorDataByDistributorCode?distributorCode=' + distributorCode)
            .pipe(map(regInfoModel => {
                return regInfoModel;
            }));
    }    

    getDistributorList() {
        return this.http.get<any>(this.distribution.distributionApiServer + '/Distributor/GetDistributorListData')
            .pipe(map(distributorList => {
                return distributorList;
            }));
    }

    save(regInfoModel: any, isEditMode: boolean, event) {
        return this.http.post<any>(this.distribution.distributionApiServer + '/Dsr/Save?isEditMode=' + isEditMode + '&evnt=' + event, regInfoModel)
            .pipe(map(model => {
                return model;
            }))
    }

   

    generateDistributorCode(selectedTerritory: string): any {
        return this.http.get<any>(this.distribution.environmentApiServer + '/Location/GenerateDistributorCode?territoryCode=' + selectedTerritory)
            .pipe(map(distributorCode => {
                return distributorCode;
            }));
    }
    saveB2bDsr(regInfoModel: any, isEditMode: boolean, event: any) {
        return this.http.post<any>(this.distribution.distributionApiServer + '/Dsr/SaveB2bDsr?isEditMode=' + isEditMode + '&evnt=' + event, regInfoModel)
            .pipe(map(model => {
                return model;
            }))
    }
    GetB2bDistributorDataByDistributorCode(DistributorCode: string, catId: string) {
        return this.http.get<any>(this.distribution.distributionApiServer + '/Dsr/GetB2bDistributorDataByDistributorCode?distributorCode=' + DistributorCode + '&catId=' + catId)
            .pipe(map(regInfoModel => {
                return regInfoModel;
            }));
    }
}
