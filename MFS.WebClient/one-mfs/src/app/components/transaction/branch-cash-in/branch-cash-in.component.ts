import { Component, OnInit } from '@angular/core';
import { MessageService } from 'primeng/api';
import { BranchCashInService, DistributorDepositService } from 'src/app/services/transaction';
import { first } from 'rxjs/operators';
import { async } from 'rxjs/internal/scheduler/async';
import { AuthenticationService } from 'src/app/shared/_services';
import { ActivatedRoute } from '@angular/router';

@Component({
    selector: 'app-branch-cash-in',
    templateUrl: './branch-cash-in.component.html',
    styleUrls: ['./branch-cash-in.component.css']
})
export class BranchCashInComponent implements OnInit {
    currentUserModel: any = {};
    branchCashInModel: any = {};
    error: boolean = false;
    isEditMode: boolean = false;
    msgs: any[];
    amountInWords: string = "";
    isEnableCashInAmt: boolean = false;
    transAmtLimit: any;
    isRegistrationPermitted: boolean = false;
    isActionDisabled: boolean = true;
    isLoading: boolean = false;

    constructor(private messageService: MessageService, private branchCashInService: BranchCashInService,
        private distributorDepositService: DistributorDepositService, private authService: AuthenticationService
        , private route: ActivatedRoute) {

        this.authService.currentUser.subscribe(x => {
            this.currentUserModel = x;
        });
    }

    ngOnInit() {
        this.branchCashInModel.cashInAmount = '';
        this.isRegistrationPermitted = this.authService.checkRegisterPermissionAccess(this.route.snapshot.routeConfig.path);
    }
    async onSearch() {
        return true;
    }

    onBranchCashInSave() {
        this.branchCashInModel.checkedUser = this.currentUserModel.user.username;
        this.branchCashInModel.branchCode = this.currentUserModel.user.branchCode;
        if (!this.branchCashInModel.mphone || this.branchCashInModel.mphone == '' ||
            !this.branchCashInModel.cashInAmount || this.branchCashInModel.cashInAmount == '' || this.branchCashInModel.cashInAmount == '0') {
            this.msgs = [];
            this.msgs.push({ severity: 'error', summary: 'Warning! ', detail: 'Cannot be left blank' });
            this.error = true;
        }
        else {

            this.branchCashInService.saveBranchCashIn(this.branchCashInModel, this.isEditMode).pipe(first())
                .subscribe(
                    data => {

                        if (this.isRegistrationPermitted) {
                            if (data == "1") {
                                this.messageService.add({ severity: 'success', summary: 'Approved successfully', detail: 'Branch cash in approved' });
                            }
                            else {
                                this.messageService.add({ severity: 'error', summary: 'Not Approved', detail: data });
                            }
                        }
                        window.history.back();
                    },
                    error => {
                        console.log(error);
                    });


        }
    }


    getDetailsByMphone(): any {      
        //console.log(this.branchCashInModel.mphone.length);
        if (this.branchCashInModel.mphone != null) {
            this.isLoading = true;
            this.branchCashInService.getDetailsByMphone(this.branchCashInModel.mphone)
                .pipe(first())
                .subscribe(
                    data => {
                        this.isLoading = false;
                        if (data != null) {
                            this.branchCashInModel.name = data.NAME;
                            this.branchCashInModel.category = data.CATEGORY;
                            this.branchCashInModel.status = data.STATUS;
                            this.branchCashInModel.regStatus = data.REGSTATUS;

                            if (this.branchCashInModel.regStatus == "Logical" && this.branchCashInModel.category!='Customer') {
                                this.isEnableCashInAmt = true;
                                this.branchCashInModel.cashInAmount = '';
                                this.amountInWords = '';
                                this.messageService.add({ severity: 'warn', summary: 'Not able to cash in', detail: 'You are not physically registered' });
                            }
                            else {
                                this.isEnableCashInAmt = false;
                            }
                        }
                        else {
                            this.branchCashInModel.mphone = null;
                            this.branchCashInModel.name = null;
                            this.branchCashInModel.category = null;
                            this.branchCashInModel.status = null;
                            this.branchCashInModel.regStatus = null;
                        }



                    },
                    error => {
                        console.log(error);
                    }
                );
        }

        if (this.branchCashInModel.mphone.toString().length == 11 && this.branchCashInModel.cashInAmount > 0) {
            this.isActionDisabled = false;
        }
        else {
            this.isActionDisabled = true;
        }

      
    }

    GetAmountInWords() {       
        this.transAmtLimit = this.currentUserModel.user.tranAmtLimit;
        this.isLoading = true;
        if (this.branchCashInModel.cashInAmount <= this.transAmtLimit) {
            this.distributorDepositService.GetAmountInWords(this.branchCashInModel.cashInAmount)
                .pipe(first())
                .subscribe(
                    data => {
                        this.isLoading = false;
                        this.amountInWords = data;

                    },
                    error => {
                        console.log(error);
                    }
                );
        }
        else {
            this.messageService.add({ severity: 'warn', summary: 'Exceed Limit', detail: 'Limit Amount :' + this.transAmtLimit });
            this.branchCashInModel.cashInAmount = null;
            this.amountInWords = null;
        }

        if (this.branchCashInModel.mphone.toString().length == 11 && this.branchCashInModel.cashInAmount > 0) {
            this.isActionDisabled = false;
        }
        else {
            this.isActionDisabled = true;
        }
    
    }
}
