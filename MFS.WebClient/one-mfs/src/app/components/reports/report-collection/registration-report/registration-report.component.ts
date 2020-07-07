import { Component, OnInit } from '@angular/core';
import { MfsUtilityService } from 'src/app/services/mfs-utility.service';
import { first } from 'rxjs/operators';
import { KycService } from 'src/app/services/distribution/kyc.service';
import { KycReportService } from 'src/app/services/report/kyc-report.service';
import { NgbDatepickerConfig, NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';

@Component({
    selector: 'registration-report',
    templateUrl: './registration-report.component.html',
    styleUrls: ['./registration-report.component.css']
})
export class RegistrationReportComponent implements OnInit {

    model: any;
    accountCategoryList: any;
    basedOnList: any;
    regStatusList: any;
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
        this.getAccountCategoryDDL();
        this.basedOnList = [
            { label: 'Create Date', value: 'CD' },
            { label: 'Approved Date', value: 'AD' }           
        ];
        this.regStatusList = [
            { label: 'All', value: 'All' },
            { label: 'Close', value: 'C' },
            { label: 'Logical', value: 'L' },
            { label: 'Physical', value: 'P' },
            { label: 'Reject', value: 'R' }
        ];
        this.optionList = [
            { label: 'Cumulative', value: 'CL' },
            { label: 'Period', value: 'P' }
        ];
    }
    getAccountCategoryDDL(): any {
        this.kycReportService.getAccountCategoryList()
            .pipe(first())
            .subscribe(
                data => {
                    this.accountCategoryList = data;
                },
                error => {
                    console.log(error);
                }
            );
    }
    onOptionChange() {
        if (this.model.options === 'CL') {
            this.isDateDisabled = true;
        }
        else {
            this.isDateDisabled = false;
            this.model.fromDate = {};
            this.model.toDate = {};
        }
    }


    getReportParam() {
        if (this.validate()) {
            var obj: any = {};
            if (this.model.fromDate && this.model.toDate) {
                obj.fromDate = this.mfsUtilityService.renderDate(this.model.fromDate, true);
                obj.toDate = this.mfsUtilityService.renderDate(this.model.toDate, true);
            }                     
            obj.basedOn = this.model.basedOn;
            obj.options = this.model.options;
            obj.accCategory = this.model.accCategory;
            obj.regStatus = this.model.regStatus;
            return obj;
        }
        else {
            var obj: any = {};
            obj.isNotValidated = true;
        }
        return this.model;
    }

    validate(): any {
        if (!this.model.basedOn || !this.model.regStatus || !this.model.accCategory || !this.model.options) {
            return false;
        }
        else {
            return true;
        }
    }

}
