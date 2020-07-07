import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'securedData'
})
export class SecuredDataPipe implements PipeTransform {

    transform(value: any, args?: any): any {
        if (value && value.length > 3) {
            return value.substring(0, 2) + '****-***' + value.substr(-4);
        }
        else {
            return value;
        }
  }

}
