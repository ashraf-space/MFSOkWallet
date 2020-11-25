import { Component, OnInit } from '@angular/core';
import { MfsUtilityService } from 'src/app/services/mfs-utility.service';
import { KycReportService } from 'src/app/services/report/kyc-report.service';
import { NgbDatepickerConfig, NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { RoleService } from '../../../../services/security/role.service';
import { first } from 'rxjs/operators';

@Component({
    selector: 'app-app-user',
    templateUrl: './app-user.component.html',
    styleUrls: ['./app-user.component.css']
})
export class AppUserComponent implements OnInit {
    model: any;
    roleList: any;
    constructor(private mfsUtilityService: MfsUtilityService,
        private kycReportService: KycReportService,
        private roleService: RoleService,
        private ngbDatepickerConfig: NgbDatepickerConfig) {
        ngbDatepickerConfig.minDate = { year: 1919, month: 1, day: 1 };
        var currentDate = new Date();
        ngbDatepickerConfig.maxDate = { year: currentDate.getFullYear(), month: currentDate.getMonth() + 1, day: currentDate.getDate() };
        this.model = {};
    }

    ngOnInit() {
        this.SecuredRoleList();
    }
    async SecuredRoleList() {
        this.roleService.getRoleListForDDL().pipe(first())
            .subscribe(
                data => {
                    this.roleList = data;
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
            if (this.model.branchCode) {
                obj.branchCode = this.model.branchCode;
            }
            else {
                obj.branchCode = '';
            }
            if (this.model.userName) {
                obj.userName = this.model.userName;
            }
            else {
                obj.userName = '';
            }
            if (this.model.name) {
                obj.name = this.model.name;
            }
            else {
                obj.name = '';
            }
            if (this.model.mobileNo) {
                obj.mobileNo = this.model.mobileNo;
            }
            else {
                obj.mobileNo = '';
            }
            if (this.model.roleName) {
                obj.roleName = this.model.roleName;
            }
            else {
                obj.roleName = '';
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
