import { Component, OnInit } from '@angular/core';
import { MfsUtilityService } from 'src/app/services/mfs-utility.service';
import { first } from 'rxjs/operators';
import { KycService } from 'src/app/services/distribution/kyc.service';
import { KycReportService } from 'src/app/services/report/kyc-report.service';
import { NgbDatepickerConfig, NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { MessageService, MenuItem } from 'primeng/api';

@Component({
    selector: 'app-online-reg',
    templateUrl: './online-reg.component.html',
    styleUrls: ['./online-reg.component.css']
})
export class OnlineRegComponent implements OnInit {

    model: any;
    categoryList: any;
    statusList: any;
    isOkAccExist: any;
    constructor(private mfsUtilityService: MfsUtilityService,
        private messageService: MessageService,
        private kycReportService: KycReportService,
        private ngbDatepickerConfig: NgbDatepickerConfig) {
        ngbDatepickerConfig.minDate = { year: 1919, month: 1, day: 1 };
        var currentDate = new Date();
        ngbDatepickerConfig.maxDate = { year: currentDate.getFullYear(), month: currentDate.getMonth() + 1, day: currentDate.getDate() };
        this.model = {};
    }

    ngOnInit() {
        this.categoryList = [
            { label: 'Customer Self Registration', value: 'O' },
            { label: 'Agent Registration', value: 'Q' },
            { label: 'Customer Registration E-KYC', value: 'E' },
            { label: 'Agent Registration E-KYC', value: 'EA' }
        ];
        this.statusList = [
            { label: 'Logical', value: 'L' },
            { label: 'Physical', value: 'P' }
        ];
    }

    getReportParam() {
        if (this.validate()) {
            var obj: any = {};
            if (this.model.fromDate && this.model.toDate) {
                obj.fromDate = this.mfsUtilityService.renderDate(this.model.fromDate, true);
                obj.toDate = this.mfsUtilityService.renderDate(this.model.toDate, true);
            }
            else {
                obj.fromDate = null;
                obj.toDate = null;
            }
            if (this.model.category) {
                obj.category = this.model.category;
            }
            else {
                obj.category = null;
            }
            if (this.model.regStatus) {
                obj.regStatus = this.model.regStatus;
            }
            else {
                obj.regStatus = null;
            }
            if (this.model.accNo) {
                obj.accNo = this.model.accNo;
            }
            else {
                obj.accNo = null;
            }

            return obj;
        }
        else {
            var obj: any = {};
            obj.isNotValidated = true;
            return obj;
        }
    }
    disabledDatepicker() {
        if (this.model.accNo) {
            this.isOkAccExist = true;
            this.model.toDate = null;
            this.model.fromDate = null;
            this.model.category = null;
            this.model.regStatus = null;
        }
        else {
            this.isOkAccExist = false;
        }
    }
    diffBetweenDate(fromDate: any, toDate: any): any {
        let vFromDate = new Date(fromDate);
        let vToDate = new Date(toDate);

        return Math.floor((Date.UTC(vToDate.getFullYear(), vToDate.getMonth(), vToDate.getDate()) - Date.UTC(vFromDate.getFullYear(), vFromDate.getMonth(), vFromDate.getDate())) / (1000 * 60 * 60 * 24));

    }
    validate(): any {
        if ((!this.model.category && !this.model.fromDate && !this.model.toDate && !this.model.regStatus) && this.model.accNo) {
            return true;
        }
        if (!this.model.category && !this.model.fromDate && !this.model.toDate && !this.model.accNo && !this.model.regStatus) {
            return false;
        }
        if (!this.model.category || !this.model.fromDate || !this.model.toDate || !this.model.regStatus) {
            return false;
        }
        else {
            var gapDays = 0;
            gapDays = this.diffBetweenDate(this.mfsUtilityService.renderDate(this.model.fromDate, true), this.mfsUtilityService.renderDate(this.model.toDate, true));
            if (gapDays > 2) {
                this.messageService.add({ severity: 'warn', summary: 'Long Date Interval', sticky: true, detail: 'Please Dont select the range more than two day' });
                return false;
            }
            return true;
        }
    }

}
