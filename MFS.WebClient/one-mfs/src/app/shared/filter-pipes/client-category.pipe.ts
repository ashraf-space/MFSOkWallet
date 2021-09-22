import { Pipe, PipeTransform } from '@angular/core';
import { retry } from 'rxjs/operators';

@Pipe({
  name: 'clientCategory'
})
export class ClientCategoryPipe implements PipeTransform {

    transform(value: any, args?: any): any {
        switch (value) {
            case 'C':
                return 'Customer ';
                break;
            case 'D':
                return 'Distributor ';
                break;
            case 'A':
                return 'Agent ';
                break;
            case 'M':
                return 'Merchant ';
                break;
            case 'R':
                return 'DSR ';
                break;
            case 'E':
                return 'Enterprise';
                break;
            case 'CM':
                return 'Retail';
                break;
            case 'BD':
                return 'B2B Distributor';
                break;
            case 'BA':
                return 'B2B DSR';
                break;
            case 'BR':
                return 'B2B Retail';
                break;
            default:
                return value;
        }
    }

}
