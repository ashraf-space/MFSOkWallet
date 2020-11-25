import { Component, OnInit } from '@angular/core';
import { MessageService } from 'primeng/api';
import { AuthenticationService } from 'src/app/shared/_services';
import { ActivatedRoute } from '@angular/router';
import { first } from 'rxjs/operators';
import { DistributorDepositService } from 'src/app/services/transaction';
import { NidPaymentCollectionService } from 'src/app/services/transaction/nid-payment-collection.service';

@Component({
  selector: 'app-nid-payment-collection',
  templateUrl: './nid-payment-collection.component.html',
  styleUrls: ['./nid-payment-collection.component.css']
})
export class NidPaymentCollectionComponent implements OnInit {
    currentUserModel: any = {};
    nidPaymentCollectionModel: any = {};
    error: boolean = false;
    isEditMode: boolean = false;
    msgs: any[];
    amountInWords: string = "";
    isEnableCashInAmt: boolean = false;
    transAmtLimit: any;
    isRegistrationPermitted: boolean = false;
    isActionDisabled: boolean = true;
    isLoading: boolean = false;

    constructor(private messageService: MessageService, private nidPaymentCollectionService: NidPaymentCollectionService,
        private distributorDepositService: DistributorDepositService, private authService: AuthenticationService
        , private route: ActivatedRoute) {

        this.authService.currentUser.subscribe(x => {
            this.currentUserModel = x;
        });
    }

    ngOnInit() {
        this.nidPaymentCollectionModel.cashInAmount = '';
        this.isRegistrationPermitted = this.authService.checkRegisterPermissionAccess(this.route.snapshot.routeConfig.path);
    }
    async onSearch() {
        return true;
    }

    //onBranchCashInSave() {
    //    this.nidPaymentCollectionModel.checkedUser = this.currentUserModel.user.username;
    //    this.nidPaymentCollectionModel.branchCode = this.currentUserModel.user.branchCode;
    //    if (!this.nidPaymentCollectionModel.mphone || this.nidPaymentCollectionModel.mphone == '' ||
    //        !this.nidPaymentCollectionModel.cashInAmount || this.nidPaymentCollectionModel.cashInAmount == '' || this.nidPaymentCollectionModel.cashInAmount == '0') {
    //        this.msgs = [];
    //        this.msgs.push({ severity: 'error', summary: 'Warning! ', detail: 'Cannot be left blank' });
    //        this.error = true;
    //    }
    //    else {
    //        this.isLoading = true;
    //        this.branchCashInService.saveBranchCashIn(this.nidPaymentCollectionModel, this.isEditMode).pipe(first())
    //            .subscribe(
    //                data => {

    //                    if (this.isRegistrationPermitted) {
    //                        if (data == "1") {
    //                            this.messageService.add({ severity: 'success', summary: 'Approved successfully', detail: 'Branch cash in approved' });
    //                        }
    //                        else {
    //                            this.messageService.add({ severity: 'error', summary: 'Not Approved', detail: data });
    //                        }
    //                    }
    //                    setTimeout(() => {
    //                        this.isLoading = false;
    //                        location.reload();
    //                    }, 5000);
    //                    //window.history.back();
    //                },
    //                error => {
    //                    console.log(error);
    //                });


    //    }
    //}


    //getDetailsByMphone(): any {

    //    //if (this.nidPaymentCollectionModel.mphone != null) {
    //    if (this.nidPaymentCollectionModel.mphone.length == 11) {
    //        this.isLoading = true;
    //        this.branchCashInService.getDetailsByMphone(this.nidPaymentCollectionModel.mphone)
    //            .pipe(first())
    //            .subscribe(
    //                data => {
    //                    this.isLoading = false;
    //                    if (data != null) {
    //                        this.nidPaymentCollectionModel.name = data.NAME;
    //                        this.nidPaymentCollectionModel.category = data.CATEGORY;
    //                        this.nidPaymentCollectionModel.status = data.STATUS;
    //                        this.nidPaymentCollectionModel.regStatus = data.REGSTATUS;

    //                        if (this.nidPaymentCollectionModel.regStatus == "Logical" && this.nidPaymentCollectionModel.category != 'Customer') {
    //                            this.isEnableCashInAmt = true;
    //                            this.nidPaymentCollectionModel.cashInAmount = '';
    //                            this.amountInWords = '';
    //                            this.messageService.add({ severity: 'warn', summary: 'Not able to cash in', detail: 'You are not physically registered' });
    //                        }
    //                        else {
    //                            this.isEnableCashInAmt = false;
    //                        }
    //                    }
    //                    else {
    //                        this.nidPaymentCollectionModel.mphone = null;
    //                        this.nidPaymentCollectionModel.name = null;
    //                        this.nidPaymentCollectionModel.category = null;
    //                        this.nidPaymentCollectionModel.status = null;
    //                        this.nidPaymentCollectionModel.regStatus = null;
    //                    }



    //                },
    //                error => {
    //                    console.log(error);
    //                }
    //            );
    //    }
    //    else {
    //        this.nidPaymentCollectionModel.mphone = this.nidPaymentCollectionModel.mphone;
    //    }

    //    if (this.nidPaymentCollectionModel.mphone.toString().length == 11 && this.nidPaymentCollectionModel.cashInAmount > 0) {
    //        this.isActionDisabled = false;
    //    }
    //    else {
    //        this.isActionDisabled = true;
    //    }


    //}

    GetAmountInWords() {
        this.transAmtLimit = this.currentUserModel.user.tranAmtLimit;
        this.isLoading = true;
        if (this.nidPaymentCollectionModel.amount <= this.transAmtLimit) {
            this.distributorDepositService.GetAmountInWords(this.nidPaymentCollectionModel.amount)
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
            this.nidPaymentCollectionModel.cashInAmount = null;
            this.amountInWords = null;
        }

        if (this.nidPaymentCollectionModel.beneficiaryNumber.toString().length == 11 && this.nidPaymentCollectionModel.amount > 0) {
            this.isActionDisabled = false;
        }
        else {
            this.isActionDisabled = true;
        }

    }

}
