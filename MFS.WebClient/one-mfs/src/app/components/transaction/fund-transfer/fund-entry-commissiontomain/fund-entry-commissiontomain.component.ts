import { Component, OnInit } from '@angular/core';
import { first } from 'rxjs/operators';
import { FundTransferService } from 'src/app/services/transaction';
import { MfsSettingService } from 'src/app/services/mfs-setting.service';
import { GridSettingService } from 'src/app/services/grid-setting.service';
import { Message } from '@angular/compiler/src/i18n/i18n_ast';

@Component({
  selector: 'app-fund-entry-commissiontomain',
  templateUrl: './fund-entry-commissiontomain.component.html',
  styleUrls: ['./fund-entry-commissiontomain.component.css']
})
export class FundEntryCommissiontomainComponent implements OnInit {
    fundEntryTypeList: any;
    gridConfig: any;
    acList: any;
    fundTransferModel: any = {};
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
    fromHolderName: string = null;

    constructor(private fundTransferService: FundTransferService, private mfsSettingService: MfsSettingService, private gridSettingService: GridSettingService) {
        this.gridConfig = {};
    }

    ngOnInit() {
        this.initialiseGridConfig();

        this.getACList();
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


    //fill amount against mphone
    fillAmountByMphone(SelectedAC, formOrTotype) {

        if (this.fundTransferModel.transFrom) {
            this.fromAC = this.acList.find(it => {
                //return it.value.toLowerCase().includes(this.fundTransferModel.transFrom.toLowerCase());
                return it.value == this.fundTransferModel.transFrom;
            }).label;
        }
        this.fromHolderName = this.fromAC;
        this.fundTransferService.getAmountByAC(SelectedAC)
            .pipe(first())
            .subscribe(
                data => {
                    if (data != null) {
                        if (formOrTotype == 'From') {
                            this.fromAmount = data["AMOUNT"];
                            this.fromCategory = data["CATEGORY"];
                        }

                        else {
                            this.toAmount = data["AMOUNT"];
                            this.toCategory = data["CATEGORY"];
                        }
                    }
                   

                },
                error => {
                    console.log(error);
                }
            );
    }

    async GetTransactionDetailsByPayAmount() {

        this.fromAC = this.acList.find(it => {
            return it.value.toLowerCase().includes(this.fundTransferModel.transFrom.toLowerCase());
        }).label;

        //this.toAC = this.acList.find(it => {
        //    return it.value.toLowerCase().includes(this.fundTransferModel.transTo.toLowerCase());
        //}).label;


        this.initialiseGridConfig();

    }

    initialiseGridConfig(): any {

        //this.fundTransferService.GetTransactionDetailsByPayAmount(this.fundTransferModel.payAmt)
        this.fundTransferService.GetTransactionDetailsByPayAmount(this.fundTransferModel, this.fromAC, this.toAC)
            .pipe(first())
            .subscribe(
                data => {

                    this.vMTransactionDetailList = data;

                },
                error => {
                    console.log(error);
                }
            );

        this.cols = [
            { field: 'acNo', header: 'A/C No', width: '30%' },
            { field: 'glCode', header: 'GL No', width: '30%' },
            { field: 'glName', header: 'GL Name', width: '30%', filter: this.gridSettingService.getFilterableNone() },
            { field: 'debitAmount', header: 'Debit Amount', width: '20%', filter: this.gridSettingService.getFilterableNone() },
            { field: 'creditAmount', header: 'Credit Amount', width: '20%', filter: this.gridSettingService.getFilterableNone() }
        ];

    };
}
