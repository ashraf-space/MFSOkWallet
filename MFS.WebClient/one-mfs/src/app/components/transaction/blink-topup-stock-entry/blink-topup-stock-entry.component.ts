import { Component, OnInit } from '@angular/core';
import { FundTransferService } from 'src/app/services/transaction';
import { MfsSettingService } from 'src/app/services/mfs-setting.service';
import { GridSettingService } from 'src/app/services/grid-setting.service';
import { MessageService, Message } from 'primeng/api';
import { AuthenticationService } from 'src/app/shared/_services';
import { first } from 'rxjs/operators';

@Component({
    selector: 'app-blink-topup-stock-entry',
    templateUrl: './blink-topup-stock-entry.component.html',
    styleUrls: ['./blink-topup-stock-entry.component.css']
})
export class BlinkTopupStockEntryComponent implements OnInit {
    blinkTopupStockEntryModel: any = {};
    currentUserModel: any = {};
    entryBranchCode: string = "";
    gridConfig: any;
    isEditMode: boolean = false;
    isRegistrationPermitted: boolean = false;
    msgs: Message[] = [];
    error: boolean = false;

    cols: any[];
    vMTransactionDetails: any = {};
    vMTransactionDetailList: any;

    isLoading: boolean = false;
    isSaveDisable = true;


    constructor(private fundTransferService: FundTransferService, private mfsSettingService: MfsSettingService, private gridSettingService: GridSettingService
        , private authService: AuthenticationService, private messageService: MessageService) {
        this.gridConfig = {};

        this.authService.currentUser.subscribe(x => {
            this.currentUserModel = x;
        });
    }

    ngOnInit() {
        this.blinkTopupStockEntryModel.discountRatio = 0.9910802775024777;
        this.initialiseGridConfig();
        this.getGlDetailsForBlink();
    }

    async getGlDetailsForBlink() {
        this.fundTransferService.getGlDetailsForBlink()
            .pipe(first())
            .subscribe(
                data => {
                    this.blinkTopupStockEntryModel.glCode = data.GLCODE;
                    this.blinkTopupStockEntryModel.glName = data.GLNAME;
                    this.blinkTopupStockEntryModel.fromSysCoaCode = data.SYSCOACODE;
                    this.blinkTopupStockEntryModel.amount = data.AMOUNT;
                },
                error => {
                    console.log(error);
                }
            );
    }

    initialiseGridConfig(): any {
        this.blinkTopupStockEntryModel.hotkey = "LBTOP TO GL";
        this.isLoading = true;
        this.fundTransferService.GetTransDtlForBlinkByPayAmount(this.blinkTopupStockEntryModel)
            .pipe(first())
            .subscribe(
                data => {
                    this.isLoading = false;

                    if (data != null) {
                        this.vMTransactionDetailList = data;
                        this.blinkTopupStockEntryModel.rowThreeFour = data[2].debitAmount;
                        this.blinkTopupStockEntryModel.rowFiveSix = data[4].debitAmount;
                        if (data[0].glCode != '') {
                            this.isSaveDisable = false;
                        }
                        else {
                            this.isSaveDisable = true;
                        }
                    }

                },
                error => {
                    console.log(error);
                }
            );

        this.cols = [
            { field: 'glCode', header: 'GL Code', width: '15%', template: 'none' },
            { field: 'glName', header: 'GL Name', width: '45%', filter: this.gridSettingService.getFilterableNone(), template: 'none' },
            { field: 'debitAmount', header: 'Debit Amount', width: '20%', filter: this.gridSettingService.getFilterableNone(), template: this.gridSettingService.getMoneyTemplateForRowData() },
            { field: 'creditAmount', header: 'Credit Amount', width: '20%', filter: this.gridSettingService.getFilterableNone(), template: this.gridSettingService.getMoneyTemplateForRowData() }
        ];

    };

    async GetTransDtlForRobiByPayAmount() {
        if (
            !this.blinkTopupStockEntryModel.transactionAmt || this.blinkTopupStockEntryModel.transactionAmt == '0' ||
            !this.blinkTopupStockEntryModel.discountRatio || this.blinkTopupStockEntryModel.discountRatio == '0') {
            this.msgs = [];
            this.msgs.push({ severity: 'error', summary: 'Warning! ', detail: 'Cannot be left blank' });
            this.error = true;
        }
        else {
            this.initialiseGridConfig();
        }

    }


    saveBlinkTopupStockEntry(event): any {

        if (!this.blinkTopupStockEntryModel.glCode || this.blinkTopupStockEntryModel.glCode == '' ||
            !this.blinkTopupStockEntryModel.transactionAmt || this.blinkTopupStockEntryModel.transactionAmt == '0' ||
            !this.blinkTopupStockEntryModel.discountRatio || this.blinkTopupStockEntryModel.discountRatio == '0') {
            this.msgs = [];
            this.msgs.push({ severity: 'error', summary: 'Warning! ', detail: 'Cannot be left blank' });
            this.error = true;
        }
        else {

            this.blinkTopupStockEntryModel.entryBrCode = this.currentUserModel.user.branchCode;
            this.blinkTopupStockEntryModel.entryUser = this.currentUserModel.user.username;

            this.blinkTopupStockEntryModel.fromCatId = "LBTOP";
            this.blinkTopupStockEntryModel.toCatId = "GL";
            this.blinkTopupStockEntryModel.hotkey = "LBTOP TO GL";//for data base save     
            this.isLoading = true;
            this.fundTransferService.saveBlinkTopupStockEntry(this.blinkTopupStockEntryModel).pipe(first())
                .subscribe(
                    data => {
                        if (data == 'Failed') {
                            this.messageService.add({ severity: 'error', summary: 'Not Saved', detail: 'Sorry! Banglalink Topup Stock Entry Failed.' });
                        }
                        else {
                            this.messageService.add({ severity: 'success', summary: 'Save Successfully', detail: 'Banglalink TopUp stock entry added' });
                        }
                        

                        setTimeout(() => {
                            this.isLoading = false;
                            location.reload();
                        }, 5000);

                    },
                    error => {
                        console.log(error);
                    });


        }



    }

}
