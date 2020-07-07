import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class MfsModalService {

    constructor(private http: HttpClient, private sanitizer: DomSanitizer) { }

    initiateModal(): any {
        this.http.get('partial-html/generic-modal-content.html', { responseType: 'text' }).pipe(map(model => {
            return this.sanitizer.bypassSecurityTrustHtml(model);
        }));
    }
}
