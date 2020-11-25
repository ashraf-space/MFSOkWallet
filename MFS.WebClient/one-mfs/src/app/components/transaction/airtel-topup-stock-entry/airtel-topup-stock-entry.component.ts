import { Component, OnInit } from '@angular/core';
import { FundTransferService } from 'src/app/services/transaction';
import { MfsSettingService } from 'src/app/services/mfs-setting.service';
import { GridSettingService } from 'src/app/services/grid-setting.service';
import { AuthenticationService } from 'src/app/shared/_services';
import { MessageService, Message } from 'primeng/api';
import { first } from 'rxjs/operators';

@Component({
    selector: 'app-airtel-topup-stock-entry',
    templateUrl: './airtel-topup-stock-entry.component.html',
    styleUrls: ['./airtel-topup-stock-entry.component.css']
})
export class AirtelTopupStockEntryComponent implements OnInit {

    airtelTopupStockEntryModel: any = {};
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
        this.airtelTopupStockEntryModel.discountRatio = .98560;
        this.initialiseGridConfig();
        this.getGlDetailsForAirtel();
    }

    async getGlDetailsForAirtel() {
        this.fundTransferService.getGlDetailsForAirtel()
            .pipe(first())
            .subscribe(
                data => {
                    this.airtelTopupStockEntryModel.glCode = data.GLCODE;
                    this.airtelTopupStockEntryModel.glName = data.GLNAME;
                    this.airtelTopupStockEntryModel.fromSysCoaCode = data.SYSCOACODE;
                    this.airtelTopupStockEntryModel.amount = data.AMOUNT;
                },
                error => {
                    console.log(error);
                }
            );
    }

    initialiseGridConfig(): any {
        this.airtelTopupStockEntryModel.hotkey = "ATTOP TO GL";
        this.isLoading = true;
        this.fundTransferService.GetTransDtlForAirtelByPayAmount(this.airtelTopupStockEntryModel)
            .pipe(first())
            .subscribe(
                data => {
                    this.isLoading = false;

                    if (data != null) {
                        this.vMTransactionDetailList = data;
                        this.airtelTopupStockEntryModel.rowThreeFour = data[2].debitAmount;
                        this.airtelTopupStockEntryModel.rowFiveSix = data[4].debitAmount;
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
            !this.airtelTopupStockEntryModel.transactionAmt || this.airtelTopupStockEntryModel.transactionAmt == '0' ||
            !this.airtelTopupStockEntryModel.discountRatio || this.airtelTopupStockEntryModel.discountRatio == '0') {
            this.msgs = [];
            this.msgs.push({ severity: 'error', summary: 'Warning! ', detail: 'Cannot be left blank' });
            this.error = true;
        }
        else {
            this.initialiseGridConfig();
        }

    }


    saveAirtelTopupStockEntry(event): any {

        if (!this.airtelTopupStockEntryModel.glCode || this.airtelTopupStockEntryModel.glCode == '' ||
            !this.airtelTopupStockEntryModel.transactionAmt || this.airtelTopupStockEntryModel.transactionAmt == '0' ||
            !this.airtelTopupStockEntryModel.discountRatio || this.airtelTopupStockEntryModel.discountRatio == '0') {
            this.msgs = [];
            this.msgs.push({ severity: 'error', summary: 'Warning! ', detail: 'Cannot be left blank' });
            this.error = true;
        }
        else {

            this.airtelTopupStockEntryModel.entryBrCode = this.currentUserModel.user.branchCode;
            this.airtelTopupStockEntryModel.entryUser = this.currentUserModel.user.username;

            this.airtelTopupStockEntryModel.fromCatId = "ATTOP";
            this.airtelTopupStockEntryModel.toCatId = "GL";
            this.airtelTopupStockEntryModel.hotkey = "ATTOP TO GL";//for data base save     
            this.isLoading = true;
            this.fundTransferService.saveAirtelTopupStockEntry(this.airtelTopupStockEntryModel).pipe(first())
                .subscribe(
                    data => {
                        if (data == 'Failed') {
                            this.messageService.add({ severity: 'error', summary: 'Not Saved', detail: 'Sorry! Airtel Topup Stock Entry Failed.' });
                        }
                        else {
                            this.messageService.add({ severity: 'success', summary: 'Save successfully', detail: 'Airtel Topup Stock Entry added' });
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
