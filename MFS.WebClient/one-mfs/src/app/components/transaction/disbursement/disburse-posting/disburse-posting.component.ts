import { Component, OnInit, ViewChild } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { disbursementService } from 'src/app/services/transaction';
import { GridSettingService } from 'src/app/services/grid-setting.service';
import { AuthenticationService } from 'src/app/shared/_services';
import { MessageService, Message } from 'primeng/api';
import { first } from 'rxjs/operators';

@Component({
  selector: 'app-disburse-posting',
  templateUrl: './disburse-posting.component.html',
  styleUrls: ['./disburse-posting.component.css']
})
export class DisbursePostingComponent implements OnInit {
    gridConfig: any;
    currentUserModel: any = {};
    tblDisburseModel: any = {};
    msgs: Message[] = [];
    error: boolean = false;
    cols: any[];
    isLoading: boolean = false;
    disburseCompanyList: any;
    disburseTypeList: any;
    companyAndBatchNoList: any;
    processBatchNo: string;
    isAllSendDisabled: boolean = true;
    isBatchDeleteDisabled: boolean = true;
    tblDisburseInvalidDataList: any;

    @ViewChild('fileInput') fileInput;
    message: string;
    allUsers: any = {};
    validOrInvalid: string = "";
    totalSum: number = 0;
    brCode: string = "";
    checkerId: string="";

    constructor(private http: HttpClient, private disbursementService: disbursementService, private gridSettingService: GridSettingService,
        private authService: AuthenticationService, private messageService: MessageService) {

        this.gridConfig = {};

        this.authService.currentUser.subscribe(x => {
            this.currentUserModel = x;
        });
    }

    ngOnInit() {
        this.getCompanyAndBatchNoList();
    }
    getCompanyAndBatchNoList(): any {
        this.disbursementService.getCompanyAndBatchNoList('forPosting')
            .pipe(first())
            .subscribe(
                data => {
                    this.companyAndBatchNoList = data;
                },
                error => {
                    console.log(error);
                }
            );
    }
        

    //Process(): any {
    //    if (this.processBatchNo) {
    //        this.isLoading = true;
    //        this.disbursementService.Process(this.processBatchNo)
    //            .pipe(first())
    //            .subscribe(
    //                data => {
    //                    this.isLoading = false;
    //                    if (data == true)
    //                        this.messageService.add({ severity: 'success', summary: 'Processed successfully', detail: 'Disburse process successfully done' });
    //                    else
    //                        this.messageService.add({ severity: 'warn', summary: 'Failed', detail: 'Something wrong happened' });

    //                    setTimeout(() => {
    //                        this.isLoading = false;
    //                        location.reload();
    //                    }, 100);
    //                },
    //                error => {
    //                    console.log(error);
    //                }
    //            );
    //    }
    //    else {
    //        this.messageService.add({ severity: 'warn', summary: 'Company & Batch No Empty', detail: 'Select Company & BatchNo First!' });
    //    }

    //};

    getDataForPosting() {
        if (this.processBatchNo) {
            this.isLoading = true;
            //this.disbursementService.getValidOrInvalidData(this.processBatchNo, this.validOrInvalid)
            this.disbursementService.getValidOrInvalidData(this.processBatchNo, 'V', 'forPosting')
                .pipe(first())
                .subscribe(
                    data => {
                        this.isLoading = false;
                        if (data.length > 0) {
                            this.tblDisburseInvalidDataList = data;
                            //this.totalSum = Math.round((data[data.length - 1].totalSum + Number.EPSILON) * 100) / 100;                           
                            this.totalSum = data[data.length - 1].totalSum.toFixed(3); 
                            this.isAllSendDisabled = false;
                            this.isBatchDeleteDisabled = false;
                        }
                        else {
                            this.tblDisburseInvalidDataList = null;
                            this.totalSum = 0;
                            this.isAllSendDisabled = true;
                            this.isBatchDeleteDisabled = true;
                        }


                    },
                    error => {
                        console.log(error);
                    }
                );
            this.cols = [
                { field: 'sl', header: 'SL No.', width: '10%' },
                { field: 'acNo', header: 'A/C No.', width: '10%', filter: this.gridSettingService.getFilterableNone() },
                { field: 'batchno', header: 'Batch No', width: '20%', filter: this.gridSettingService.getFilterableNone() },
                { field: 'amount', header: 'Amount', width: '20%', filter: this.gridSettingService.getFilterableNone() },
                { field: 'disburseType', header: 'Disburse Type', width: '10%', filter: this.gridSettingService.getFilterableNone() },
                { field: 'remarks', header: 'Remarks', width: '25%', filter: this.gridSettingService.getFilterableNone() }
            ];
        }
        else {
            this.messageService.add({ severity: 'warn', summary: 'Company & Batch No Empty', detail: 'Select Company & BatchNo First!' });
        }

    }


    AllSend(): any {
        if (this.processBatchNo) {
            this.isLoading = true;
            this.brCode = this.currentUserModel.user.branchCode;
            this.checkerId = this.currentUserModel.user.username;
            this.disbursementService.AllSend(this.processBatchNo, this.brCode, this.checkerId, this.totalSum)
                .pipe(first())
                .subscribe(
                    data => {
                        this.isLoading = false;
                        if (data == "1") 
                            this.messageService.add({ severity: 'success', summary: 'Posted successfully', detail: 'Disburse posting successfully done' });
                        else
                            this.messageService.add({ severity: 'warn', summary: 'Failed', detail: 'Something wrong happened' });

                        setTimeout(() => {
                            this.isLoading = false;
                            location.reload();
                        }, 5000);
                    },
                    error => {
                        console.log(error);
                    }
                );
        }
        else {
            this.messageService.add({ severity: 'warn', summary: 'Company & Batch No Empty', detail: 'Select Company & BatchNo First!' });
        }

    };

    BatchDelete(): any {
        if (this.processBatchNo) {
            this.isLoading = true;
            this.brCode = this.currentUserModel.user.branchCode;
            this.checkerId = this.currentUserModel.user.username;
            this.disbursementService.BatchDelete(this.processBatchNo, this.brCode, this.checkerId, this.totalSum)
                .pipe(first())
                .subscribe(
                    data => {
                        this.isLoading = false;
                        if (data == 'SUCCESS')
                            this.messageService.add({ severity: 'success', summary: 'Delete successfully', detail: 'Disburse delete successfully done' });
                        else
                            this.messageService.add({ severity: 'warn', summary: 'Failed', detail: 'Something wrong happened' });

                        setTimeout(() => {
                            this.isLoading = false;
                            location.reload();
                        }, 5000);
                    },
                    error => {
                        console.log(error);
                    }
                );
        }
        else {
            this.messageService.add({ severity: 'warn', summary: 'Company & Batch No Empty', detail: 'Select Company & BatchNo First!' });
        }

    };



}
