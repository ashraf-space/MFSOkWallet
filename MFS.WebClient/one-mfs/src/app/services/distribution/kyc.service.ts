import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { MfsSettingService } from '../mfs-setting.service';
@Injectable({
  providedIn: 'root'
})
export class KycService {
   
    
    getBalanceInfoByMphone(entity: any) {
        return this.http.get<any>(this.environment.distributionApiServer + '/Kyc/getBalanceInfoByMphone?mphone=' + entity)
            .pipe(map(model => {
                return model;
            }));
    }
   
    
    constructor(private http: HttpClient, private environment: MfsSettingService) {
    }

    checkNidValid(value: any, type:any) {
        return this.http.get<any>(this.environment.distributionApiServer + '/Kyc/CheckNidValid?photoid=' + value + '&type=' + type)
            .pipe(map(data => {
                return data;
            }));
    }
    checkNidValidWithIdType(value: any, type: any,idType:any) {
        return this.http.get<any>(this.environment.distributionApiServer + '/Kyc/checkNidValidWithIdType?photoid=' + value + '&type=' + type + '&idType=' + idType)
            .pipe(map(data => {
                return data;
            }));
    }
    getReginfoByMphone(mPhone: string): any {
        return this.http.get<any>(this.environment.distributionApiServer + '/Kyc/GetReginfoByMphone?mPhone=' + mPhone)
            .pipe(map(regInfoModel => {
                return regInfoModel;
            }));
    } 
    GetOccupationList(): any {
        return this.http.get<any>(this.environment.distributionApiServer + '/Kyc/GetOccupationList')
            .pipe(map(data => {
                return data;
            }));
    }
    updetKyc(model: any) {
        return this.http.post<any>(this.environment.distributionApiServer + '/kyc/updetKyc', model)
            .pipe(map(model => {
                return model;
            }));
    }
    getClientDistLocationInfo(distCode: any, locationCode: any) {
        return this.http.get<any>(this.environment.distributionApiServer + '/Kyc/getClientDistLocationInfo?distCode=' + distCode + '&locationCode=' + locationCode)
            .pipe(map(data => {
                return data;
            }));
    }
    getPhotoIdTypeByCode(photoIdTypeCode: any) {
        return this.http.get<any>(this.environment.distributionApiServer + '/Kyc/getPhotoIdTypeByCode?photoIdTypeCode=' + photoIdTypeCode)
            .pipe(map(regInfoModel => {
                return regInfoModel;
            }));
    }
    getBranchNameByCode(branchCode: any) {
        return this.http.get<any>(this.environment.distributionApiServer + '/Kyc/getBranchNameByCode?branchCode=' + branchCode)
            .pipe(map(regInfoModel => {
                return regInfoModel;
            }));
    }

    clientClose(actionModel: any, remarks: string) {
        return this.http.post(this.environment.distributionApiServer + '/Kyc/clientClose?remarks=' + remarks, actionModel)
            .pipe(map(model => {
                return model;
            }))
    }
    addRemoveLien(model: any, remarks: string) {
        return this.http.post(this.environment.distributionApiServer + '/Kyc/addRemoveLien?remarks=' + remarks, model)
            .pipe(map(model => {
                return model;
            }))
    }
    blackListClient(actionModel: any, remarks: string) {
        return this.http.post(this.environment.distributionApiServer + '/Kyc/blackListClient?remarks=' + remarks, actionModel)
            .pipe(map(model => {
                return model;
            }))
    }

    onReleaseBindDevice(model: any) {
        return this.http.post(this.environment.distributionApiServer + '/Kyc/OnReleaseBindDevice', model)
            .pipe(map(model => {
                return model;
            }))
    }
    getSubCatNameById(mphone: any) {
        return this.http.get<any>(this.environment.distributionApiServer + '/Kyc/getSubCatNameById?mphone=' + mphone)
            .pipe(map(regInfoModel => {
                return regInfoModel;
            }));
    }
    changeStatus(model: any, remarks: any) {
        return this.http.post(this.environment.distributionApiServer + '/Kyc/changeStatus?remarks=' + remarks, model)
            .pipe(map(model => {
                return model;
            }))
    }


}
