import { Component, OnInit } from '@angular/core';
import { MessageService } from 'primeng/api';
import { first } from 'rxjs/operators';
import { DistributorService } from 'src/app/services/distribution';
import { MfsUtilityService } from 'src/app/services/mfs-utility.service';
import { ApplicationUserService } from 'src/app/services/security';

@Component({
  selector: 'app-branch-wise-count',
  templateUrl: './branch-wise-count.component.html',
  styleUrls: ['./branch-wise-count.component.css']
})
export class BranchWiseCountComponent implements OnInit {
    model: any;
    isLoading: boolean = false;
    tansactionTypeDDL: any;
    optionDDL: any;
    isDateRangeShow: boolean = false;
    error: boolean = false;
    bankBranchList: any;
    userNameList: any;

    constructor(private messageService: MessageService, private mfsUtilityService: MfsUtilityService
        , private distributionService: DistributorService
        , private applicationUserService: ApplicationUserService) {
        this.model = {};
    }

    ngOnInit() {

        this.getBankBranchListForDDL();
        //this.getUserListDdl();
       
        this.optionDDL = [
            { label: 'Cumulative', value: 'Cumulative' },
            { label: 'Period', value: 'Period' }
        ];
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

    //getUserListDdl() {
    //    this.applicationUserService.getAppUserListDdlForStingValue()
    //        .pipe(first())
    //        .subscribe(
    //            data => {
    //                this.userNameList = data;
    //            },
    //            error => {
    //                console.log(error);
    //            }
    //        );
    //}

    loadUserByBranch() {
        if (this.model.branchCode) {
            this.applicationUserService.getAppUserListDdlForStingValue(this.model.branchCode)
                .pipe(first())
                .subscribe(
                    data => {
                        this.userNameList = data;
                    },
                    error => {
                        console.log(error);
                    }
                );
        }
        else {
            this.userNameList = null;
        }
    }

    showDateRangeDiv() {
        if (this.model.option) {
            if (this.model.option == 'Period') {
                this.isDateRangeShow = true;
            }
            else {
                this.isDateRangeShow = false;
            }
        }
        else {
            this.messageService.add({ severity: 'warn', summary: 'Option Empty', detail: 'Select Option First!' });
            this.model.option = '';
        }
    }

    getReportParam() {
        if (this.validate()) {
            var obj: any = {};
            obj.branchCode = this.model.branchCode;
            obj.userId = this.model.userId;
            obj.option = this.model.option;
            if (this.model.option == "Period") {
                obj.fromDate = this.mfsUtilityService.renderDate(this.model.fromDate, true);
                obj.toDate = this.mfsUtilityService.renderDate(this.model.toDate, true);
            }
            else {
                obj.fromDate = null;
                obj.toDate = null;
            }
            return obj;
        }
        else {
            var obj: any = {};
            obj.isNotValidated = true;
        }
    }

    validate(): any {
        if (this.model.option == "Period") {
            if (!this.model.fromDate || !this.model.toDate
                || !this.model.option || this.model.option == '0') {
                return false;
            }
            else {
                return true;
            }
        }
        else {
            if (!this.model.option || this.model.option == '0') {
                return false;
            }
            else {
                return true;
            }

        }


    }

}
