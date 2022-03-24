import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Message, MessageService } from 'primeng/api';
import { first } from 'rxjs/operators';
import { BranchCashInService, DistributorDepositService } from 'src/app/services/transaction';
import { AuthenticationService } from 'src/app/shared/_services';

@Component({
    selector: 'app-commission-convertion-addoredit',
    templateUrl: './commission-convertion-addoredit.component.html',
    styleUrls: ['./commission-convertion-addoredit.component.css']
})
export class CommissionConvertionAddoreditComponent implements OnInit {
    commissionConvertModel: any = {};
    currentUserModel: any = {};
    entityId: string;
    distCode: string;
    isEditMode: boolean = false;
    isRegistrationPermitted: boolean = false;
    msgs: Message[] = [];
    error: boolean = false;
    amountInWords: string = "";
    transAmtLimit: any;
    distributorAcList: any;
    chkStatus: any;
    isDisableSave: boolean = true;

    constructor(private distributorDepositService: DistributorDepositService, private router: Router,
        private route: ActivatedRoute, private messageService: MessageService, private authService: AuthenticationService,
        private branchCashInService: BranchCashInService) {

        this.authService.currentUser.subscribe(x => {
            this.currentUserModel = x;
        });
    }

    ngOnInit() {
        this.entityId = this.route.snapshot.paramMap.get('id');
        if (this.entityId) {
            this.isEditMode = true;
            this.GetCommissionConversionByTransNo();

            this.isRegistrationPermitted = this.authService.checkRegisterPermissionAccess(this.route.snapshot.routeConfig.path);
        }
    }

    GetCommissionConversionByTransNo(): any {
        this.distributorDepositService.GetCommissionConversionByTransNo(this.entityId)
            .pipe(first())
            .subscribe(
                data => {
                    this.commissionConvertModel = data;
                    this.commissionConvertModel.name = data.n_ame;
                    this.commissionConvertModel.category = data.c_ategory;
                    this.isDisableSave = false;
                },
                error => {
                    console.log(error);
                }
            )
    }


    enableDisableSave() {

        if (this.commissionConvertModel.amount <= this.commissionConvertModel.commiBalance) {
            this.isDisableSave = false;
        }
        else {
            this.isDisableSave = true;
            this.messageService.add({ severity: 'error', summary: 'Exit commision balance', detail: "amount can't exit your commission balance!" });
        }

    }

    saveCommissionConversion(event): any {

        if (!this.commissionConvertModel.mphone || this.commissionConvertModel.mphone == '' ||
            !this.commissionConvertModel.amount || this.commissionConvertModel.amount == '' || this.commissionConvertModel.amount == '0') {
            this.msgs = [];
            this.msgs.push({ severity: 'error', summary: 'Warning! ', detail: 'Cannot be left blank' });
            this.error = true;
        }
        else {
            if (!this.isEditMode) {
                this.commissionConvertModel.EntryBranchCode = this.currentUserModel.user.branchCode;
                this.commissionConvertModel.createUser = this.currentUserModel.user.username;
            }

            if (this.isEditMode && !this.isRegistrationPermitted) {
                this.commissionConvertModel.updateUser = this.currentUserModel.user.username;
            }
            if (this.isEditMode && this.isRegistrationPermitted) {
                this.commissionConvertModel.checkedUser = this.currentUserModel.user.username;
            }
            if (this.commissionConvertModel.acNo != "" || this.distCode != "") {


                if (!this.isRegistrationPermitted) {
                    this.distributorDepositService.saveCommissionConversion(this.commissionConvertModel, this.isEditMode, event).pipe(first())
                        .subscribe(
                            data => {
                                if (this.isEditMode) {
                                    if (event == 'edit')
                                        this.messageService.add({ severity: 'success', summary: 'Update successfully', detail: 'Commission Conversion entry updated' });
                                    else if (event == 'register')
                                        if (data == "1") {
                                            this.messageService.add({ severity: 'success', summary: 'Approved successfully', detail: 'Commission Conversion entry approved' });
                                        }
                                        else {
                                            this.messageService.add({ severity: 'error', summary: 'Not Approved', detail: data });
                                        }
                                    else
                                        this.messageService.add({ severity: 'success', summary: 'Pass to maker successfully', detail: 'Commission Conversion entry passed' });
                                }

                                else
                                    this.messageService.add({ severity: 'success', summary: 'Save successfully', detail: 'Commission Conversion entry added' });

                                window.history.back();
                            },
                            error => {
                                console.log(error);
                            });
                }

                else {
                    this.checkIsAlreadyActinDone(event);
                }


            }
        }



    }
    checkIsAlreadyActinDone(event): any {

        this.distributorDepositService.GetCommissionConversionByTransNo(this.entityId)
            .pipe(first())
            .subscribe(
                data => {
                    this.chkStatus = data["status"];
                    if (this.chkStatus == null) {
                        this.distributorDepositService.saveCommissionConversion(this.commissionConvertModel, this.isEditMode, event).pipe(first())
                            .subscribe(
                                data => {
                                    if (this.isEditMode) {
                                        if (event == 'edit')
                                            this.messageService.add({ severity: 'success', summary: 'Update successfully', detail: 'Commission Conversion entry updated' });
                                        else if (event == 'register') {
                                            if (data == "1") {
                                                this.messageService.add({ severity: 'success', summary: 'Approved successfully', detail: 'Commission Conversion entry approved' });
                                            }
                                            else {
                                                this.messageService.add({ severity: 'error', summary: 'Not Approved', detail: data });
                                            }
                                        }
                                        else {
                                            if (data == "Failed") {
                                                this.messageService.add({ severity: 'error', summary: 'Failed', detail: 'Action is already performed, reload page' });
                                            }
                                            else {
                                                this.messageService.add({ severity: 'success', summary: 'Reject successfully', detail: 'Commission Conversion rejected' });
                                            }
                                        }
                                    }

                                    else
                                        this.messageService.add({ severity: 'success', summary: 'Save successfully', detail: 'Commission Conversion entry added' });

                                    window.history.back();
                                },
                                error => {
                                    console.log(error);
                                });
                    }
                    else {
                        this.messageService.add({ severity: 'error', summary: 'Action already done', detail: 'Action already done agianst this A/C number' });
                    }

                },
                error => {
                    console.log(error);
                }
            )
    }

    cancel() {
        window.history.back();
    }

    getDetailsByMphone(): any {

        //if (this.commissionConvertModel.mphone != null) {
        if (this.commissionConvertModel.mphone.length == 11) {
            //this.isLoading = true;
            this.branchCashInService.getDetailsByMphoneForCommiConvert(this.commissionConvertModel.mphone)
                .pipe(first())
                .subscribe(
                    data => {
                        //this.isLoading = false;
                        if (data != null) {
                            this.commissionConvertModel.name = data.NAME;
                            this.commissionConvertModel.commiBalance = data.COMMIBALANCE;
                            this.commissionConvertModel.category = data.CATEGORY;
                            this.commissionConvertModel.status = data.STATUS;
                            this.commissionConvertModel.regStatus = data.REGSTATUS;

                            if (this.commissionConvertModel.commiBalance < 0.01) {
                                //this.isEnableCashInAmt = true;
                                this.commissionConvertModel.amount = '';

                                this.messageService.add({ severity: 'warn', summary: 'have no Commission Balance', detail: 'You have no Commission Balance to convert' });
                            }
                            else {
                                //this.isEnableCashInAmt = false;
                            }
                        }
                        else {
                            this.commissionConvertModel.mphone = null;
                            this.commissionConvertModel.name = null;
                            this.commissionConvertModel.category = null;
                            this.commissionConvertModel.status = null;
                            this.commissionConvertModel.regStatus = null;
                        }



                    },
                    error => {
                        console.log(error);
                    }
                );
        }
        else {
            this.commissionConvertModel.mphone = this.commissionConvertModel.mphone;
        }

        //if (this.commissionConvertModel.mphone.toString().length == 11 && this.commissionConvertModel.commiBalance > 0) {
        //    this.isActionDisabled = false;
        //}
        //else {
        //    this.isActionDisabled = true;
        //}


    }

}
