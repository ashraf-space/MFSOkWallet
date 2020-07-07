import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { MfsSettingService } from '../mfs-setting.service';
@Injectable({
  providedIn: 'root'
})
export class ReportInfoService {
      
    constructor(private http: HttpClient, private settings: MfsSettingService) {
    }

    onRoleInfoSave(reportInfoModel: any, isEditMode: boolean, event: any) {
        return this.http.post<any>(this.settings.reportingApiServer + '/ReportInfo/SaveReportInfo?isEditMode=' + isEditMode + '&evnt=' + event, reportInfoModel)
            .pipe(map(data => {
                return data;
            }));
    }
    getReportConfigById(entityId: any) {
        return this.http.get<any>(this.settings.reportingApiServer + '/ReportInfo/GetReportConfigById?id=' + entityId)
            .pipe(map(data => {
                return data;
            }));
    }
}
