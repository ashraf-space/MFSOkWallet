import { Injectable } from '@angular/core';
import { MfsSettingService } from '../mfs-setting.service';
import { map, first } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class QrCodeService {
    constructor(private http: HttpClient, private mfsSettings: MfsSettingService) {

    }
    //getQrImage(): any {
    //    return this.httpClient.get(this.mfsSettings.distributionApiServer +'/Kyc/GetQrCode', { responseType: 'json' });
    //}

    getQrImage(qrCodeNumber: any): any {
        return this.http.get<any>(this.mfsSettings.distributionApiServer + '/Kyc/GetQrCode?qrCodeNumber= ' + qrCodeNumber)
            .pipe(map(data => {
                return data;
            }));
    }
   //getQrImage(): Observable<ArrayBuffer> {
    //    return this.httpClient.get('https://picsum.photos/200/300', { responseType: 'arraybuffer' });
    //}
}
