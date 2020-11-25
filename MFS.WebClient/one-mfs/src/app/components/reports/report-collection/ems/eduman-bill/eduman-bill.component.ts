import { Component, OnInit } from '@angular/core';
import { MfsUtilityService } from 'src/app/services/mfs-utility.service';
import { first } from 'rxjs/operators';
import { KycService } from 'src/app/services/distribution/kyc.service';
import { KycReportService } from 'src/app/services/report/kyc-report.service';
import { NgbDatepickerConfig, NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';

@Component({
    selector: 'app-eduman-bill',
    templateUrl: './eduman-bill.component.html',
    styleUrls: ['./eduman-bill.component.css']
})
export class EdumanBillComponent implements OnInit {
    model: any;
    dateTypeList: any;
    utilityList: any;
    gatewayList: any;
    catTypeList: any;
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
        this.catTypeList = [
            { label: 'Agent', value: 'A' },
            { label: 'Customer', value: 'C' },
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
            if (!this.model.studentId) {
                obj.studentId = null;
            }
            else {
                obj.studentId =this.model.studentId
            }
            if (!this.model.instituteId) {
                obj.instituteId = null;
            }
            else {
                obj.instituteId = this.model.instituteId
            }
            obj.catType = this.model.catType;
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
        if (!this.model.catType || !this.model.dateType ||
            !this.model.fromDate || !this.model.toDate) {
            return false;
        }
        else {
            return true;
        }
    }

}
