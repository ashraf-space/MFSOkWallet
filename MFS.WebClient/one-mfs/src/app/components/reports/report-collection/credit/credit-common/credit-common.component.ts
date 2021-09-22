import { Component, OnInit } from '@angular/core';
import { MfsUtilityService } from 'src/app/services/mfs-utility.service';
import { first } from 'rxjs/operators';
import { KycService } from 'src/app/services/distribution/kyc.service';
import { KycReportService } from 'src/app/services/report/kyc-report.service';
import { NgbDatepickerConfig, NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { AuthenticationService } from 'src/app/shared/_services';

@Component({
  selector: 'app-credit-common',
  templateUrl: './credit-common.component.html',
  styleUrls: ['./credit-common.component.css']
})
export class CreditCommonComponent implements OnInit {

    model: any;
    currentUserModel: any = {};
    reportTypeList: any;
    constructor(private mfsUtilityService: MfsUtilityService,
        private kycReportService: KycReportService,
        private authService: AuthenticationService,
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
            { label: 'OBL Online', value: 'OBLON' },
            { label: 'OBL Offline', value: 'OBLOFF' },
            { label: 'Other Bank', value: 'OTHBNK' }           
        ];
  }
    getReportParam() {
        if (this.validate()) {
            var obj: any = {};
            obj.branchCode = this.currentUserModel.user.branchCode;
            if (this.model.fromDate && this.model.toDate) {
                obj.fromDate = this.mfsUtilityService.renderDate(this.model.fromDate, true);
                obj.toDate = this.mfsUtilityService.renderDate(this.model.toDate, true);
            }
            obj.reportType = this.model.reportType;
            if (this.model.transNo) {
                obj.transNo = this.model.transNo;
            }
            else {
                obj.transNo = null;
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
        if (!this.model.fromDate || !this.model.toDate || !this.model.reportType) {
            return false;
        }
        else {
            return true;
        }
    }
}
