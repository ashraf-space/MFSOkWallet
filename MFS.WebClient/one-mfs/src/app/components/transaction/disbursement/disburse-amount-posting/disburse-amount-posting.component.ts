import { Component, OnInit } from '@angular/core';
import { MfsSettingService } from 'src/app/services/mfs-setting.service';
import { GridSettingService } from 'src/app/services/grid-setting.service';
import { AuthenticationService } from 'src/app/shared/_services';
import { MessageService, Message } from 'primeng/api';
import { disbursementService } from 'src/app/services/transaction';
import { first } from 'rxjs/operators';

@Component({
  selector: 'app-disburse-amount-posting',
  templateUrl: './disburse-amount-posting.component.html',
  styleUrls: ['./disburse-amount-posting.component.css']
})
export class DisburseAmountPostingComponent implements OnInit {
    disburseAmoutPostingModel: any = {};
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
    isEditMode: boolean = false;
    msgs: Message[] = [];
    error: boolean = false;
    transAmtLimit: any;
    isLoading: boolean = false;

    constructor(private disbursementService: disbursementService,  private mfsSettingService: MfsSettingService,
        private gridSettingService: GridSettingService
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
        this.disbursementService.getTransactionList(this.transAmtLimit)
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
        this.disbursementService.GetTransactionDetailsByTransactionNo(this.fundTransferModel.transNo)
            .pipe(first())
            .subscribe(
                data => {
                    this.isLoading = false;
                    this.vMTransactionDetailList = data;
                    if (this.fundTransferModel.transNo != "") {
                        this.glCode = data[0].disburseAC;
                        this.glName = data[0].company;
                        this.amount = data[0].debitAmount;
                    }
                },
                error => {
                    console.log(error);
                }
            );

        this.cols = [
            { field: 'glCode', header: 'GL Code', width: '30%' },
            { field: 'glName', header: 'GL Name', width: '30%', filter: this.gridSettingService.getFilterableNone() },
            { field: 'debitAmount', header: 'Debit Amount', width: '20%', filter: this.gridSettingService.getFilterableNone() },
            { field: 'creditAmount', header: 'Credit Amount', width: '20%', filter: this.gridSettingService.getFilterableNone() }
        ];

    };

    AproveOrRejectDisburseAmountPosting(event): any {
        if (this.fundTransferModel.transNo) {
            this.fundTransferModel.checkUser = this.currentUserModel.user.username;
        }
        if (!this.fundTransferModel.transNo || this.fundTransferModel.transNo == '') {
            this.msgs = [];
            this.msgs.push({ severity: 'error', summary: 'Warning! ', detail: 'Cannot be left blank' });
            this.error = true;
        }
        else {
           
            this.isLoading = true;
            this.disbursementService.AproveOrRejectDisburseAmountPosting(this.fundTransferModel, event).pipe(first())
                .subscribe(
                    data => {

                        if (event == "register")
                            //this.messageService.add({ severity: 'success', summary: 'Register successfully', detail: 'Fund transfer GL to GL transferred' });                            
                            if (data == "1") {
                                this.messageService.add({ severity: 'success', summary: 'Approved successfully', detail: 'Disburse amount posting is successful' });
                            }
                            else if (data == "2") {
                                this.messageService.add({ severity: 'error', summary: 'Not Approved', detail: 'Sorry! Insufficient balance.' });
                            }
                            else {
                                this.messageService.add({ severity: 'error', summary: 'Not Approved', detail: 'Sorry! Something wrong.' });
                            }
                        else
                            this.messageService.add({ severity: 'success', summary: 'Reject successfully', detail: 'Disburse amount posting is rejected' });
                        //console.log(data);
                        setTimeout(() => {
                            this.isLoading = false;
                            location.reload();
                        }, 100);//1000ms=1sec
                        

                    },
                    error => {
                        console.log(error);
                    });

        }

    }

}
