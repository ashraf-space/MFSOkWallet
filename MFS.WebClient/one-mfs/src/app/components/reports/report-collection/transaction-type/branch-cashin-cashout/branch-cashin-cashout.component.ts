import { Component, OnInit } from '@angular/core';
import { MessageService } from 'primeng/api';
import { MfsUtilityService } from 'src/app/services/mfs-utility.service';
import { AuthenticationService } from 'src/app/shared/_services';

@Component({
    selector: 'app-branch-cashin-cashout',
    templateUrl: './branch-cashin-cashout.component.html',
    styleUrls: ['./branch-cashin-cashout.component.css']
})
export class BranchCashinCashoutComponent implements OnInit {
    model: any;
    cashinCashoutTypeDDL: any;
    optionDDL: any;
    isLoading: boolean = false;
    isDateRangeShow: boolean = false;
    currentUserModel: any = {};
    error: boolean = false;
    constructor(private messageService: MessageService, private mfsUtilityService: MfsUtilityService, private authService: AuthenticationService) {
        this.model = {};
        this.authService.currentUser.subscribe(x => {
            this.currentUserModel = x;
        });
    }

    ngOnInit() {
        this.cashinCashoutTypeDDL = [
            { label: 'Cash In Report', value: 'Cash In Report' },
            { label: 'Cash Out Report', value: 'Cash Out Report' }
        ];
        this.optionDDL = [
            { label: 'Cumulative', value: 'Cumulative' },
            { label: 'Period', value: 'Period' }
        ];
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
            obj.branchCode = this.currentUserModel.user.branchCode;
            obj.cashinCashoutType = this.model.cashinCashoutType;
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
