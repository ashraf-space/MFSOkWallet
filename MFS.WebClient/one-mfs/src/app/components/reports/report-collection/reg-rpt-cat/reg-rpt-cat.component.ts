import { Component, OnInit } from '@angular/core';
import { MfsUtilityService } from 'src/app/services/mfs-utility.service';
import { first } from 'rxjs/operators';
import { KycService } from 'src/app/services/distribution/kyc.service';
import { KycReportService } from 'src/app/services/report/kyc-report.service';
import { NgbDatepickerConfig, NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
@Component({
  selector: 'app-reg-rpt-cat',
  templateUrl: './reg-rpt-cat.component.html',
  styleUrls: ['./reg-rpt-cat.component.css']
})
export class RegRptCatComponent implements OnInit {
    model: any;
    accountCategoryList: any;
    regSourceList: any;
    regStatusList: any;
    accountStatusList: any;
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
        this.regSourceList = [
            { label: 'Agent', value: 'A' },
            { label: 'Online', value: 'O' },
            { label: 'Bulk', value: 'B' },
            { label: 'EKYC', value: 'E' },
            { label: 'Bank User', value: 'P' },
            { label: 'Agent Online', value: 'Q' }
        ];
        this.regStatusList = [          
            { label: 'Close', value: 'C' },
            { label: 'Logical', value: 'L' },
            { label: 'Physical', value: 'P' },
            { label: 'Reject', value: 'R' }
        ];
        this.accountStatusList = [
            { label: 'Dormant', value: 'D' },
            { label: 'Active', value: 'A' },
            { label: 'Close', value: 'C' },
            { label: 'Outward Block', value: 'O' }
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
    getReportParam() {
        if (this.validate()) {
            var obj: any = {};
            if (this.model.fromDate && this.model.toDate) {
                obj.fromDate = this.mfsUtilityService.renderDate(this.model.fromDate, true);
                obj.toDate = this.mfsUtilityService.renderDate(this.model.toDate, true);
            }
            if (!this.model.regSource) {
                obj.regSource = '';
            }
            else {
                obj.regSource = this.model.regSource;
            }
            if (!this.model.status) {
                obj.status = '';
            }
            else {
                obj.status = this.model.status;
            }
            if (!this.model.accCategory) {
                obj.accCategory = '';
            }
            else {
                obj.accCategory = this.model.accCategory;
            }
            if (!this.model.regStatus) {
                obj.regStatus = '';
            }
            else {
                obj.regStatus = this.model.regStatus;
            }                                   
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
