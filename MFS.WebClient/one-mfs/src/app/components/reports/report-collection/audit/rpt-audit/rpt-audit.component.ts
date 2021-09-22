import { Component, OnInit } from '@angular/core';
import { MfsUtilityService } from 'src/app/services/mfs-utility.service';
import { first } from 'rxjs/operators';
import { KycService } from 'src/app/services/distribution/kyc.service';
import { KycReportService } from 'src/app/services/report/kyc-report.service';
import { NgbDatepickerConfig, NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { AuditTrailService } from '../../../../../shared/_services/audit-trail.service';
import { BankBranchService } from '../../../../../services/environment/bank-branch.service';
import { ApplicationUserService } from '../../../../../services/security/application-user.service';
@Component({
    selector: 'app-rpt-audit',
    templateUrl: './rpt-audit.component.html',
    styleUrls: ['./rpt-audit.component.css']
})
export class RptAuditComponent implements OnInit {

    model: any;
    branchList: any;
    userList: any;
    parentMenuList: any;
    actionList: any;
    isLoading: any;
    constructor(private mfsUtilityService: MfsUtilityService,
        private auditTrailService: AuditTrailService,
        private kycReportService: KycReportService,
        private bankBranchService: BankBranchService,
        private applicationUserService: ApplicationUserService,
        private ngbDatepickerConfig: NgbDatepickerConfig) {
        ngbDatepickerConfig.minDate = { year: 1919, month: 1, day: 1 };
        var currentDate = new Date();
        ngbDatepickerConfig.maxDate = { year: currentDate.getFullYear(), month: currentDate.getMonth() + 1, day: currentDate.getDate() };
        this.model = {};
    }

    ngOnInit() {
        this.actionList = this.mfsUtilityService.getActionList();
        this.getParentMenuList();
        this.getBankBranchList();
    }
    getParentMenuList() {
        this.auditTrailService.getParentMenuList()
            .pipe(first())
            .subscribe(
                data => {
                    this.parentMenuList = data;
                },
                error => {
                    console.log(error);
                }
            );
    }
    async getBankBranchList() {
        this.bankBranchService.getBankBranchListForDDL().pipe(first())
            .subscribe(
                data => {
                    this.branchList = data;
                },
                error => {
                    console.log(error);
                });

    }
    getUserByBranchCode() {
        this.isLoading = true;
        this.applicationUserService.getAppUserListDdlForStingValue(this.model.branchCode).pipe(first())
            .subscribe(
                data => {
                    this.userList = data;
                    this.isLoading = false;
                },
                error => {
                    console.log(error);
                });
        this.isLoading = false;
    }
    getReportParam() {
        if (this.validate()) {
            var obj: any = {};
            if (this.model.fromDate && this.model.toDate) {
                obj.fromDate = this.mfsUtilityService.renderDate(this.model.fromDate, true);
                obj.toDate = this.mfsUtilityService.renderDate(this.model.toDate, true);
            }          
            if (this.model.branchCode) {
                obj.branchCode = this.model.branchCode;
            }
            else {
                obj.branchCode = null;
            }
            if (this.model.user) {
                obj.user = this.model.user;
            }
            else {
                obj.user = null;
            }
            if (this.model.parentMenu) {
                obj.parentMenu = this.model.parentMenu;
            }
            else {
                obj.parentMenu = null;
            }
            if (this.model.action) {
                obj.parentMenu = this.model.action;
            }
            else {
                obj.action = null;
            }
            if (this.model.auditId) {
                obj.auditId = this.model.auditId;
            }
            else {
                obj.auditId = null;
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
}
