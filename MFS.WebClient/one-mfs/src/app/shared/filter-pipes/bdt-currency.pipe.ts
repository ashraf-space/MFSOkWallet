import { Pipe, PipeTransform } from '@angular/core';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';  

@Pipe({
  name: 'bdtCurrency'
})
export class BdtCurrencyPipe implements PipeTransform {
    constructor(private _sanitizer: DomSanitizer) { } 

    transform(value: any, args: any): any {
        return this._sanitizer.bypassSecurityTrustHtml("BDT  " + value );
    }
}
