import { Component, OnInit } from '@angular/core';
import { MfsUtilityService } from 'src/app/services/mfs-utility.service';

@Component({
    selector: 'app-jgbill-daily-details',
    templateUrl: './jgbill-daily-details.component.html',
    styleUrls: ['./jgbill-daily-details.component.css']
})
export class JgbillDailyDetailsComponent implements OnInit {

    model: any;
    isLoading: boolean = false;
    error: boolean = false;
    constructor(private mfsUtilityService: MfsUtilityService) {
        this.model = {};
    }

    ngOnInit() {
    }

    getReportParam() {
        if (this.validate()) {
            var obj: any = {};
            obj.fromDate = this.mfsUtilityService.renderDate(this.model.fromDate, true);
            obj.toDate = this.mfsUtilityService.renderDate(this.model.toDate, true);       
                       
            return obj;
        }
        else {
            var obj: any = {};
            obj.isNotValidated = true;
            return obj;
        }
    }

    validate(): any {
        if (!this.model.fromDate || !this.model.toDate) {
            return false;
        }
        else {
            return true;
        }
    }

}
