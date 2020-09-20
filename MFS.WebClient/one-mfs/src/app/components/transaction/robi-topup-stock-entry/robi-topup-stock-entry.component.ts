import { Component, OnInit } from '@angular/core';
import { FundTransferService } from 'src/app/services/transaction';
import { MfsSettingService } from 'src/app/services/mfs-setting.service';
import { GridSettingService } from 'src/app/services/grid-setting.service';
import { AuthenticationService } from 'src/app/shared/_services';
import { MessageService, Message } from 'primeng/api';
import { first } from 'rxjs/operators';

@Component({
    selector: 'app-robi-topup-stock-entry',
    templateUrl: './robi-topup-stock-entry.component.html',
    styleUrls: ['./robi-topup-stock-entry.component.css']
})
export class RobiTopupStockEntryComponent implements OnInit {

    robiTopupStockEntryModel: any = {};
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
        this.robiTopupStockEntryModel.discountRatio = .98560;
        this.initialiseGridConfig();
        this.getGlDetailsForRobi();
    }

    async getGlDetailsForRobi() {
        this.fundTransferService.getGlDetailsForRobi()
            .pipe(first())
            .subscribe(
                data => {
                    this.robiTopupStockEntryModel.glCode = data.GLCODE;
                    this.robiTopupStockEntryModel.glName = data.GLNAME;
                    this.robiTopupStockEntryModel.fromSysCoaCode = data.SYSCOACODE;
                    this.robiTopupStockEntryModel.amount = data.AMOUNT;
                },
                error => {
                    console.log(error);
                }
            );
    }

    initialiseGridConfig(): any {
        this.robiTopupStockEntryModel.hotkey = "RBTOP TO GL";
        this.isLoading = true;
        this.fundTransferService.GetTransDtlForRobiByPayAmount(this.robiTopupStockEntryModel)
            .pipe(first())
            .subscribe(
                data => {
                    this.isLoading = false;

                    if (data != null) {
                        this.vMTransactionDetailList = data;
                        this.robiTopupStockEntryModel.rowThreeFour = data[2].debitAmount;
                        this.robiTopupStockEntryModel.rowFiveSix = data[4].debitAmount;
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
            !this.robiTopupStockEntryModel.transactionAmt || this.robiTopupStockEntryModel.transactionAmt == '0' ||
            !this.robiTopupStockEntryModel.discountRatio || this.robiTopupStockEntryModel.discountRatio == '0') {
            this.msgs = [];
            this.msgs.push({ severity: 'error', summary: 'Warning! ', detail: 'Cannot be left blank' });
            this.error = true;
        }
        else {
            this.initialiseGridConfig();
        }

    }


    saveRobiTopupStockEntry(event): any {

        if (!this.robiTopupStockEntryModel.glCode || this.robiTopupStockEntryModel.glCode == '' ||
            !this.robiTopupStockEntryModel.transactionAmt || this.robiTopupStockEntryModel.transactionAmt == '0' ||
            !this.robiTopupStockEntryModel.discountRatio || this.robiTopupStockEntryModel.discountRatio == '0') {
            this.msgs = [];
            this.msgs.push({ severity: 'error', summary: 'Warning! ', detail: 'Cannot be left blank' });
            this.error = true;
        }
        else {

            this.robiTopupStockEntryModel.entryBrCode = this.currentUserModel.user.branchCode;
            this.robiTopupStockEntryModel.entryUser = this.currentUserModel.user.username;

            this.robiTopupStockEntryModel.fromCatId = "RBTOP";
            this.robiTopupStockEntryModel.toCatId = "GL";
            this.robiTopupStockEntryModel.hotkey = "RBTOP TO GL";//for data base save     
            this.isLoading = true;
            this.fundTransferService.saveRobiTopupStockEntry(this.robiTopupStockEntryModel).pipe(first())
                .subscribe(
                    data => {
                        this.messageService.add({ severity: 'success', summary: 'Save successfully', detail: 'Robi Topup Stock Entry added' });
                       
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
