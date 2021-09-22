import { Component, OnInit } from '@angular/core';
import { MfsUtilityService } from 'src/app/services/mfs-utility.service';
import { first } from 'rxjs/operators';
import { KycService } from 'src/app/services/distribution/kyc.service';
import { KycReportService } from 'src/app/services/report/kyc-report.service';
import { NgbDatepickerConfig, NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { AuthenticationService } from 'src/app/shared/_services';

@Component({
  selector: 'app-commission-rpt',
  templateUrl: './commission-rpt.component.html',
  styleUrls: ['./commission-rpt.component.css']
})
export class CommissionRptComponent implements OnInit {

    model: any;
    currentUserModel: any = {};
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
  }
    getReportParam() {
        if (this.validate()) {
            var obj: any = {};
            //obj.branchCode = this.currentUserModel.user.branchCode;
            if (this.model.fromDate && this.model.toDate) {
                obj.fromDate = this.mfsUtilityService.renderDate(this.model.fromDate, true);
                obj.toDate = this.mfsUtilityService.renderDate(this.model.toDate, true);
            }
            if (this.model.mphone) {
                obj.mphone = this.model.mphone;
            }
            else {
                obj.mphone = null;
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
        if (!this.model.fromDate || !this.model.toDate || !this.model.mphone) {
            return false;
        }
        else {
            return true;
        }
    }
}
