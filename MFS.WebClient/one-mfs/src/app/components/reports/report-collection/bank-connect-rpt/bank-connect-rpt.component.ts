import { Component, OnInit } from '@angular/core';
import { MfsUtilityService } from 'src/app/services/mfs-utility.service';
import { first } from 'rxjs/operators';
import { KycService } from 'src/app/services/distribution/kyc.service';
import { KycReportService } from 'src/app/services/report/kyc-report.service';
import { NgbDatepickerConfig, NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { AuthenticationService } from 'src/app/shared/_services';

@Component({
    selector: 'app-bank-connect-rpt',
    templateUrl: './bank-connect-rpt.component.html',
    styleUrls: ['./bank-connect-rpt.component.css']
})
export class BankConnectRptComponent implements OnInit {

    model: any;
    dateTypeList: any;
    fromCategoryList: any;
    toCategoryList: any;
    catTypeList: any;
    isDateDisabled: boolean = false;
    currentUserModel: any = {};
    particularList: any;
    reportTypeList: any;
    constructor(private mfsUtilityService: MfsUtilityService,
        private authService: AuthenticationService,
        private kycReportService: KycReportService,
        private ngbDatepickerConfig: NgbDatepickerConfig) {
        this.authService.currentUser.subscribe(x => {
            this.currentUserModel = x;
        });
        ngbDatepickerConfig.minDate = { year: 1919, month: 1, day: 1 };
        var currentDate = new Date();
        ngbDatepickerConfig.maxDate = { year: currentDate.getFullYear(), month: currentDate.getMonth() + 1, day: currentDate.getDate() };
        this.model = {};
    }


    ngOnInit() {
        this.reportTypeList = [
            { label: 'Details Report', value: 'dtl' },
            { label: 'Summary Report', value: 'sum' }
        ];
        this.dateTypeList = [
            { label: 'Transaction Date', value: 'trans' },
            { label: 'Eod Date', value: 'eod' }
        ];
        this.fromCategoryList = [
            { label: 'BALANCE WITH BANK-03', value: 'MTB' },
            { label: 'CUSTOMER DEPOSIT', value: 'C' }
        ];
        this.toCategoryList = [
            { label: 'BALANCE WITH BANK-03', value: 'MTB' },
            { label: 'CUSTOMER DEPOSIT', value: 'C' }
        ];
        this.particularList = [
            { label: 'BALANCE WITH BANK-03 (MTB) => CUSTOMER WALLET', value: 'BWB3TOCW' },
            { label: 'CUSTOMER WALLET => BALANCE WITH BANK-03 (MTB)', value: 'CWTOBWB3' },
            { label: 'BALANCE WITH BANK - 02(JBL) => CUSTOMER WALLET', value: 'BWB2TOCW' },
            { label: 'CUSTOMER ACCOUNT => PAYABLE TO OTHER BANK (JBL)', value: 'CWTOPTOBJBL' },
            { label: 'BALANCE WITH BANK-01 (BBL) => CUSTOMER WALLET', value: 'BWB1TOCW' },
            { label: 'CUSTOMER ACCOUNT => PAYABLE TO OTHER BANK (BBL)', value: 'CATOPTOBBBL' }
        ]
    }
    getReportParam() {
        if (this.validate()) {
            var obj: any = {};
            if (this.model.fromDate && this.model.toDate) {
                obj.fromDate = this.mfsUtilityService.renderDate(this.model.fromDate, true);
                obj.toDate = this.mfsUtilityService.renderDate(this.model.toDate, true);
            }
            //obj.fromCatId = this.model.fromCatId;
            //obj.toCatId = this.model.toCatId;
            obj.particular = this.model.particular;
            obj.reportType = this.model.reportType;
            obj.dateType = this.model.dateType;
            return obj;
        }
        else {
            var obj: any = {};
            obj.isNotValidated = true;
            return obj;
        }
    }

    validate(): any {
        if (!this.model.particular || !this.model.dateType ||
            !this.model.fromDate || !this.model.toDate) {
            return false;
        }
        else {
            return true;
        }
    }


}
