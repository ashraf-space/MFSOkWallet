import { Component, OnInit } from '@angular/core';
import { MfsUtilityService } from 'src/app/services/mfs-utility.service';
import { first } from 'rxjs/operators';
import { KycService } from 'src/app/services/distribution/kyc.service';
import { KycReportService } from 'src/app/services/report/kyc-report.service';
import { NgbDatepickerConfig, NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { AuthenticationService } from 'src/app/shared/_services';

@Component({
  selector: 'app-com-bill-col',
  templateUrl: './com-bill-col.component.html',
  styleUrls: ['./com-bill-col.component.css']
})
export class ComBillColComponent implements OnInit {

    model: any;
    currentUserModel: any = {};
    rptTypeList: any;
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
        this.rptTypeList = [
            { label: 'NID Bill', value: 'NID' },
            { label: 'Lankabangla Credit Card', value: 'LBC' },
            { label: 'Lankabangla DPS', value: 'LBD' }
            
        ];

  }
    getReportParam() {
        if (this.validate()) {
            var obj: any = {};
            obj.branchCode = this.currentUserModel.user.branchCode;
            obj.reportType = this.model.reportType;
            if (this.model.fromDate && this.model.toDate) {
                obj.fromDate = this.mfsUtilityService.renderDate(this.model.fromDate, true);
                obj.toDate = this.mfsUtilityService.renderDate(this.model.toDate, true);
            }
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
