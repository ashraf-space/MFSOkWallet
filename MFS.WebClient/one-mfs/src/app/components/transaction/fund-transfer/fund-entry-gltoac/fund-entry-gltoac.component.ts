import { Component, OnInit } from '@angular/core';
import { FundTransferService } from 'src/app/services/transaction';
import { MfsSettingService } from 'src/app/services/mfs-setting.service';
import { GridSettingService } from 'src/app/services/grid-setting.service';
import { first } from 'rxjs/operators';
import { MessageService, Message } from 'primeng/api';
import { AuthenticationService } from 'src/app/shared/_services';

@Component({
    selector: 'app-fund-entry-gltoac',
    templateUrl: './fund-entry-gltoac.component.html',
    styleUrls: ['./fund-entry-gltoac.component.css']
})
export class FundEntryGltoacComponent implements OnInit {
    fundEntryTypeList: any;
    currentUserModel: any = {};
    gridConfig: any;
    acList: any;
    glList: any;
    fundTransferModel: any = {};
    SelectedGL: string = "";
    fromAmount: any;
    formOrTotype: string = "";
    toAmount: any;
    fromGl: string = "";
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
    toHolderName: any;
    GLBalance: any;
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

        if (this.fundTransferModel.transTo) {
            this.toAC = this.acList.find(it => {
                //return it.value.toLowerCase().includes(this.fundTransferModel.transTo.toLowerCase());
                return it.value == this.fundTransferModel.transTo;
            }).label;
        }
        this.toHolderName = this.toAC;
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
                            this.fundTransferModel.toCatId = null;
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

                    this.fundTransferModel.payAmt = 0;
                    this.initialiseGridConfig();

                },
                error => {
                    console.log(error);
                }
            );
    }

    //fill amount against Sys code
    fillAmount(SelectedGL, formOrTotype) {
        this.isLoading = true;
        this.fundTransferService.getAmountByGL(SelectedGL)
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
                            this.fromAmount = data;
                            this.fromCategory = 'System';
                            this.fundTransferModel.fromCatId = 'S';
                        }

                    }

                    else {
                        this.toAmount = data;
                        this.toCategory = 'System';
                    }

                    this.fundTransferModel.payAmt = 0;
                    this.initialiseGridConfig();

                },
                error => {
                    console.log(error);
                }
            );
    }

    async GetTransactionDetailsByPayAmount() {
        if (this.fundTransferModel.payAmt == '') {
            this.fundTransferModel.payAmt = 0;
        }
        if (this.fundTransferModel.fromSysCoaCode && this.fundTransferModel.transTo) {
            this.transAmtLimit = this.currentUserModel.user.tranAmtLimit;
            if (+this.fundTransferModel.payAmt <= this.transAmtLimit) {
                if (+this.fromAmount >= +this.fundTransferModel.payAmt) {
                    this.fromGl = this.glList.find(it => {
                        return it.value.toLowerCase().includes(this.fundTransferModel.fromSysCoaCode.toLowerCase());
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
                this.messageService.add({ severity: 'warn', summary: 'Exceed Limit', detail: 'Limit Amount :' + this.transAmtLimit });
                this.fundTransferModel.payAmt = 0;
            }
        }
        else {
            this.messageService.add({ severity: 'warn', summary: 'Select from GL to AC', detail: 'First select from GL to AC' });
            this.fundTransferModel.payAmt = 0;
        }

        this.initialiseGridConfig();

    }

    initialiseGridConfig(): any {
        this.isLoading = true;
        this.fundTransferModel.hotkey = "GL TO AC";
        //this.fundTransferService.GetTransactionDetailsByPayAmount(this.fundTransferModel.payAmt)
        this.fundTransferService.GetTransactionDetailsByPayAmount(this.fundTransferModel, this.fromGl, this.toAC)
            .pipe(first())
            .subscribe(
                data => {
                    this.isLoading = false;
                    this.vMTransactionDetailList = data;
                    //this.fundTransferModel.toSysCoaCode = data[1].glCode;
                    this.fundTransferModel.toSysCoaCode = data[1].glSysCoaCode;
                    this.toAC = data[1].coaDesc;

                    if (data[0].glCode != '') {
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
            { field: 'glCode', header: 'GL No/Account No', width: '30%', template: 'none'  },
            { field: 'glName', header: 'GL/Account Name', width: '30%', filter: this.gridSettingService.getFilterableNone(), template: 'none'  },
            { field: 'debitAmount', header: 'Debit Amount', width: '20%', filter: this.gridSettingService.getFilterableNone(), template: this.gridSettingService.getMoneyTemplateForRowData() },
            { field: 'creditAmount', header: 'Credit Amount', width: '20%', filter: this.gridSettingService.getFilterableNone(), template: this.gridSettingService.getMoneyTemplateForRowData() }
        ];

    };

    saveFundTransferEntry(event): any {

        if (!this.fundTransferModel.fromSysCoaCode || this.fundTransferModel.fromSysCoaCode == '' ||
            !this.fundTransferModel.transTo || this.fundTransferModel.transTo == '' ||
            !this.fundTransferModel.payAmt || this.fundTransferModel.payAmt == '0' || this.fundTransferModel.payAmt == '') {
            this.msgs = [];
            this.msgs.push({ severity: 'error', summary: 'Warning! ', detail: 'Cannot be left blank' });
            this.error = true;
        }
        else {

            if (!this.isEditMode) {
                this.fundTransferModel.entryBrCode = this.currentUserModel.user.branchCode;
                this.fundTransferModel.entryUser = this.currentUserModel.user.username;
            }
          
            if (this.fundTransferModel.fromSysCoaCode) {
                this.isLoading = true;
                this.fundTransferService.GetGLBalanceByGLSysCoaCode(this.fundTransferModel.fromSysCoaCode)
                    .pipe(first())
                    .subscribe(
                        data => {
                         
                            this.GLBalance = data;
                            this.onConfirmSave();
                        },
                        error => {
                            console.log(error);
                        }
                    );
            }

            
        }
    }

    onConfirmSave() {
        if (+this.fundTransferModel.payAmt <= +this.GLBalance) {
            if (this.fromGl != "" || this.toAC != "") {
                this.fundTransferModel.hotkey = "GL TO AC";//for data base save
              
                this.fundTransferService.saveFundTransferEntry(this.fundTransferModel, this.fromGl, this.toAC, this.isEditMode, event).pipe(first())
                    .subscribe(
                        data => {
                          
                            if (this.isEditMode)
                                this.messageService.add({ severity: 'success', summary: 'Update successfully', detail: 'Fund entry GL TO AC updated' });
                            else
                                this.messageService.add({ severity: 'success', summary: 'Save successfully', detail: 'Fund entry GL TO AC added' });
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
        else {
            this.messageService.add({ severity: 'warn', summary: 'Exceed from amount', detail: 'Cannot be greater than from amount :' + this.GLBalance });
            this.fundTransferModel.payAmt = 0;
        }
    }
}

