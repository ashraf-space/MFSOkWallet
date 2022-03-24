import { Component, OnInit } from '@angular/core';
import { FundTransferService } from 'src/app/services/transaction';
import { MfsSettingService } from 'src/app/services/mfs-setting.service';
import { GridSettingService } from 'src/app/services/grid-setting.service';
import { AuthenticationService } from 'src/app/shared/_services';
import { MessageService, Message } from 'primeng/api';
import { first } from 'rxjs/operators';

@Component({
  selector: 'app-fund-transfer-gltoac',
  templateUrl: './fund-transfer-gltoac.component.html',
  styleUrls: ['./fund-transfer-gltoac.component.css']
})
export class FundTransferGltoacComponent implements OnInit {
    fundTransferModel: any = {};
    currentUserModel: any = {};
    gridConfig: any;
    transactionList: any;
    fromGl: string = "";
    toGl: string = "";
    vMTransactionDetailList: any;
    cols: any[];
    amount: any;
    glName: any;
    glCode: any;
    router: any;
    transType: string = null;
    toAcNo: any;
    isEditMode: boolean = false;
    msgs: Message[] = [];
    error: boolean = false;
    transAmtLimit: any;
    isLoading: boolean = false;
    remarks: any;



    constructor(private fundTransferService: FundTransferService, private mfsSettingService: MfsSettingService, private gridSettingService: GridSettingService
        , private authService: AuthenticationService, private messageService: MessageService) {

        this.gridConfig = {};

        this.authService.currentUser.subscribe(x => {
            this.currentUserModel = x;
        });

    }

    ngOnInit() {
        this.transAmtLimit = this.currentUserModel.user.tranAmtLimit;
        this.fundTransferModel.transNo = "";
        this.initialiseGridConfig();
        this.getTransactionList();
    }

    async getTransactionList() {
        this.fundTransferService.getTransactionList("GL TO AC", this.currentUserModel.user.branchCode, this.transAmtLimit)
            .pipe(first())
            .subscribe(
                data => {
                    this.transactionList = data;
                },
                error => {
                    console.log(error);
                }
            );
    }

    async GetTransactionDetailsByTransNo() {

        this.initialiseGridConfig();

    }

    initialiseGridConfig(): any {
        this.isLoading = true;
        this.fundTransferService.GetTransactionDetailsByTransactionNo(this.fundTransferModel.transNo)
            .pipe(first())
            .subscribe(
                data => {
                    this.isLoading = false;
                    this.vMTransactionDetailList = data;
                    if (this.fundTransferModel.transNo != "") {
                        this.glCode = data[0].glCode;
                        this.glName = data[0].glName;
                        this.amount = data[0].debitAmount;
                        this.toAcNo = data[1].acNo;
                        this.remarks = data[0].remarks;
                    }
                },
                error => {
                    console.log(error);
                }
            );

        this.cols = [
            { field: 'glCode', header: 'GL Code', width: '30%', template: 'none'  },
            { field: 'glName', header: 'GL Name', width: '30%', filter: this.gridSettingService.getFilterableNone(), template: 'none'  },
            { field: 'debitAmount', header: 'Debit Amount', width: '20%', filter: this.gridSettingService.getFilterableNone(), template: this.gridSettingService.getMoneyTemplateForRowData() },
            { field: 'creditAmount', header: 'Credit Amount', width: '20%', filter: this.gridSettingService.getFilterableNone(), template: this.gridSettingService.getMoneyTemplateForRowData() }
        ];

    };

    AproveOrRejectFundTransfer(event): any {
        if (this.fundTransferModel.transNo) {
            this.fundTransferModel.checkUser = this.currentUserModel.user.username;
        }
        if (!this.fundTransferModel.transNo || this.fundTransferModel.transNo == '') {
            this.msgs = [];
            this.msgs.push({ severity: 'error', summary: 'Warning! ', detail: 'Cannot be left blank' });
            this.error = true;
        }
        else {
        //if (this.fundTransferModel.transNo != "") {
            this.transType = "G2A";
            this.isLoading = true;
            this.fundTransferService.AproveOrRejectFundTransfer(this.fundTransferModel, event, this.transType).pipe(first())
                .subscribe(
                    data => {
                        
                        if (event == "register")
                            //this.messageService.add({ severity: 'success', summary: 'Register successfully', detail: 'Fund transfer GL to AC transferred' });
                            if (data == "1") {
                                this.messageService.add({ severity: 'success', summary: 'Approved successfully', detail: 'Fund transfer GL to AC is transfered' });
                            }
                            else if (data == "2") {
                                this.messageService.add({ severity: 'error', summary: 'Not Approved', detail: 'Sorry! Insufficient balance.' });
                            }
                            else if (data == "Failed") {
                                this.messageService.add({ severity: 'error', summary: 'Failed', detail: 'Action is already performed, reload page' });
                            }
                            else {
                                this.messageService.add({ severity: 'error', summary: 'Not Approved', detail: 'Sorry! Something wrong.' });
                            }
                        else {
                            if (data == "Failed") {
                                this.messageService.add({ severity: 'error', summary: 'Failed', detail: 'Action is already performed, reload page' });
                            }
                            else {
                                this.messageService.add({ severity: 'success', summary: 'Reject successfully', detail: 'Fund transfer GL to AC rejected' });
                            }
                        }
                            
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
