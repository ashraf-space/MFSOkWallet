import { Pipe, PipeTransform } from '@angular/core';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';  


@Pipe({
  name: 'yesNo'
})
export class YesNoPipe implements PipeTransform {
    constructor(private _sanitizer: DomSanitizer) { }  
    html: string;
    transform(value: any, args: any): any {
      switch (value) {
          //case 'Y':
          //    return 'Yes ';
          //    break;
          //case 'N':
          //    return 'No ';
          //    break;       
          case 'Y':
              return this._sanitizer.bypassSecurityTrustHtml("<span style='color:green;padding:0.5em;font-size: 160%;'> <i class='pi pi-check'></i> </span>");
              break;
          case 'N':
              return this._sanitizer.bypassSecurityTrustHtml("<span style='color:red;padding:0.5em;font-size: 170%;'> <i class='pi pi-times'></i>  </span>");
              break;
          default:
              return value;
      }
  }

}
