import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { FundTransferService } from 'src/app/services/transaction';
import { first } from 'rxjs/operators';
import { MfsSettingService } from 'src/app/services/mfs-setting.service';
import { GridSettingService } from 'src/app/services/grid-setting.service';
import { AuthenticationService } from 'src/app/shared/_services';
import { MessageService, Message } from 'primeng/api';


@Component({
    selector: 'app-fund-entry',
    templateUrl: './fund-entry.component.html',
    styleUrls: ['./fund-entry.component.css'],
    encapsulation: ViewEncapsulation.None
})
export class FundEntryComponent implements OnInit {
    fundEntryTypeList: any;
    fundTransferModel: any = {};
    currentUserModel: any = {};
    entryBranchCode: string = "";
    gridConfig: any;
    glList: any;
    //glTransMst: any = {};
    SelectedGL: string = "";
    fromAmount: any;
    formOrTotype: string = "";
    toAmount: any;
    fromGl: string = "";
    toGl: string = "";
    selectedEntryType: string = "";
    isEditMode: boolean = false;
    isRegistrationPermitted: boolean = false;
    msgs: Message[] = [];
    error: boolean = false;

    cols: any[];
    vMTransactionDetails: any = {};
    vMTransactionDetailList: any;
    transAmtLimit: any;
    GLBalance: any;
    isLoading: boolean = false;
    isSaveDisable = true;
    glType: string = null;

    constructor(private fundTransferService: FundTransferService, private mfsSettingService: MfsSettingService, private gridSettingService: GridSettingService
        , private authService: AuthenticationService, private messageService: MessageService) {
        this.gridConfig = {};

        this.authService.currentUser.subscribe(x => {
            this.currentUserModel = x;
        });
    }

    ngOnInit() {
        this.initialiseGridConfig();
        this.getGlList();
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


    //fill amount against Sys code
    fillAmount(SelectedGL, formOrTotype) {      
        if (this.fundTransferModel.fromSysCoaCode) {
            this.fromGl = this.glList.find(it => {
                return it.value.toLowerCase().includes(this.fundTransferModel.fromSysCoaCode.toLowerCase());
            }).label;
        }

        if (this.fundTransferModel.toSysCoaCode) {
            this.toGl = this.glList.find(it => {
                return it.value.toLowerCase().includes(this.fundTransferModel.toSysCoaCode.toLowerCase());
            }).label;
        }

        if (this.fromGl != this.toGl) {
            this.isLoading = true;
            this.fundTransferService.getAmountByGL(SelectedGL)
                .pipe(first())
                .subscribe(
                    data => {
                        this.isLoading = false;
                        if (formOrTotype == 'From') {
                            if (data == null) {
                                this.fromAmount = 0;
                            }
                            else {
                                this.fromAmount = data;
                            }
                        }
                        else {
                            if (data == null) {
                                this.toAmount = 0;
                            }
                            else {
                                this.toAmount = data;
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
        else {
            if (formOrTotype == 'From') {
                this.messageService.add({ severity: 'warn', summary: 'Can not be same', detail: 'Can not be same as to GL.' });
                this.fundTransferModel.fromSysCoaCode = null;
                this.fromAmount = 0;
            }
            else {
                this.messageService.add({ severity: 'warn', summary: 'Can not be same', detail: 'Can not be same as from GL.' });
                this.fundTransferModel.toSysCoaCode = null;
                this.toAmount = 0;
            }
        }

    }

    async GetTransactionDetailsByPayAmount() {

        if (this.fundTransferModel.payAmt == '') {
            this.fundTransferModel.payAmt = 0;
        }
       
        if (this.fundTransferModel.fromSysCoaCode && this.fundTransferModel.toSysCoaCode) {
            this.transAmtLimit = this.currentUserModel.user.tranAmtLimit;
            this.glType = this.fundTransferModel.fromSysCoaCode.charAt(0);

            if (+this.fundTransferModel.payAmt <= this.transAmtLimit) {
                if ((this.glType == "L" || this.glType == "I")) {
                    if (+this.fundTransferModel.payAmt <= +this.fromAmount) {
                        this.fromGl = this.glList.find(it => {
                            return it.value.toLowerCase().includes(this.fundTransferModel.fromSysCoaCode.toLowerCase());
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
                    this.fromGl = this.glList.find(it => {
                        return it.value.toLowerCase().includes(this.fundTransferModel.fromSysCoaCode.toLowerCase());
                    }).label;

                    this.toGl = this.glList.find(it => {
                        return it.value.toLowerCase().includes(this.fundTransferModel.toSysCoaCode.toLowerCase());
                    }).label;
                }
                
            }
            else {
                this.messageService.add({ severity: 'warn', summary: 'Exceed Limit', detail: 'Your Limit Amount :' + this.transAmtLimit });
                this.fundTransferModel.payAmt = 0;
            }
        }
        else {
            this.messageService.add({ severity: 'warn', summary: 'Select from GL to GL', detail: 'First select from GL to GL' });
            this.fundTransferModel.payAmt = 0;
        }


        this.initialiseGridConfig();

    }

    initialiseGridConfig(): any {
        this.fundTransferModel.hotkey = "GL TO GL";
        this.isLoading = true;
        this.fundTransferService.GetTransactionDetailsByPayAmount(this.fundTransferModel, this.fromGl, this.toGl)
            .pipe(first())
            .subscribe(
                data => {
                    this.isLoading = false;
                    this.vMTransactionDetailList = data;

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
            { field: 'glCode', header: 'GL Code', width: '30%', template: 'none' },
            { field: 'glName', header: 'GL Name', width: '30%', filter: this.gridSettingService.getFilterableNone(), template: 'none' },
            { field: 'debitAmount', header: 'Debit Amount', width: '20%', filter: this.gridSettingService.getFilterableNone(), template: this.gridSettingService.getMoneyTemplateForRowData()  },
            { field: 'creditAmount', header: 'Credit Amount', width: '20%', filter: this.gridSettingService.getFilterableNone(), template: this.gridSettingService.getMoneyTemplateForRowData() }
        ];

    };

    saveFundTransferEntry(event): any {

        if (!this.fundTransferModel.fromSysCoaCode || this.fundTransferModel.fromSysCoaCode == '' ||
            !this.fundTransferModel.toSysCoaCode || this.fundTransferModel.toSysCoaCode == '' ||
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
        //if (+this.fundTransferModel.payAmt <= +this.GLBalance) {
            if (this.isEditMode && !this.isRegistrationPermitted) {
                this.fundTransferModel.updateUser = this.currentUserModel.user.username;
            }
            if (this.isEditMode && this.isRegistrationPermitted) {
                this.fundTransferModel.checkUser = this.currentUserModel.user.username;
            }


            if (this.fromGl != "" || this.toGl != "") {
                this.fundTransferModel.fromCatId = "S";
                this.fundTransferModel.toCatId = "S";
                this.fundTransferModel.hotkey = "GL TO GL";//for data base save
                
                this.fundTransferService.saveFundTransferEntry(this.fundTransferModel, this.fromGl, this.toGl, this.isEditMode, event).pipe(first())
                    .subscribe(
                        data => {
                            
                            if (this.isEditMode)
                                this.messageService.add({ severity: 'success', summary: 'Update successfully', detail: 'Fund entry GL to GL updated' });
                            else
                                this.messageService.add({ severity: 'success', summary: 'Save successfully', detail: 'Fund entry GL to GL added' });
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
        //}
        //else {
        //    this.messageService.add({ severity: 'warn', summary: 'Exceed from amount', detail: 'Cannot be greater than from amount :' + this.GLBalance });
        //    this.fundTransferModel.payAmt = 0;
        //}
    }

}
