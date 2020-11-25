import { Component, OnInit } from '@angular/core';
import { MessageService } from 'primeng/api';
import { BranchCashInService, DistributorDepositService } from 'src/app/services/transaction';
import { AuthenticationService } from 'src/app/shared/_services';
import { first } from 'rxjs/operators';
import { ActivatedRoute } from '@angular/router';

@Component({
    selector: 'app-branch-cash-out',
    templateUrl: './branch-cash-out.component.html',
    styleUrls: ['./branch-cash-out.component.css']
})
export class BranchCashOutComponent implements OnInit {
    currentUserModel: any = {};
    tblPortalCashoutModel: any = {};
    error: boolean = false;
    isEditMode: boolean = false;
    msgs: any[];
    amountInWords: string = "";
    transAmtLimit: any;
    isActionDisabled: boolean = true;
    isRegistrationPermitted: boolean = false;
    isLoading: boolean = false;
    constructor(private messageService: MessageService, private branchCashInService: BranchCashInService,
        private distributorDepositService: DistributorDepositService, private authService: AuthenticationService
        , private route: ActivatedRoute) {

        this.authService.currentUser.subscribe(x => {
            this.currentUserModel = x;
        });
    }

    ngOnInit() {
        this.tblPortalCashoutModel.cashInAmount = '0';
        this.isRegistrationPermitted = this.authService.checkRegisterPermissionAccess(this.route.snapshot.routeConfig.path);
    }

    async onSearch() {
        return true;
    }

    AproveOrRejectBranchCashout(event) {

        this.tblPortalCashoutModel.branchCode = this.currentUserModel.user.branchCode;
        this.tblPortalCashoutModel.checkBy = this.currentUserModel.user.username;
        if (!this.tblPortalCashoutModel.mphone || this.tblPortalCashoutModel.mphone == '' ||
            !this.tblPortalCashoutModel.amount || this.tblPortalCashoutModel.amount == '' || this.tblPortalCashoutModel.amount == '0' ||
            !this.tblPortalCashoutModel.transNo || this.tblPortalCashoutModel.transNo == '') {
            this.msgs = [];
            this.msgs.push({ severity: 'error', summary: 'Warning! ', detail: 'Cannot be left blank' });
            this.error = true;
        }
        else {

            this.branchCashInService.CheckData(this.tblPortalCashoutModel.transNo, this.tblPortalCashoutModel.mphone, this.tblPortalCashoutModel.amount).pipe(first())
                .subscribe(
                    data => {
                        if (data == "1") {
                            this.onConfirmSave(event);
                        }
                        else {
                            this.messageService.add({ severity: 'error', summary: 'Failed', detail: 'Something wrong happened' });
                        }

                    },
                    error => {
                        console.log(error);
                    }
                );

        }
    }
    onConfirmSave(event): any {
        this.branchCashInService.AproveOrRejectBranchCashout(this.tblPortalCashoutModel, event).pipe(first())
            .subscribe(
                data => {

                    if (event == "register")
                        //this.messageService.add({ severity: 'success', summary: 'Cashout successfully', detail: 'Branch cash out successfully' });
                        if (data == "1") {
                            this.messageService.add({ severity: 'success', summary: 'Cashout successfully', detail: 'Branch cash out approved' });
                        }
                        else if (data == "Failed") {
                            this.messageService.add({ severity: 'error', summary: 'Failed', detail: 'Action is already performed, reload page' });
                        }
                        else {
                            this.messageService.add({ severity: 'error', summary: 'Not Approved', detail: data });
                        }
                    else
                        //this.messageService.add({ severity: 'success', summary: 'Reject successfully', detail: 'Branch cash out rejected' });
                        if (data == "1") {
                            this.messageService.add({ severity: 'success', summary: 'Reject successfully', detail: 'Branch cash out rejected' });
                        }
                        else if (data == "Failed") {
                            this.messageService.add({ severity: 'error', summary: 'Failed', detail: 'Action is already performed, reload page' });
                        }
                        else {
                            this.messageService.add({ severity: 'error', summary: 'Not rejected', detail: data });
                        }

                    //window.history.back();
                    setTimeout(() => {
                        this.isLoading = false;
                        location.reload();
                    }, 20000);
                },
                error => {
                    console.log(error);
                });
    }


    getReginfoCashoutByMphone(): any {

        if (this.tblPortalCashoutModel.mphone.length == 11) {
            this.isLoading = true;
            this.branchCashInService.getReginfoCashoutByMphone(this.tblPortalCashoutModel.mphone)
                .pipe(first())
                .subscribe(
                    data => {
                        this.isLoading = false;
                        if (data != null) {
                            this.tblPortalCashoutModel.name = data.NAME;
                            this.tblPortalCashoutModel.category = data.CATEGORY;
                            this.tblPortalCashoutModel.status = data.STATUS;
                            this.tblPortalCashoutModel.regStatus = data.REGSTATUS;
                            //this.tblPortalCashoutModel.amount = data.AMOUNT;
                            this.tblPortalCashoutModel.balanceType = data.BALANCE_TYPE;
                            this.tblPortalCashoutModel.gateway = data.GATEWAY;
                            //this.GetAmountInWords();

                            if (this.tblPortalCashoutModel.mphone.toString().length == 11 && this.tblPortalCashoutModel.amount > 0) {
                                this.isActionDisabled = false;
                            }
                            else {
                                this.isActionDisabled = true;
                            }
                        }
                        else {
                            this.tblPortalCashoutModel.mphone = null;
                            this.tblPortalCashoutModel.name = null;
                            this.tblPortalCashoutModel.category = null;
                            this.tblPortalCashoutModel.status = null;
                            this.tblPortalCashoutModel.regStatus = null;
                            //this.tblPortalCashoutModel.amount = 0;
                            this.tblPortalCashoutModel.balanceType = null;
                            this.tblPortalCashoutModel.gateway = null;
                            //this.GetAmountInWords();
                            this.isActionDisabled = true;

                            this.messageService.add({ severity: 'warn', sticky: true, summary: 'Can not proceed', detail: 'This A/C does not have any Branch Cashout Transaction or an invalid  OK Wallet A/C !' });

                            //setTimeout(() => {
                            //    this.isLoading = false;
                            //    location.reload();
                            //}, 30000);
                        }



                    },
                    error => {
                        console.log(error);
                        this.isLoading = false;
                    }
                );
        }
        else {
            this.tblPortalCashoutModel.mphone = this.tblPortalCashoutModel.mphone;
        }


    }

    getAmountByTransNo(): any {
        this.transAmtLimit = this.currentUserModel.user.tranAmtLimit;
        this.isLoading = true;
        if (this.tblPortalCashoutModel.transNo != '' && this.tblPortalCashoutModel.mphone != '') {
            this.branchCashInService.getAmountByTransNo(this.tblPortalCashoutModel.mphone, this.tblPortalCashoutModel.transNo)
                .pipe(first())
                .subscribe(
                    data => {
                        this.isLoading = false;
                        if (data != null) {
                            if (data.AMOUNT <= this.transAmtLimit) {
                                this.tblPortalCashoutModel.amount = data.AMOUNT;
                                this.GetAmountInWords();

                                if (this.tblPortalCashoutModel.mphone.toString().length == 11 && this.tblPortalCashoutModel.amount > 0) {
                                    this.isActionDisabled = false;
                                }
                                else {
                                    this.isActionDisabled = true;
                                }
                            }
                            else {
                                this.tblPortalCashoutModel.transNo = '';
                                this.tblPortalCashoutModel.amount = 0;
                                this.GetAmountInWords();
                                this.isActionDisabled = true;
                                this.messageService.add({ severity: 'warn', sticky: true, summary: 'Exceed Limit', detail: 'Limit Amount :' + this.transAmtLimit });


                            }
                        }
                        else {
                            this.tblPortalCashoutModel.transNo = '';
                            this.tblPortalCashoutModel.amount = 0;
                            this.GetAmountInWords();
                            this.isActionDisabled = true;
                            this.messageService.add({ severity: 'warn', sticky: true, summary: 'Provide a valid OK Wallet Number and transaction ID', detail: 'Please collect a valid transaction ID from account holder and provide in trasaction ID field!' });

                            //setTimeout(() => {
                            //    this.isLoading = false;
                            //    //location.reload();
                            //}, 30000);
                        }



                    },
                    error => {
                        console.log(error);
                    }
                );
        }
        else {
            this.tblPortalCashoutModel.mphone = '';
            this.tblPortalCashoutModel.transNo = '';


            this.messageService.add({ severity: 'warn', sticky: true, summary: 'Invalid', detail: 'Account No or Trans No is invalid!' });
        }



    }

    GetAmountInWords() {
        this.distributorDepositService.GetAmountInWords(this.tblPortalCashoutModel.amount)
            .pipe(first())
            .subscribe(
                data => {
                    this.amountInWords = data;

                },
                error => {
                    console.log(error);
                }
            );


    }
}
