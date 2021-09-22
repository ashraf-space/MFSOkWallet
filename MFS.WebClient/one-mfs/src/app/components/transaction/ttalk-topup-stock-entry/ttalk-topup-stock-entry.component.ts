import { Component, OnInit } from '@angular/core';
import { MessageService, Message } from 'primeng/api';
import { first } from 'rxjs/operators';
import { GridSettingService } from 'src/app/services/grid-setting.service';
import { MfsSettingService } from 'src/app/services/mfs-setting.service';
import { FundTransferService } from 'src/app/services/transaction';
import { AuthenticationService } from 'src/app/shared/_services';

@Component({
  selector: 'app-ttalk-topup-stock-entry',
  templateUrl: './ttalk-topup-stock-entry.component.html',
  styleUrls: ['./ttalk-topup-stock-entry.component.css']
})
export class TtalkTopupStockEntryComponent implements OnInit {
    ttalkTopupStockEntryModel: any = {};
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
        //this.ttalkTopupStockEntryModel.discountRatio = 0.9779951100244499;
        this.ttalkTopupStockEntryModel.discountRatio = 0.9775;
        this.initialiseGridConfig();
        this.getGlDetailsForTtalk();
    }

    async getGlDetailsForTtalk() {
        this.fundTransferService.getGlDetailsForTtalk()
            .pipe(first())
            .subscribe(
                data => {
                    this.ttalkTopupStockEntryModel.glCode = data.GLCODE;
                    this.ttalkTopupStockEntryModel.glName = data.GLNAME;
                    this.ttalkTopupStockEntryModel.fromSysCoaCode = data.SYSCOACODE;
                    this.ttalkTopupStockEntryModel.amount = data.AMOUNT;
                },
                error => {
                    console.log(error);
                }
            );
    }

    initialiseGridConfig(): any {
        this.ttalkTopupStockEntryModel.hotkey = "T2TOP TO GL";
        this.isLoading = true;
        this.fundTransferService.GetTransDtlForTtalkByPayAmount(this.ttalkTopupStockEntryModel)
            .pipe(first())
            .subscribe(
                data => {
                    this.isLoading = false;

                    if (data != null) {
                        this.vMTransactionDetailList = data;
                        this.ttalkTopupStockEntryModel.rowThreeFour = data[2].debitAmount;
                        this.ttalkTopupStockEntryModel.rowFiveSix = data[4].debitAmount;
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

    async GetTransDtlForTtalkByPayAmount() {
        if (
            !this.ttalkTopupStockEntryModel.transactionAmt || this.ttalkTopupStockEntryModel.transactionAmt == '0' ||
            !this.ttalkTopupStockEntryModel.discountRatio || this.ttalkTopupStockEntryModel.discountRatio == '0') {
            this.msgs = [];
            this.msgs.push({ severity: 'error', summary: 'Warning! ', detail: 'Cannot be left blank' });
            this.error = true;
        }
        else {
            this.initialiseGridConfig();
        }

    }


    saveTtalkTopupStockEntry(event): any {

        if (!this.ttalkTopupStockEntryModel.glCode || this.ttalkTopupStockEntryModel.glCode == '' ||
            !this.ttalkTopupStockEntryModel.transactionAmt || this.ttalkTopupStockEntryModel.transactionAmt == '0' ||
            !this.ttalkTopupStockEntryModel.discountRatio || this.ttalkTopupStockEntryModel.discountRatio == '0') {
            this.msgs = [];
            this.msgs.push({ severity: 'error', summary: 'Warning! ', detail: 'Cannot be left blank' });
            this.error = true;
        }
        else {

            this.ttalkTopupStockEntryModel.entryBrCode = this.currentUserModel.user.branchCode;
            this.ttalkTopupStockEntryModel.entryUser = this.currentUserModel.user.username;

            this.ttalkTopupStockEntryModel.fromCatId = "T2TOP";
            this.ttalkTopupStockEntryModel.toCatId = "GL";
            this.ttalkTopupStockEntryModel.hotkey = "T2TOP TO GL";//for data base save     
            this.isLoading = true;
            this.fundTransferService.saveTtalkTopupStockEntry(this.ttalkTopupStockEntryModel).pipe(first())
                .subscribe(
                    data => {
                        if (data == 'Failed') {
                            this.messageService.add({ severity: 'error', summary: 'Not Saved', detail: 'Sorry! Teletalk Topup Stock Entry Failed.' });
                        }
                        else {
                            this.messageService.add({ severity: 'success', summary: 'Save Successfully', detail: 'Teletalk TopUp stock entry added' });
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
