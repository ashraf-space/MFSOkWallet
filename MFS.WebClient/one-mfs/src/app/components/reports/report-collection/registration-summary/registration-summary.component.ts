import { Component, OnInit } from '@angular/core';
import { MfsUtilityService } from 'src/app/services/mfs-utility.service';
import { first } from 'rxjs/operators';
import { KycService } from 'src/app/services/distribution/kyc.service';
import { KycReportService } from 'src/app/services/report/kyc-report.service';
import { NgbDatepickerConfig, NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';

@Component({
    selector: 'app-registration-summary',
    templateUrl: './registration-summary.component.html',
    styleUrls: ['./registration-summary.component.css']
})
export class RegistrationSummaryComponent implements OnInit {
    model: any;
    optionList: any;
    isDateDisabled: boolean = false;
    constructor(private mfsUtilityService: MfsUtilityService,
        private kycReportService: KycReportService,
        private ngbDatepickerConfig: NgbDatepickerConfig) {
        ngbDatepickerConfig.minDate = { year: 1919, month: 1, day: 1 };
        var currentDate = new Date();
        ngbDatepickerConfig.maxDate = { year: currentDate.getFullYear(), month: currentDate.getMonth() + 1, day: currentDate.getDate() };
        this.model = {};
    }

    ngOnInit() {
        this.optionList = [
            { label: 'Cumulative', value: 'CL' },
            { label: 'Period', value: 'P' }
        ];
    }

    getReportParam() {
        if (this.validate()) {
            var obj: any = {};
            if (this.model.fromDate && this.model.toDate) {
                obj.fromDate = this.mfsUtilityService.renderDate(this.model.fromDate, true);
                obj.toDate = this.mfsUtilityService.renderDate(this.model.toDate, true);
            }
                obj.options = this.model.options;
                return obj;
            }
            else {
                var obj: any = {};
                obj.isNotValidated = true;
            }
        return this.model;
    }
    onOptionChange() {
        if (this.model.options === 'CL') {
            this.isDateDisabled = true;
        }
        else {
            this.isDateDisabled = false;
        }
    }
    validate(): any {
        if (!this.model.options) {
            return false;
        }
        else {
            return true;
        }
    }

}
