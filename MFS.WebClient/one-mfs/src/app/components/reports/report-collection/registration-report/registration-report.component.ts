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
    subAccList: any;
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
        this.getSubAccountCategoryDDL();
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
            { label: 'Period', value: 'P' },
            { label: 'Cumulative', value: 'CL' }
            
        ];
        //this.subAccList = [
        //    { label: 'All', value: 'All' },
        //    { label: 'General', value: '1' },
        //    { label: 'Corona RMG 0.40', value: '2' },
        //    { label: 'PAYROLL 1.00', value: '3' },
        //    { label: 'OBL Employee', value: '4' },
        //    { label: 'Corona RMG 0.80', value: '5' },
        //    { label: 'PAYROLL 0.90', value: '6' },
        //    { label: 'Payroll 0.70 (Agent 0.90)', value: '7' },
        //    { label: 'PAYROLL ATM Free (Br 0.5, Agent 0.9)', value: '8' }
            
        //];
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
    getSubAccountCategoryDDL(): any {
        this.kycReportService.getSubAccountCategoryDDL()
            .pipe(first())
            .subscribe(
                data => {
                    this.subAccList = data;
                    this.subAccList.push({ label: 'All', value: 'All' });
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
            obj.accCategorySub = this.model.accCategorySub;
            obj.regStatus = this.model.regStatus;
            return obj;
        }
        else {
            var obj: any = {};
            obj.isNotValidated = true;
            return obj;
        }    
    }

    validate(): any {
        if (!this.model.basedOn || !this.model.regStatus || !this.model.accCategory || !this.model.options || !this.model.accCategorySub) {
            return false;
        }
        else {
            return true;
        }
    }

}
