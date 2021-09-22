import { Component, OnInit } from '@angular/core';
import { NgbDatepickerConfig } from '@ng-bootstrap/ng-bootstrap';
import { first } from 'rxjs/operators';
import { DistributorService } from 'src/app/services/distribution';
import { MfsUtilityService } from 'src/app/services/mfs-utility.service';
import { KycReportService } from 'src/app/services/report/kyc-report.service';

@Component({
    selector: 'app-sourse-wise-registration',
    templateUrl: './sourse-wise-registration.component.html',
    styleUrls: ['./sourse-wise-registration.component.css']
})
export class SourseWiseRegistrationComponent implements OnInit {
    model: any;
    accountCategoryList: any;
    regSourceList: any;
    regStatusList: any;
    accountStatusList: any;
    isDateDisabled: boolean = false;
    bankBranchList: any;

    constructor(private mfsUtilityService: MfsUtilityService,
        private kycReportService: KycReportService,
        private ngbDatepickerConfig: NgbDatepickerConfig,
        private distributionService: DistributorService) {
        ngbDatepickerConfig.minDate = { year: 1919, month: 1, day: 1 };
        var currentDate = new Date();
        ngbDatepickerConfig.maxDate = { year: currentDate.getFullYear(), month: currentDate.getMonth() + 1, day: currentDate.getDate() };
        this.model = {};
    }

    ngOnInit() {
        this.getBankBranchListForDDL();
        //this.getAccountCategoryDDL();
        this.regSourceList = [
            { label: 'Agent', value: 'A' },
            { label: 'Online', value: 'O' },
            { label: 'Bulk', value: 'B' },
            { label: 'Customer Registration E-KYC', value: 'E' },
            { label: 'Agent Registration E-KYC', value: 'EA' },
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
    //getAccountCategoryDDL(): any {
    //    this.kycReportService.getAccountCategoryList()
    //        .pipe(first())
    //        .subscribe(
    //            data => {
    //                this.accountCategoryList = data;
    //            },
    //            error => {
    //                console.log(error);
    //            }
    //        );
    //}
    getReportParam() {
        if (this.validate()) {
            var obj: any = {};
            if (this.model.fromDate && this.model.toDate) {
                obj.fromDate = this.mfsUtilityService.renderDate(this.model.fromDate, true);
                obj.toDate = this.mfsUtilityService.renderDate(this.model.toDate, true);
            }
            if (!this.model.regStatus) {
                obj.regStatus = '';
            }
            else {
                obj.regStatus = this.model.regStatus;
            }
            
            if (!this.model.status) {
                obj.status = '';
            }
            else {
                obj.status = this.model.status;
            }
            if (!this.model.regSource) {
                obj.regSource = '';
            }
            else {
                obj.regSource = this.model.regSource;
            }
            if (!this.model.branchCode) {
                obj.branchCode = '';
            }
            else {
                obj.branchCode = this.model.branchCode;
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


    getBankBranchListForDDL(): any {
        this.distributionService.getBankBranchListForDDL()
            .pipe(first())
            .subscribe(
                data => {
                    this.bankBranchList = data;
                },
                error => {
                    console.log(error);
                }
            );
    }

}
