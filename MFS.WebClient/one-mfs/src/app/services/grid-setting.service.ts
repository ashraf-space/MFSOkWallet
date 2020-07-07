import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { MfsSettingService } from './mfs-setting.service';
import { retry } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class GridSettingService {

    constructor(private http: HttpClient, private setting: MfsSettingService) {
    }

    getDefaultFilterable() {
        return {
            filterType: 'default'
        };
    }

    getDefaultDateFilterable() {
        return {
            filterType: 'date',
            min: 1970,
            max: 2030
        };
    }

    getDefaultNumberFilterable(min: number, max: number) {
        return {
            filterType: 'number',
            min: min,
            max: max
        };
    }

    getMultipleOptionFilterable(optionList) {
        return {
            filterType: 'multipleOption',
            optionList: optionList           
        };
    }

    getSingleOptionFilterable(optionList) {
        return {
            filterType: 'singleOption',
            optionList: optionList
        };
    }

    getDateTemplateForRowData() {
        return {
            name: 'date'
        };
    }

    getOpenCloseStatusTemplateForRowData() {
        return {
            name: 'open-close-status'
        };
    }
    
    getMoneyTemplateForRowData() {
        return {
            name: 'money'
        };
    }

    getAccountTypesTemplateForRowData() {
        return {
            name: 'account-types'
        };
    }

    getYesNoTemplateForRowData() {
        return {
            name: 'yes-no'
        };
    } 

    getStatusTemplateForRowData() {
        return {
            name: 'status'
        };
    } 

    getFinancialStatusTemplateForRowData() {
        return {
            name: 'fin-status'
        };
    } 

    getOutboxStatusTemplateForRowData() {
        return {
            name: 'outbox-status'
        };
    } 

    getCheckStatusTemplateForRowData() {
        return {
            name: 'check-status'
        };
    }

    getPinStatusTemplateForRowData() {
        return {
            name: 'pin-status'
        };
    }

    getReportTypeTemplateForRowData() {
        return {
            name: 'report-type'
        };
    }

    getFilterableNone() {
        return {
            filterType: 'none'
        };
    }

    getFilterableNoneAndToggleSelectAll() {
        return {
            filterType: 'showtoggle'
        };
    }

    getWorklistForGridDataSource(path) {
        return this.http.get<any>(path)
            .pipe(map(data => {
                return data;
            }));
    }

    saveBatchUpdate(path, list) {
        return this.http.post(path, list)
            .pipe(map(data => {
                return data;
            }));
    }

    getCustomFilterDDL(path: any): any {
        return this.http.get<any>(path)
            .pipe(map(data => {
                return data;
            }));
    }

}
