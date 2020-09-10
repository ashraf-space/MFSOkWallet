import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { MfsSettingService } from '../mfs-setting.service';

@Injectable({
  providedIn: 'root'
})
export class AgentService {      
    
    constructor(private http: HttpClient, private environment: MfsSettingService) {

    }

    GetclusterByTerritoryCode(code : string) {
        return this.http.get<any>(this.environment.distributionApiServer + '/agent/GetclusterByTerritoryCode?code='+code)
            .pipe(map(clusterList => {
                return clusterList;
            }));
    }

    save(agentModel: any, isEditMode: boolean, event: string) {
        return this.http.post<any>(this.environment.distributionApiServer + '/agent/saveagent?isEditMode=' + isEditMode + '&evnt=' + event, agentModel)
            .pipe(map(model => {
                return model;
            }));
    }

    getAgentList() {
        return this.http.get<any>(this.environment.distributionApiServer + '/agent/getagents')
            .pipe(map(agentList => {
                return agentList;
                console.log(agentList);
            }));
    }
    GenerateAgentCode(value: string) {
        return this.http.get<any>(this.environment.distributionApiServer + '/agent/GenerateAgentCode?Code=' + value)
            .pipe(map(agentCode => {
                return agentCode;
            }));
    }

    GetAgentByMobilePhone(mPhone: string) {
        return this.http.get<any>(this.environment.distributionApiServer + '/agent/GetAgentByMobilePhone?mPhone=' + mPhone)
            .pipe(map(regInfoModel => {
                return regInfoModel;
            }));
    }

    fillClusterDDL(): any {
        return this.http.get<any>(this.environment.environmentApiServer + '/Location/GetClustersDDL')
            .pipe(map(clusterList => {
                return clusterList;
            }));
    }
    //getBankBranchById(entityId: number) {
    //    return this.http.get<any>(this.environment.environmentApiServer + '/BankBranch/GetBankBranchById?id=' + entityId)
    //        .pipe(map(model => {
    //            return model;
    //        }));
    //}

    getDistCodeByAgentInfo(territoryCode: any, companyName: any, offAddr: any) {
        return this.http.get<any>(this.environment.distributionApiServer + '/agent/getDistCodeByAgentInfo?territoryCode='
            + territoryCode + '&companyName=' + companyName + '&offAddr=' + offAddr)
            .pipe(map(distCode => {
                return distCode;
            }));
    }

    GetAgentPhoneCodeListByCluster(selectedCluster: string): any {
        return this.http.get<any>(this.environment.distributionApiServer + '/agent/GetAgentPhoneCodeListByCluster?cluster=' + selectedCluster)
            .pipe(map(data => {
                return data;
            }));
    }

    ExecuteAgentReplace(exMobileNo: any, newMobileNo: any, exCluster: any, newCluster: any, entryBy: any, AgentPhoneCodeModel: any): any {
        return this.http.post<any>(this.environment.distributionApiServer + '/agent/ExecuteAgentReplace?exMobileNo=' + exMobileNo + '&newMobileNo=' + newMobileNo + '&exCluster=' + exCluster + ' &newCluster=' + newCluster + '&entryBy=' + entryBy, AgentPhoneCodeModel)
            .pipe(map(model => {
                return model;
            }))
    } 
   

}
