import { Injectable } from '@angular/core';
import { MfsSettingService } from '../mfs-setting.service';
import { map, first } from 'rxjs/operators';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';


@Injectable({
    providedIn: 'root'
})
export class DistributorService {
    
   

    constructor(private http: HttpClient, private distribution: MfsSettingService) {

    }

    GetDistributorByMphone(mPhone: string): any {
        return this.http.get<any>(this.distribution.distributionApiServer + '/Distributor/GetDistributorByMphone?mPhone=' + mPhone)
            .pipe(map(regInfoModel => {
                return regInfoModel;
            }));
    }
    GetDistcodeAndNameByMphone(mPhone: string): any {
        return this.http.get<any>(this.distribution.distributionApiServer + '/Distributor/GetDistcodeAndNameByMphone?mPhone=' + mPhone)
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

    save(regInfoModel: any, isEditMode: boolean, event: string) {
        return this.http.post<any>(this.distribution.distributionApiServer + '/Distributor/Save?isEditMode=' + isEditMode + '&evnt=' + event, regInfoModel)
            .pipe(map(model => {
                return model;
            }))
    }
    SaveB2bRetal(regInfoModel: any, isEditMode: boolean, event: string) {
        return this.http.post<any>(this.distribution.distributionApiServer + '/Distributor/SaveB2bRetal?isEditMode=' + isEditMode + '&evnt=' + event, regInfoModel)
            .pipe(map(model => {
                return model;
            }))
    }
    saveB2bDistributor(regInfoModel: any, isEditMode: boolean, event: string) {
        return this.http.post<any>(this.distribution.distributionApiServer + '/Distributor/SaveB2bDistributor?isEditMode=' + isEditMode + '&evnt=' + event, regInfoModel)
            .pipe(map(model => {
                return model;
            }))
    }
    getRegionList() {
        return this.http.get<any>(this.distribution.environmentApiServer + '/Location/GetRegions')
            .pipe(map(regionList => {
                return regionList;
            }));
    }

    getAreaListByRegion(code: string) {
        return this.http.get<any>(this.distribution.environmentApiServer + '/Location/GetAreasByRegion?code=' + code)
            .pipe(map(areaList => {
                return areaList;
            }));
    }

    getTerritoryListByArea(code: string) {
        return this.http.get<any>(this.distribution.environmentApiServer + '/Location/GetTerritoriesByArea?code=' + code)
            .pipe(map(territoryList => {
                return territoryList;
            }));
    }

    getPhotoIDTypeListForDDL(): any {
        return this.http.get<any>(this.distribution.environmentApiServer + '/Location/GetPhotoIDTypeList')
            .pipe(map(divisionList => {
                return divisionList;
            }));
    }

    getDivisionList(): any {
        return this.http.get<any>(this.distribution.environmentApiServer + '/Location/GetDivisions')
            .pipe(map(divisionList => {
                return divisionList;
            }));
    }

    getDistrictListByDivision(code: string): any {
        return this.http.get<any>(this.distribution.environmentApiServer + '/Location/GetChildDataByParent?code=' + code)
            .pipe(map(districtList => {
                return districtList;
            }));
    }

    getBankBranchListForDDL() {
        return this.http.get<any>(this.distribution.environmentApiServer + '/Location/GetBankBranchListForDDL')
            .pipe(map(bankBranchList => {
                return bankBranchList;
            }));
    }

    generateDistributorCode(selectedTerritory: string): any {
        return this.http.get<any>(this.distribution.environmentApiServer + '/Location/GenerateDistributorCode?territoryCode=' + selectedTerritory)
            .pipe(map(distributorCode => {
                return distributorCode;
            }));
    }
    generateB2bDistributorCode(selectedTerritory: string): any {
        return this.http.get<any>(this.distribution.environmentApiServer + '/Location/GenerateDistributorCode?territoryCode=' + selectedTerritory)
            .pipe(map(distributorCode => {
                return distributorCode;
            }));
    }
    GetDistributorCodeByPhotoId(pId: string): any {
        return this.http.get<any>(this.distribution.distributionApiServer + '/Distributor/GetDistributorCodeByPhotoId?pid=' + pId)
            .pipe(map(distributorCode => {
                return distributorCode;
            }));
    }

    addRemoveDormant(dormantModel, status, remarks): any {
        return this.http.post(this.distribution.distributionApiServer + '/Distributor/AddRemoveDormant?status=' + status + '&remarks=' + remarks, dormantModel)
            .pipe(map(model => {
                return model;
            }))
    }

    pinReset(model: any): any {
        return this.http.post(this.distribution.distributionApiServer + '/Distributor/PinReset', model)
            .pipe(map(data => {
                return data;
            }))
    }

    accountUnlock(model: any): any {
        return this.http.post(this.distribution.distributionApiServer + '/Distributor/PinReset?isUnlockRequest=true', model)
            .pipe(map(data => {
                return data;
            }))
    }
    getDistCodeByPmhone(value: string): any {
        return this.http.get<any>(this.distribution.distributionApiServer + '/Distributor/GetDistCodeByPmhone?pmphone=' + value)
            .pipe(map(distributorCode => {
                return distributorCode;
            }));
    }

    getDistributorListForDDL(): any {
        return this.http.get<any>(this.distribution.distributionApiServer + '/Distributor/GetDistributorListForDDL')
            .pipe(map(distributorList => {
                return distributorList;
            }));
    }

    getTotalAgentByMobileNo(ExMobileNo: any): any {
        return this.http.get<any>(this.distribution.distributionApiServer + '/Distributor/GetTotalAgentByMobileNo?ExMobileNo=' + ExMobileNo)
            .pipe(map(totalAgent => {
                return totalAgent;
            }));
    }

    ExecuteReplace(distributorReplaceModel: any): any {
        return this.http.post(this.distribution.distributionApiServer + '/Distributor/ExecuteReplace', distributorReplaceModel)
            .pipe(map(model => {
                return model;
            }))
    }

    bulkUploadExcel(formData: FormData, bulkUploadType: string, distributorAC: string, distributorCode: string, distributorName: string) {
        let headers = new HttpHeaders();

        headers.append('Content-Type', 'multipart/form-data');
        headers.append('Accept', 'application/json');

        const httpOptions = { headers: headers };

        //return this.http.post(this.url + '/UploadExcel', formData, httpOptions)
        return this.http.post<string>(this.distribution.reportingApiServer + '/ExcelUpload/BulkUploadExcel?bulkUploadType=' + bulkUploadType + '&distributorAC=' + distributorAC + '&distributorCode=' + distributorCode + '&distributorName=' + distributorName, formData, httpOptions)
            .pipe(map(data => {
                return data;
            }))
    }

    getRegionDetailsByMobileNo(mobileNo: any): any {
        return this.http.get<any>(this.distribution.distributionApiServer + '/Distributor/GetRegionDetailsByMobileNo?mobileNo=' + mobileNo)
            .pipe(map(data => {
                return data;
            }));
    }
    getDistributorListWithDistCodeForDDL() {
        return this.http.get<any>(this.distribution.distributionApiServer + '/Distributor/getDistributorListWithDistCodeForDDL')
            .pipe(map(distributorList => {
                return distributorList;
            }));
    }
    getB2bDistributorListWithDistCodeForDDL() {
        return this.http.get<any>(this.distribution.distributionApiServer + '/Distributor/GetB2bDistributorListWithDistCodeForDDL')
            .pipe(map(distributorList => {
                return distributorList;
            }));
    }
}
