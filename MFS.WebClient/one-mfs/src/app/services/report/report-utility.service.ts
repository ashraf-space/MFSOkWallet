import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { MfsSettingService } from '../mfs-setting.service';

@Injectable({
  providedIn: 'root'
})
export class ReportUtilityService {
    
    fileExtensionList: any;
    constructor(private http: HttpClient, private setting: MfsSettingService) {
        this.fileExtensionList = [
            { label: 'Pdf', value: 'PDF', icon: 'fas fa-file-pdf' },
            { label: 'Excel', value: 'EXCEL', icon: 'fas fa-file-excel' },
            { label: 'Word', value: 'WORDOPENXML', icon: 'fas fa-file-word' }
        ];
    }

    getFileExtensionList() {
        return this.fileExtensionList;
    }

    generateReport(path: any, model: any) {
        return this.http.post<any>(path, model)
            .pipe(map(data => {
                return data;
            }));
    }
    diffBetweenDate(fromDate: any, toDate: any) {
        let vFromDate = new Date(fromDate);
        let vToDate = new Date(toDate);
        return Math.floor((Date.UTC(vToDate.getFullYear(), vToDate.getMonth(), vToDate.getDate()) - Date.UTC(vFromDate.getFullYear(), vFromDate.getMonth(), vFromDate.getDate())) / (1000 * 60 * 60 * 24));

    }
}
