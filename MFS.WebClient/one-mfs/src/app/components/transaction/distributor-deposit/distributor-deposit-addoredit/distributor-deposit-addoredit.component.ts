import { Component, OnInit } from '@angular/core';
import { DistributorDepositService } from 'src/app/services/transaction';
import { Router, ActivatedRoute } from '@angular/router';
import { MessageService, Message } from 'primeng/api';
import { AuthenticationService } from 'src/app/shared/_services';
import { first } from 'rxjs/operators';

@Component({
    selector: 'app-distributor-deposit-addoredit',
    templateUrl: './distributor-deposit-addoredit.component.html',
    styleUrls: ['./distributor-deposit-addoredit.component.css']
})
export class DistributorDepositAddoreditComponent implements OnInit {
    cashEntryModel: any = {};
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

    constructor(private distributorDepositService: DistributorDepositService, private router: Router,
        private route: ActivatedRoute, private messageService: MessageService, private authService: AuthenticationService) {

        this.authService.currentUser.subscribe(x => {
            this.currentUserModel = x;
        });
    }

    ngOnInit() {
        this.getDistributorAcList();
        this.entityId = this.route.snapshot.paramMap.get('id');
        if (this.entityId) {
            this.isEditMode = true;
            this.getDestributorDepositByTransNo();

            this.isRegistrationPermitted = this.authService.checkRegisterPermissionAccess(this.route.snapshot.routeConfig.path);
        }
    }
    async getDistributorAcList() {
        this.distributorDepositService.getDistributorAcList()
            .pipe(first())
            .subscribe(
                data => {
                    this.distributorAcList = data;
                },
                error => {
                    console.log(error);
                }
            );
    }
    getDestributorDepositByTransNo(): any {
        this.distributorDepositService.getDestributorDepositByTransNo(this.entityId)
            .pipe(first())
            .subscribe(
                data => {
                    this.cashEntryModel = data;
                    this.distCode = data.dist_Code;
                    this.GetCompanyAndHolderName();
                    this.GetAmountInWords();
                    //this.regDate = this.mfsUtilityService.renderDateObject(data.regDate);
                    //this.dateOfBirth = this.mfsUtilityService.renderDateObject(data.dateOfBirth);
                },
                error => {
                    console.log(error);
                }
            )
    }

    //fill GetCompanyAndHolderName against mphone and Code
    GetCompanyAndHolderName() {
        this.distributorDepositService.GetCompanyAndHolderName(this.cashEntryModel.acNo)
            .pipe(first())
            .subscribe(
                data => {
                    if (data != null) {
                        this.cashEntryModel.companyName = data["company_name"];
                        this.cashEntryModel.name = data["name"];
                        this.distCode = data["dist_code"];
                    }
                    else {
                        this.cashEntryModel.companyName = null;
                        this.cashEntryModel.name = null;
                        this.distCode = null;
                    }

                },
                error => {
                    console.log(error);
                }
            );
    }

    GetAmountInWords() {
        this.transAmtLimit = this.currentUserModel.user.tranAmtLimit;

        if (!this.isRegistrationPermitted) {
            if (this.cashEntryModel.amount <= this.transAmtLimit) {
                this.distributorDepositService.GetAmountInWords(this.cashEntryModel.amount)
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
            else {
                this.messageService.add({ severity: 'warn', summary: 'Exceed Limit', detail: 'Limit Amount :' + this.transAmtLimit });
                this.cashEntryModel.amount = null;
                this.amountInWords = null;
            }
        }
        else {
            this.distributorDepositService.GetAmountInWords(this.cashEntryModel.amount)
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

    saveDistributorDepositCashEntry(event): any {

        if (!this.cashEntryModel.acNo || this.cashEntryModel.acNo == '' ||
            !this.distCode || this.cashEntryModel.acNo == '' ||
            !this.cashEntryModel.tracerNo || this.cashEntryModel.tracerNo == '' ||
            !this.cashEntryModel.adviceNo || this.cashEntryModel.adviceNo == '' ||
            !this.cashEntryModel.amount || this.cashEntryModel.amount == '' || this.cashEntryModel.amount == '0') {
            this.msgs = [];
            this.msgs.push({ severity: 'error', summary: 'Warning! ', detail: 'Cannot be left blank' });
            this.error = true;
        }
        else {
            if (!this.isEditMode) {
                this.cashEntryModel.EntryBranchCode = this.currentUserModel.user.branchCode;
                this.cashEntryModel.createUser = this.currentUserModel.user.username;
            }

            if (this.isEditMode && !this.isRegistrationPermitted) {
                this.cashEntryModel.updateUser = this.currentUserModel.user.username;
            }
            if (this.isEditMode && this.isRegistrationPermitted) {
                this.cashEntryModel.checkedUser = this.currentUserModel.user.username;
            }
            if (this.cashEntryModel.acNo != "" || this.distCode != "") {

                
                if (!this.isRegistrationPermitted) {
                    this.distributorDepositService.save(this.cashEntryModel, this.isEditMode, event).pipe(first())
                        .subscribe(
                            data => {
                                if (this.isEditMode) {
                                    if (event == 'edit')
                                        this.messageService.add({ severity: 'success', summary: 'Update successfully', detail: 'Depositor cash entry updated' });
                                    else if (event == 'register')
                                        if (data == "1") {
                                            this.messageService.add({ severity: 'success', summary: 'Approved successfully', detail: 'Depositor cash entry approved' });
                                        }
                                        else {
                                            this.messageService.add({ severity: 'error', summary: 'Not Approved', detail: data });
                                        }
                                    else
                                        this.messageService.add({ severity: 'success', summary: 'Pass to maker successfully', detail: 'Depositor cash entry passed' });
                                }

                                else
                                    this.messageService.add({ severity: 'success', summary: 'Save successfully', detail: 'Depositor cash entry added' });

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
        
        this.distributorDepositService.getDestributorDepositByTransNo(this.entityId)
            .pipe(first())
            .subscribe(
                data => {
                    this.chkStatus = data["status"];
                    if (this.chkStatus == null) {
                        this.distributorDepositService.save(this.cashEntryModel, this.isEditMode, event).pipe(first())
                            .subscribe(
                                data => {
                                    if (this.isEditMode) {
                                        if (event == 'edit')
                                            this.messageService.add({ severity: 'success', summary: 'Update successfully', detail: 'Depositor cash entry updated' });
                                        else if (event == 'register')
                                            if (data == "1") {
                                                this.messageService.add({ severity: 'success', summary: 'Approved successfully', detail: 'Depositor cash entry approved' });
                                            }
                                            else {
                                                this.messageService.add({ severity: 'error', summary: 'Not Approved', detail: data });
                                            }
                                        else
                                            this.messageService.add({ severity: 'success', summary: 'Pass to maker successfully', detail: 'Depositor cash entry passed' });
                                    }

                                    else
                                        this.messageService.add({ severity: 'success', summary: 'Save successfully', detail: 'Depositor cash entry added' });

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



}
