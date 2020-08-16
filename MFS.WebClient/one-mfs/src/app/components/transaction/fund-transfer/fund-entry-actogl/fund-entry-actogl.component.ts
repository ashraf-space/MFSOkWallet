import { Component, OnInit } from '@angular/core';
import { FundTransferService } from 'src/app/services/transaction';
import { MfsSettingService } from 'src/app/services/mfs-setting.service';
import { GridSettingService } from 'src/app/services/grid-setting.service';
import { first } from 'rxjs/operators';
import { MessageService, Message } from 'primeng/api';
import { AuthenticationService } from 'src/app/shared/_services';


@Component({
    selector: 'app-fund-entry-actogl',
    templateUrl: './fund-entry-actogl.component.html',
    styleUrls: ['./fund-entry-actogl.component.css']
})
export class FundEntryActoglComponent implements OnInit {
    fundEntryTypeList: any;
    fundTransferModel: any = {};
    currentUserModel: any = {};
    gridConfig: any;
    acList: any;
    glList: any;
    SelectedGL: string = "";
    fromAmount: any;
    formOrTotype: string = "";
    toAmount: any;
    fromAC: string = "";
    toGl: string = "";
    selectedEntryType: string = "";
    isEditMode: boolean = false;
    fromCategory: string = "";
    toCategory: string = "";
    msgs: Message[] = [];
    error: boolean = false;

    cols: any[];
    vMTransactionDetails: any = {};
    vMTransactionDetailList: any;
    transAmtLimit: any;
    fromHolderName: string = null;
    isLoading: boolean = false;
    isSaveDisable = true;

    constructor(private fundTransferService: FundTransferService, private mfsSettingService: MfsSettingService, private gridSettingService: GridSettingService
        , private messageService: MessageService, private authService: AuthenticationService) {
        this.gridConfig = {};
        this.authService.currentUser.subscribe(x => {
            this.currentUserModel = x;
        });
    }

    ngOnInit() {
        this.initialiseGridConfig();
        this.getACList();
        this.getGlList();
    }
    async getACList() {
        this.fundTransferService.getACList()
            .pipe(first())
            .subscribe(
                data => {
                    this.acList = data;
                },
                error => {
                    console.log(error);
                }
            );
    }

    async getGlList() {
        this.fundTransferService.getGlList()
            .pipe(first())
            .subscribe(
                data => {
                    this.glList = data;
                },
                error => {
                    console.log(error);
                }
            );
    }
    //fill amount against mphone
    fillAmountByMphone(SelectedAC, formOrTotype) {
        if (this.fundTransferModel.transFrom) {
            this.fromAC = this.acList.find(it => {
                //return it.value.toLowerCase().includes(this.fundTransferModel.transFrom.toLowerCase());
                return it.value == this.fundTransferModel.transFrom;
            }).label;
        }

        this.fromHolderName = this.fromAC;
        this.isLoading = true;
        this.fundTransferService.getAmountByAC(SelectedAC)
            .pipe(first())
            .subscribe(
                data => {
                    this.isLoading = false;
                    if (formOrTotype == 'From') {
                        if (data == null) {
                            this.fromAmount = 0;
                            this.fromCategory = null;
                            this.fundTransferModel.fromCatId = null;
                        }
                        else {
                            this.fromAmount = data["AMOUNT"];
                            this.fromCategory = data["CATEGORY"];
                            this.fundTransferModel.fromCatId = data["CATID"];
                        }
                    }

                    else {
                        if (data == null) {
                            this.toAmount = 0;
                            this.toCategory = null;
                            this.fundTransferModel.toCatId = null;
                        }
                        else {
                            this.toAmount = data["AMOUNT"];
                            this.toCategory = data["CATEGORY"];
                            this.fundTransferModel.toCatId = data["CATID"];
                        }
                    }

                },
                error => {
                    console.log(error);
                }
            );

        this.fundTransferModel.payAmt = 0;
        this.initialiseGridConfig();
    }

    //fill amount against Sys code
    fillAmount(SelectedGL, formOrTotype) {
        this.isLoading = true;
        this.fundTransferService.getAmountByGL(SelectedGL)
            .pipe(first())
            .subscribe(
                data => {
                    this.isLoading = false;
                    if (formOrTotype == 'From')
                        this.fromAmount = data;

                    else {
                        if (data == null) {
                            this.toAmount = 0;
                            this.toCategory = null;
                            this.fundTransferModel.toCatId = null;
                        }
                        else {
                            this.toAmount = data;
                            this.toCategory = 'System';
                            this.fundTransferModel.toCatId = 'S';
                        }
                    }

                },
                error => {
                    console.log(error);
                }
            );

        this.fundTransferModel.payAmt = 0;
        this.initialiseGridConfig();
    }

    async GetTransactionDetailsByPayAmount() {
        if (this.fundTransferModel.payAmt == '') {
            this.fundTransferModel.payAmt = 0;
        }

        this.transAmtLimit = this.currentUserModel.user.tranAmtLimit;
        if (+this.fundTransferModel.payAmt <= this.transAmtLimit) {
            if (+this.fromAmount >= +this.fundTransferModel.payAmt) {
                this.fromAC = this.acList.find(it => {
                    return it.value.toLowerCase().includes(this.fundTransferModel.transFrom.toLowerCase());
                }).label;

                this.toGl = this.glList.find(it => {
                    return it.value.toLowerCase().includes(this.fundTransferModel.toSysCoaCode.toLowerCase());
                }).label;
            }
            else {
                this.messageService.add({ severity: 'warn', summary: 'Exceed from amount', detail: 'Cannot be greater than from amount :' + this.fromAmount });
                this.fundTransferModel.payAmt = 0;
            }
        }
        else {
            this.messageService.add({ severity: 'warn', summary: 'Exceed Limit', detail: 'Limit Amount :' + this.transAmtLimit });
            this.fundTransferModel.payAmt = 0;
        }


        this.initialiseGridConfig();

    }

    initialiseGridConfig(): any {
        this.isLoading = true;
        this.fundTransferModel.hotkey = "AC TO GL";
        this.fundTransferService.GetTransactionDetailsByPayAmount(this.fundTransferModel, this.fromAC, this.toGl)
            .pipe(first())
            .subscribe(
                data => {
                    this.isLoading = false;
                    this.vMTransactionDetailList = data;
                    //this.fundTransferModel.fromSysCoaCode = data[0].glCode;
                    this.fundTransferModel.fromSysCoaCode = data[0].glSysCoaCode;
                    this.fromAC = data[0].coaDesc;

                    if (data[0].acNo != '') {
                        this.isSaveDisable = false;
                    }
                    else {
                        this.isSaveDisable = true;
                    }
                  

                },
                error => {
                    console.log(error);
                }
            );

        this.cols = [
            { field: 'glCode', header: 'GL No', width: '30%' },
            { field: 'glName', header: 'GL Name', width: '30%', filter: this.gridSettingService.getFilterableNone() },
            { field: 'debitAmount', header: 'Debit Amount', width: '20%', filter: this.gridSettingService.getFilterableNone() },
            { field: 'creditAmount', header: 'Credit Amount', width: '20%', filter: this.gridSettingService.getFilterableNone() }
        ];

    };

    saveFundTransferEntry(event): any {
        if (!this.fundTransferModel.transFrom || this.fundTransferModel.transFrom == '' ||
            !this.fundTransferModel.toSysCoaCode || this.fundTransferModel.toSysCoaCode == '' ||
            !this.fundTransferModel.payAmt || this.fundTransferModel.payAmt == '' || this.fundTransferModel.payAmt == '0') {
            this.msgs = [];
            this.msgs.push({ severity: 'error', summary: 'Warning! ', detail: 'Cannot be left blank' });
            this.error = true;
        }
        else {

            if (!this.isEditMode) {
                this.fundTransferModel.entryBrCode = this.currentUserModel.user.branchCode;
                this.fundTransferModel.entryUser = this.currentUserModel.user.username;
            }

            if (this.fromAC != "" || this.toGl != "") {
                this.fundTransferModel.hotkey = "AC TO GL";//for data base save
                this.isLoading = true;
                this.fundTransferService.saveFundTransferEntry(this.fundTransferModel, this.fromAC, this.toGl, this.isEditMode, event).pipe(first())
                    .subscribe(
                        data => {
                            this.isLoading = false;
                            if (this.isEditMode)
                                this.messageService.add({ severity: 'success', summary: 'Update successfully', detail: 'Fund entry AC to GL updated' });
                            else
                                this.messageService.add({ severity: 'success', summary: 'Save successfully', detail: 'Fund entry AC to GL added' });
                            console.log(data);
                            setTimeout(() => {
                                this.isLoading = false;
                                location.reload();
                            }, 5000);
                            //window.history.back();
                        },
                        error => {
                            console.log(error);
                        });

            }
        }
    }
}

