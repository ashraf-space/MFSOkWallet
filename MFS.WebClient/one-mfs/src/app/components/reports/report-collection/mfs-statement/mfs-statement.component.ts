import { Component, OnInit } from '@angular/core';
import { MessageService } from 'primeng/api';
import { first } from 'rxjs/operators';
import { MfsUtilityService } from 'src/app/services/mfs-utility.service';

@Component({
    selector: 'app-mfs-statement',
    templateUrl: './mfs-statement.component.html',
    styleUrls: ['./mfs-statement.component.css']
})
export class MfsStatementComponent implements OnInit {
    model: any;
    isLoading: boolean = false;
    error: boolean = false;
    yearList: any=[];
    monthList: any;

    constructor(private messageService: MessageService, private mfsUtilityService: MfsUtilityService) {
        this.model = {};
    }

    ngOnInit() {        

        this.monthList = [
            { label: 'January', value: '01' },
            { label: 'February', value: '02' },
            { label: 'March', value: '03' },
            { label: 'April', value: '04' },
            { label: 'May', value: '05' },
            { label: 'June', value: '06' },
            { label: 'July', value: '07' },
            { label: 'August', value: '08' },
            { label: 'September', value: '09' },
            { label: 'October', value: '10' },
            { label: 'November', value: '11' },
            { label: 'December', value: '12' },
        ];

        var year = new Date().getFullYear();
        for (var i = 0; i <= 12; i++) {
            this.yearList.push({ label: (year - i), value: (year - i) });
        }


    }

    //getYearForDDL(): any {
    //    this.distributionService.getBankBranchListForDDL()
    //        .pipe(first())
    //        .subscribe(
    //            data => {
    //                this.bankBranchList = data;
    //            },
    //            error => {
    //                console.log(error);
    //            }
    //        );
    //}

    getReportParam() {
        if (this.validate()) {
            var obj: any = {};
            obj.year = this.model.year;
            obj.month = this.model.month;
          
            return obj;
        }
        else {
            var obj: any = {};
            obj.isNotValidated = true;
        }
    }

    validate(): any {

        if (!this.model.month || !this.model.year) {
            return false;
        }
        else {
            return true;
        }


    }

}
