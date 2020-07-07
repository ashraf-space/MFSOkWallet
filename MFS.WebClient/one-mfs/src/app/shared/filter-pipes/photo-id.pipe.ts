import { Pipe, PipeTransform } from '@angular/core';
import { DistributorService } from 'src/app/services/distribution';
import { first, retry } from 'rxjs/operators';

@Pipe({
  name: 'photoId'
})
export class PhotoIdPipe implements PipeTransform {
    photoIDTypeList: any;

    constructor(private distributionService: DistributorService) {
        this.distributionService.getPhotoIDTypeListForDDL()
            .pipe(first())
            .subscribe(
                data => {
                    this.photoIDTypeList = data;
                },
                error => {
                    console.log(error);
                }
            );  
    }

    transform(value: any, args?: any): any {
        var typeName; 
        if (this.photoIDTypeList.length != 0) {
            this.photoIDTypeList.forEach(x => {
                if (x.value == value) {
                    typeName = x.label;
                }
            })
        }

        return typeName;
    }

}
