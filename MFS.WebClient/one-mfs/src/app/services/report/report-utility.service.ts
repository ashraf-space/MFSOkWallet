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
}
