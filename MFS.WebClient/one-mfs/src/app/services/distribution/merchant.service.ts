import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MfsSettingService } from '../mfs-setting.service';
import { map } from 'rxjs/operators';

@Injectable({
    providedIn: 'root'
})
export class MerchantService {
   
    constructor(private http: HttpClient, private distribution: MfsSettingService) {

    }

    GetDistributorDataByDistributorCode(distributorCode: string): any {
        return this.http.get<any>(this.distribution.distributionApiServer + '/Merchant/GetDistributorDataByDistributorCode?distributorCode=' + distributorCode)
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
        return this.http.post<any>(this.distribution.distributionApiServer + '/Merchant/Save?isEditMode=' + isEditMode + '&evnt=' + event, regInfoModel)
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


    getMerchantCodeListForDDL(): any {
        return this.http.get<any>(this.distribution.distributionApiServer + '/Merchant/GetMerchantCodeList')
            .pipe(map(divisionList => {
                return divisionList;
            }));
    }


    getMerchantBankBranchList() {
        return this.http.get<any>(this.distribution.distributionApiServer + '/Merchant/GetMerchantBankBranchList')
            .pipe(map(merchantBankBranchList => {
                return merchantBankBranchList;
            }));
    }

    getDistrictByBank(data: any) {
        return this.http.get<any>(this.distribution.distributionApiServer + '/Merchant/GetDistrictByBank?bankCode=' + data)
            .pipe(map(bankDistrictList => {
                return bankDistrictList;
            }));
    }

    getBankBranchListByBankCodeAndDistCode(eftBankCode: any, eftDistCode: any) {
        return this.http.get<any>(this.distribution.distributionApiServer + '/Merchant/GetBankBranchListByBankCodeAndDistCode?eftBankCode=' + eftBankCode + '&eftDistCode=' + eftDistCode)
            .pipe(map(bankDistrictList => {
                return bankDistrictList;
            }));
    }

    generateMerchantCode(selectedCategory: string) {
        return this.http.get<any>(this.distribution.distributionApiServer + '/Merchant/GenerateMerchantCode?selectedCategory=' + selectedCategory)
            .pipe(map(merchantCode => {
                return merchantCode;
            }));
    }
    getMerChantByMphone(entityId: string) {
        return this.http.get<any>(this.distribution.distributionApiServer + '/Merchant/getMerChantByMphone?mPhone=' + entityId)
            .pipe(map(merchant => {
                return merchant;
            }));
    }

    checkMphoneAlreadyExist(mphone: any) {
        return this.http.get<any>(this.distribution.distributionApiServer + '/Merchant/checkMphoneAlreadyExist?mPhone=' + mphone)
            .pipe(map(merchant => {
                return merchant;
            }));
    }

    getRoutingNo(eftBankCode: any, eftDistCode: any, eftBranchCode: any) {
        return this.http.get<any>(this.distribution.distributionApiServer + '/Merchant/getRoutingNo?eftBankCode=' + eftBankCode + '&eftDistCode=' + eftDistCode + '&eftBranchCode=' + eftBranchCode)
            .pipe(map(routingNo => {
                return routingNo;
            }));
    }

    getChainMerchantList() {
        return this.http.get<any>(this.distribution.distributionApiServer + '/Merchant/GetChainMerchantList')
            .pipe(map(chainMerchantList => {
                return chainMerchantList;
            }));
    }
    getParentMerchantByMphone(selectedChainMerchant: any) {
        return this.http.get<any>(this.distribution.distributionApiServer + '/Merchant/GetParentMerchantByMphone?mphone=' + selectedChainMerchant)
            .pipe(map(parentMerchant => {
                return parentMerchant;
            }));
    }

    saveChildMerchant(regInfoModel: any, isEditMode: boolean, event: any) {
        return this.http.post<any>(this.distribution.distributionApiServer + '/Merchant/SaveChildMerchant?isEditMode=' + isEditMode + '&evnt=' + event, regInfoModel)
            .pipe(map(model => {
                return model;
            }))
    }

    getChildMerChantByMphone(entityId: string) {
        return this.http.get<any>(this.distribution.distributionApiServer + '/Merchant/GetChildMerChantByMphone?mPhone=' + entityId)
            .pipe(map(merchant => {
                return merchant;
            }));
    }
    getAllMerchant() {
        return this.http.get<any>(this.distribution.distributionApiServer + '/Merchant/GetAllMerchant')
            .pipe(map(merchantList => {
                return merchantList;
            }));
    }

    getMerChantConfigByMphone(entityId: any) {
        return this.http.get<any>(this.distribution.distributionApiServer + '/Merchant/GetMerChantConfigByMphone?mPhone=' + entityId)
            .pipe(map(merchant => {
                return merchant;
            }));
    }

    onMerchantConfigUpdate(merchantConfigModel: any, event: any) {
        return this.http.post<any>(this.distribution.distributionApiServer + '/Merchant/onMerchantConfigUpdate',merchantConfigModel)
            .pipe(map(model => {
                return model;
            }))
    }
    getMerChantUserByMphone(entityId: any) {
        return this.http.get<any>(this.distribution.distributionApiServer + '/Merchant/getMerChantUserByMphone?mPhone=' + entityId)
            .pipe(map(merchant => {
                return merchant;
            }));
    }
    onMerchantUserSave(merchantUserModel: any, isEditMode:any ,event: any) {
        return this.http.post<any>(this.distribution.distributionApiServer + '/Merchant/SaveMerchantUser?isEditMode=' + isEditMode + '&evnt=' + event, merchantUserModel)
            .pipe(map(model => {
                return model;
            }))
    }
    getMerchantList() {
        return this.http.get<any>(this.distribution.distributionApiServer + '/Merchant/getMerchantListForUser')
            .pipe(map(merchant => {
                return merchant;
            }));
    }
    checkSnameExist(orgCode: any) {
        return this.http.get<any>(this.distribution.distributionApiServer + '/Merchant/checkSnameExist?orgCode=' + orgCode)
            .pipe(map(merchant => {
                return merchant;
            }));
    }
    isMerchantAllow(catId: any) {
        if (catId === 'M' || catId === 'PR' || catId === 'D') {
            return true;
        }
        else {
            return false;
        }
    }
}
