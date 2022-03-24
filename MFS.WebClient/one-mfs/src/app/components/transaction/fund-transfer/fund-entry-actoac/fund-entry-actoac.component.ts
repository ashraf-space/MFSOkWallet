import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { FundTransferService } from 'src/app/services/transaction';
import { MfsSettingService } from 'src/app/services/mfs-setting.service';
import { GridSettingService } from 'src/app/services/grid-setting.service';
import { first } from 'rxjs/operators';
import { MessageService, Message } from 'primeng/api';
import { AuthenticationService } from 'src/app/shared/_services';
import { StaticInjector } from '@angular/core/src/di/injector';


@Component({
    selector: 'app-fund-entry-actoac',
    templateUrl: './fund-entry-actoac.component.html',
    styleUrls: ['./fund-entry-actoac.component.css'],
    encapsulation: ViewEncapsulation.None
})
export class FundEntryActoacComponent implements OnInit {
    fundEntryTypeList: any;
    fundTransferModel: any = {};
    currentUserModel: any = {};
    gridConfig: any;
    acList: any;
    glTransMst: any = {};
    SelectedAC: string = "";
    fromAmount: any;
    formOrTotype: string = "";
    toAmount: any;
    fromAC: string = "";
    toAC: string = "";
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
    toHolderName: any;
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
        
    }


    async getACList() {
        this.fundTransferService.getACList()
            .pipe(first())
            .subscribe(
                dataList => {
                    this.acList = dataList;
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

        if (this.fundTransferModel.transTo) {
            this.toAC = this.acList.find(it => {
                //return it.value.toLowerCase().includes(this.fundTransferModel.transTo.toLowerCase());
                return it.value == this.fundTransferModel.transTo;
            }).label;
        }        

        if (this.fromAC != this.toAC) {
            this.fromHolderName = this.fromAC;
            this.toHolderName = this.toAC;
            this.isLoading = true;
            this.fundTransferService.getAmountByAC(SelectedAC)
                .pipe(first())
                .subscribe(
                    data => {
                        this.isLoading = false;
                        if (formOrTotype == 'From') {
                            if (data == null) {
                                this.fromHolderName = null;
                                this.fromAmount = 0;
                                this.fromCategory = null;
                                this.fundTransferModel.fromCatId = null;
                            }
                            else {
                                //this.fromHolderName = this.fromAC;
                                this.fromAmount = data["AMOUNT"];
                                this.fromCategory = data["CATEGORY"];
                                this.fundTransferModel.fromCatId = data["CATID"];
                            }

                        }

                        else {
                            if (data == null) {
                                this.toHolderName = null;
                                this.toAmount = 0;
                                this.toCategory = null;
                                this.fundTransferModel.toCatId = null;
                            }
                            else {
                                //this.toHolderName = this.toAC;
                                this.toAmount = data["AMOUNT"];
                                this.toCategory = data["CATEGORY"];
                                this.fundTransferModel.toCatId = data["CATID"];
                            }

                        }

                        this.fundTransferModel.payAmt = 0;
                        this.initialiseGridConfig();

                    },
                    error => {
                        console.log(error);
                    }
                );

            //this.fundTransferModel.payAmt = 0;
            //this.initialiseGridConfig();
        }
        else {
            if (formOrTotype == 'From') {
                this.messageService.add({ severity: 'warn', summary: 'Can not be same', detail: 'Can not be same as to A/C.' });
                this.fundTransferModel.transFrom = null;
                this.fromAmount = 0;
                this.fromCategory = null;
                this.fundTransferModel.fromCatId = null;
            }
            else {
                this.messageService.add({ severity: 'warn', summary: 'Can not be same', detail: 'Can not be same as from A/C.' });
                this.fundTransferModel.transTo = null;
                this.toAmount = 0;
                this.toCategory = null;
                this.fundTransferModel.toCatId = null;
            }
        }

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

                this.toAC = this.acList.find(it => {
                    return it.value.toLowerCase().includes(this.fundTransferModel.transTo.toLowerCase());
                }).label;
            }
            else {
                this.messageService.add({ severity: 'warn', summary: 'Exceed from amount', detail: 'Cannot be greater than from amount :' + this.fromAmount });
                this.fundTransferModel.payAmt = 0;
            }


        }
        else {
            this.messageService.add({ severity: 'warn', summary: 'Exceed Limit', detail: 'Your Limit Amount :' + this.transAmtLimit });
            this.fundTransferModel.payAmt = 0;
        }



        this.initialiseGridConfig();

    }

    initialiseGridConfig(): any {
        this.fundTransferModel.hotkey = "AC TO AC";
        this.isLoading = true;
        this.fundTransferService.GetTransactionDetailsByPayAmount(this.fundTransferModel, this.fromAC, this.toAC)
            .pipe(first())
            .subscribe(
                data => {
                    this.isLoading = false;
                    this.vMTransactionDetailList = data;
                    //this.fundTransferModel.fromSysCoaCode = data[0].glCode;
                    //this.fundTransferModel.toSysCoaCode = data[1].glCode;
                    this.fundTransferModel.fromSysCoaCode = data[0].glSysCoaCode;
                    this.fundTransferModel.toSysCoaCode = data[1].glSysCoaCode;
                    this.fromAC = data[0].coaDesc;
                    this.toAC = data[1].coaDesc;

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
            { field: 'acNo', header: 'A/C No', width: '30%', template: 'none'  },
            { field: 'glCode', header: 'GL Code', width: '20%', template: 'none'  },
            { field: 'glName', header: 'GL Name', width: '20%', filter: this.gridSettingService.getFilterableNone(), template: 'none'  },
            { field: 'debitAmount', header: 'Debit Amount', width: '15%', filter: this.gridSettingService.getFilterableNone(), template: this.gridSettingService.getMoneyTemplateForRowData() },
            { field: 'creditAmount', header: 'Credit Amount', width: '15%', filter: this.gridSettingService.getFilterableNone(), template: this.gridSettingService.getMoneyTemplateForRowData() }
        ];

    };

    saveFundTransferEntry(event): any {
        if (!this.fundTransferModel.transFrom || this.fundTransferModel.transFrom == '' ||
            !this.fundTransferModel.transTo || this.fundTransferModel.transTo == '' ||
            !this.fundTransferModel.payAmt || this.fundTransferModel.payAmt == '' || this.fundTransferModel.payAmt == '0' ||
            !this.fundTransferModel.remarks || this.fundTransferModel.remarks == '0' || this.fundTransferModel.remarks == '') {
            this.msgs = [];
            this.msgs.push({ severity: 'error', summary: 'Warning! ', detail: 'Cannot be left blank' });
            this.error = true;
        }
        else {
            if (!this.isEditMode) {
                this.fundTransferModel.entryBrCode = this.currentUserModel.user.branchCode;
                this.fundTransferModel.entryUser = this.currentUserModel.user.username;
            }
            if (this.fromAC != "" || this.toAC != "") {
                this.fundTransferModel.hotkey = "AC TO AC";//for data base save
                this.isLoading = true;
                this.fundTransferService.saveFundTransferEntry(this.fundTransferModel, this.fromAC, this.toAC, this.isEditMode, event).pipe(first())
                    .subscribe(
                        data => {
                          
                            if (this.isEditMode)
                                this.messageService.add({ severity: 'success', summary: 'Update successfully', detail: 'Fund entry AC to AC updated' });
                            else
                                this.messageService.add({ severity: 'success', summary: 'Save successfully', detail: 'Fund entry AC to AC added' });
                            //console.log(data);
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
