import { Component, OnInit } from '@angular/core';
import { MfsUtilityService } from 'src/app/services/mfs-utility.service';
import { first } from 'rxjs/operators';
import { KycService } from 'src/app/services/distribution/kyc.service';
import { KycReportService } from 'src/app/services/report/kyc-report.service';
import { NgbDatepickerConfig, NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
@Component({
    selector: 'app-kyc-balance',
    templateUrl: './kyc-balance.component.html',
    styleUrls: ['./kyc-balance.component.css']
})
export class KycBalanceComponent implements OnInit {

    model: any;
    accountCategoryList: any;
    basedOnList: any;
    regStatusList: any;
    optionList: any;
    isDateDisabled: boolean = false;
    isAccInputDisable: boolean = false;
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
        this.regStatusList = [
            { label: 'All', value: 'All' },
            { label: 'Close', value: 'C' },
            { label: 'Logical', value: 'L' },
            { label: 'Physical', value: 'P' },
            { label: 'Reject', value: 'R' }
        ];
        this.optionList = [
            { label: 'Cumulative', value: 'CL' },
            { label: 'Period', value: 'P' },
            { label: 'Single Account', value: 'S' }
        ];
    }
    onOptionChange() {
        if (this.model.options === 'CL') {
            this.isDateDisabled = true;
            this.isAccInputDisable = true;
        }
        else if (this.model.options === 'S') {
            this.isAccInputDisable = false;
            this.isDateDisabled = true;
        }
        else {
            this.isDateDisabled = false;
            this.isAccInputDisable = true;
            this.model.fromDate = {};
            this.model.toDate = {};
        }
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
    getReportParam() {
        if (this.validate()) {
            var obj: any = {};
            if (this.model.fromDate && this.model.toDate) {
                obj.fromDate = this.mfsUtilityService.renderDate(this.model.fromDate, true);
                obj.toDate = this.mfsUtilityService.renderDate(this.model.toDate, true);
            }
            obj.basedOn = this.model.accNo;
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
        if (!this.model.regStatus || !this.model.accCategory || !this.model.options) {
            return false;
        }
        else {
            return true;
        }
    }
}
