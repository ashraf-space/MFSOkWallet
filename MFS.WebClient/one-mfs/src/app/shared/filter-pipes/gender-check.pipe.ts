import { Pipe, PipeTransform } from '@angular/core';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';  

@Pipe({
  name: 'genderCheck'
})
export class GenderCheckPipe implements PipeTransform {
    constructor(private _sanitizer: DomSanitizer) { }  

    transform(value: any, args?: any): any {
        switch (value) {
            case 'M':
                return this._sanitizer.bypassSecurityTrustHtml("<i class='fas fa-male' > </i> Male "); ;
                break;
            case 'F':
                return this._sanitizer.bypassSecurityTrustHtml("<i class='fas fa-female' > </i> Female ");
                break;
            default:
                return value;
        }
    }

}
