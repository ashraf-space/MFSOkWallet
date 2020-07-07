import { Pipe, PipeTransform } from '@angular/core';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';  

@Pipe({
  name: 'reportType'
})
export class ReportTypePipe implements PipeTransform {
    constructor(private _sanitizer: DomSanitizer) { }

    transform(value: any, args?: any): any {
        switch (value) {
            case 'T':
                return 'Transaction';
                break;
            case 'K':
                return 'Know Your Customer [ KYC ]';
                break;
            default:
        }
    }

}
