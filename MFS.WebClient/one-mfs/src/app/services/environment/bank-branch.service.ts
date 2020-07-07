import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { MfsSettingService } from '../mfs-setting.service';

@Injectable({
    providedIn: 'root'
})
export class BankBranchService {

    constructor(private http: HttpClient, private environment: MfsSettingService) {

    }

    getBankBranchListForDDL() {
        return this.http.get<any>(this.environment.environmentApiServer + '/BankBranch/GetBankBranchListForDDL')
            .pipe(map(bankBranchList => {
                return bankBranchList;
            }));
    }

    save(bankBranchModel: any, isEditMode: boolean) {
        return this.http.post<any>(this.environment.environmentApiServer + '/BankBranch/Save?isEditMode=' + isEditMode, bankBranchModel)
            .pipe(map(model => {
                return model;
            }));
    }

    getBankBranchList() {
        return this.http.get<any>(this.environment.environmentApiServer + '/BankBranch/GetBankBranchList')
            .pipe(map(bankBranchList => {
                return bankBranchList;
            }));
    }

    getBankBranchById(entityId: string) {
        return this.http.get<any>(this.environment.environmentApiServer + '/BankBranch/GetBankBranchById?branchCode=' + entityId)
            .pipe(map(model => {
                return model;
            }));
    }
}
