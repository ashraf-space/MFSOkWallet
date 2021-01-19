import { Component, OnInit } from '@angular/core';
import { MfsUtilityService } from 'src/app/services/mfs-utility.service';
import { first } from 'rxjs/operators';
import { KycService } from 'src/app/services/distribution/kyc.service';
import { KycReportService } from 'src/app/services/report/kyc-report.service';
import { NgbDatepickerConfig, NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { ReportUtilityService } from '../../../../../services/report/report-utility.service';
import { MessageService, MenuItem } from 'primeng/api';
import { AuthenticationService } from 'src/app/shared/_services';

@Component({
  selector: 'app-nesco-bill-rpt',
  templateUrl: './nesco-bill-rpt.component.html',
  styleUrls: ['./nesco-bill-rpt.component.css']
})
export class NescoBillRptComponent implements OnInit {

    model: any;
    dateTypeList: any;
    utilityList: any;
    gatewayList: any;
    catTypeList: any;
    isDateDisabled: boolean = false;
    rptTypeList: any;
    currentUserModel: any = {};
    constructor(private mfsUtilityService: MfsUtilityService,
        private kycReportService: KycReportService,
        private reportUtilityService: ReportUtilityService,
        private authService: AuthenticationService,
        private messageService: MessageService,
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
            { label: 'Daily Details Report', value: 'DDR' },
            { label: 'Daily S&D Wise Summary Report', value: 'DSS' },
            { label: 'Monthly S&D Wise Summary Report', value: 'MSS' },
            { label: 'Mothly Date-wise Summary (MDS)', value: 'MDS' }
        ];
        
  }
    getReportParam() {
        var isValidate = this.validate();
        if (isValidate) {
            var obj: any = {};
            obj.branchCode = this.currentUserModel.user.branchCode;
            if (this.model.fromDate && this.model.toDate) {
                obj.fromDate = this.mfsUtilityService.renderDate(this.model.fromDate, true);
                obj.toDate = this.mfsUtilityService.renderDate(this.model.toDate, true);
            }
            if (!this.model.transNo) {
                obj.transNo = null;
            }
            else {
                obj.transNo = this.model.transNo
            }
            obj.reportType = this.model.reportType;
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
            if (this.model.reportType === 'DDR' || this.model.reportType === 'DSS') {
                var gapDay = 0;
                gapDay = +this.reportUtilityService.diffBetweenDate(this.mfsUtilityService.renderDate(this.model.fromDate, true), this.mfsUtilityService.renderDate(this.model.toDate, true));
                if (gapDay > 0) {
                    this.messageService.add({ severity: 'warn', summary: 'Long Date Interval', sticky: true, detail: 'Please Dont select the range more than One Day' });
                    return false;
                }
                else {
                    return true;
                }
            }
            else if (this.model.reportType === 'MSS' || this.model.reportType === 'MDS') {
                var gapDay = 0;
                gapDay = +this.reportUtilityService.diffBetweenDate(this.mfsUtilityService.renderDate(this.model.fromDate, true), this.mfsUtilityService.renderDate(this.model.toDate, true));
                if (gapDay > 31) {
                    this.messageService.add({ severity: 'warn', summary: 'Long Date Interval', sticky: true, detail: 'Please Dont select the range more than One Month' });
                    return false;
                }
                else {
                    return true;
                }
            }           
        }
    }

}
