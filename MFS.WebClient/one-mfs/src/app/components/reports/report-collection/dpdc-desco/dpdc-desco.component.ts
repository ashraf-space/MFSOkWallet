import { Component, OnInit } from '@angular/core';
import { MfsUtilityService } from 'src/app/services/mfs-utility.service';
import { first } from 'rxjs/operators';
import { KycService } from 'src/app/services/distribution/kyc.service';
import { KycReportService } from 'src/app/services/report/kyc-report.service';
import { NgbDatepickerConfig, NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { AuthenticationService } from 'src/app/shared/_services';

@Component({
    selector: 'app-dpdc-desco',
    templateUrl: './dpdc-desco.component.html',
    styleUrls: ['./dpdc-desco.component.css']
})
export class DpdcDescoComponent implements OnInit {

    model: any;
    dateTypeList: any;
    utilityList: any;
    gatewayList: any;
    catTypeList: any;
    isDateDisabled: boolean = false;
    currentUserModel: any = {};
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
        this.utilityList = [
            { label: 'DPDC Postpaid', value: 'dpdc' },
            { label: 'DPDC Prepaid', value: 'dpdck' },
            { label: 'DESCO', value: 'desco' },
            { label: 'WASA Dhaka', value: 'wasa' },
            { label: 'JALALABAD GAS', value: 'jgtd' },
            { label: 'West Zone Power (Prepaid)', value: 'wzpdcl' },
            { label: 'Desco Prepaid', value: 'descop' },
            { label: 'BGDCL', value: 'bgdcl' },
            { label: 'WASA Khulna', value: 'kwasa' },
            { label: 'West Zone Power (Postpaid)', value: 'wzpdclpo' }
        ];
        this.gatewayList = [
            { label: 'USSD', value: 'U' },
            { label: 'APP', value: 'A' },
            { label: 'Branch', value: 'All' },
            { label: 'ALL', value: 'All' }

        ];
        this.catTypeList = [
            { label: 'Agent', value: 'A' },
            { label: 'Customer', value: 'C' },
            { label: 'Branch Teller', value: 'All' },
            { label: 'ALL', value: 'All' }

        ];
        this.dateTypeList = [
            { label: 'EOD Date', value: 'eod' },
            { label: 'Transaction Date', value: 'trans' }
        ]
    }



    getReportParam() {
        if (this.validate()) {
            var obj: any = {};
            if (this.model.fromDate && this.model.toDate) {
                obj.fromDate = this.mfsUtilityService.renderDate(this.model.fromDate, true);
                obj.toDate = this.mfsUtilityService.renderDate(this.model.toDate, true);
            }
            obj.utility = this.model.utility;
            obj.gateway = 'All';
            obj.catType = 'All';
            obj.dateType = this.model.dateType;
            obj.branchCode = this.currentUserModel.user.branchCode;
            return obj;
        }
        else {
            var obj: any = {};
            obj.isNotValidated = true;
            return obj;
        }
    }

    validate(): any {
        if (!this.model.utility  || !this.model.dateType ||
            !this.model.fromDate || !this.model.toDate) {
            return false;
        }
        else {
            return true;
        }
    }

}
