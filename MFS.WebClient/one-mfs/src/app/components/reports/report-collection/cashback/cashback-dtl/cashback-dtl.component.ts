import { Component, OnInit } from '@angular/core';
import { MfsUtilityService } from 'src/app/services/mfs-utility.service';
import { first } from 'rxjs/operators';
import { KycService } from 'src/app/services/distribution/kyc.service';
import { KycReportService } from 'src/app/services/report/kyc-report.service';
import { NgbDatepickerConfig, NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { AuthenticationService } from 'src/app/shared/_services';

@Component({
    selector: 'app-cashback-dtl',
    templateUrl: './cashback-dtl.component.html',
    styleUrls: ['./cashback-dtl.component.css']
})
export class CashbackDtlComponent implements OnInit {
    model: any;
    reportTypeList: any;
    cashbackList: any;
    isDateDisabled: boolean = false;
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
        this.reportTypeList = [
            { label: 'Detalis Report', value: 'dtl' },
            { label: 'Summary Report', value: 'sum' }
        ]
        this.getCashBackList();
    }
    async getCashBackList() {
        this.kycReportService.getCashBackList().pipe(first())
            .subscribe(
                data => {
                    this.cashbackList = data;
                },
                error => {
                    console.log(error);
                });

    }
    getReportParam() {
        if (this.validate()) {
            var obj: any = {};
            if (this.model.fromDate && this.model.toDate) {
                obj.fromDate = this.mfsUtilityService.renderDate(this.model.fromDate, true);
                obj.toDate = this.mfsUtilityService.renderDate(this.model.toDate, true);
            }
            
            obj.reportType = this.model.reportType;
            obj.cbType = this.model.cbType;
            return obj;
        }
        else {
            var obj: any = {};
            obj.isNotValidated = true;
            return obj;
        }
    }

    validate(): any {
        if (!this.model.fromDate || !this.model.toDate || !this.model.reportType || !this.model.cbType) {
            return false;
        }
        else {
            return true;
        }
    }

}
