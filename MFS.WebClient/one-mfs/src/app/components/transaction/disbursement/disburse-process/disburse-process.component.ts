import { Component, OnInit, ViewEncapsulation, ViewChild, Input, Output, EventEmitter } from '@angular/core';
import { MessageService, Message } from 'primeng/api';
import { AuthenticationService } from 'src/app/shared/_services';
import { GridSettingService } from 'src/app/services/grid-setting.service';
import { disbursementService } from 'src/app/services/transaction';
import { first } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
declare let jsPDF: any;



@Component({
    selector: 'app-disburse-process',
    templateUrl: './disburse-process.component.html',
    styleUrls: ['./disburse-process.component.css'],
    encapsulation: ViewEncapsulation.None
})
export class DisburseProcessComponent implements OnInit {
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
    isProcessDisabled: boolean = false;
    isPostingDisabled: boolean = true;
    tblDisburseInvalidDataList: any;

    @ViewChild('fileInput') fileInput;
    message: string;
    allUsers: any = {};
    validOrInvalid: string = "";
    totalSum: number = 0;
    companyName: string = null;
    accountNo: any;

    constructor(private http: HttpClient, private disbursementService: disbursementService, private gridSettingService: GridSettingService,
        private authService: AuthenticationService, private messageService: MessageService) {

        this.gridConfig = {};

        this.authService.currentUser.subscribe(x => {
            this.currentUserModel = x;
        });
    }

    ngOnInit() {
        this.getdisburseTypeList();
        this.getDisburseCompanyList();
        this.getCompanyAndBatchNoList();

        this.initialiseGridConfig();
    }
    getCompanyAndBatchNoList(): any {
        this.disbursementService.getCompanyAndBatchNoList(null)
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
    getdisburseTypeList(): any {
        this.disbursementService.getDisburseTypeList()
            .pipe(first())
            .subscribe(
                data => {
                    this.disburseTypeList = data;
                },
                error => {
                    console.log(error);
                }
            );
    }
    getDisburseCompanyList(): any {
        this.disbursementService.getDisburseCompanyList()
            .pipe(first())
            .subscribe(
                data => {
                    this.disburseCompanyList = data;
                },
                error => {
                    console.log(error);
                }
            );
    }

    createbatchNo() {
        if (this.tblDisburseModel.OrganizationId) {
            this.isLoading = true;
            this.disbursementService.getBatchNo(this.tblDisburseModel.OrganizationId, this.tblDisburseModel.DisburseType)
                .pipe(first())
                .subscribe(
                    data => {
                        this.isLoading = false;
                        this.accountNo = data["AccountNo"];
                        if (this.accountNo) {
                            this.tblDisburseModel.Batchno = data[":B1"];
                            this.tblDisburseModel.Amount = data[":B2"];
                        }
                        else {
                            this.tblDisburseModel.Batchno = null;
                            this.tblDisburseModel.Amount = 0;   
                            this.accountNo = 'No account found';
                        }
                                            
                        
                    },
                    error => {
                        console.log(error);
                    }
                );
        }
        else {
            this.messageService.add({ severity: 'warn', summary: 'Select Company', detail: 'Select Company first.' });
            this.tblDisburseModel.DisburseType = '';
        }

    }

    clearDisburseTypeAndBatchNo() {
        if (this.tblDisburseModel.OrganizationId) {
            this.tblDisburseModel.DisburseType = '';
            this.tblDisburseModel.Batchno = '';
        }
        else {
            this.messageService.add({ severity: 'warn', summary: 'Select Company', detail: 'Select Company first.' });
            this.tblDisburseModel.DisburseType = '';
        }

    }

    checkProcess() {
        if (this.processBatchNo) {
            this.isLoading = true;
            this.disbursementService.checkProcess(this.processBatchNo)
                .pipe(first())
                .subscribe(
                    data => {
                        this.isLoading = false;
                        this.tblDisburseInvalidDataList = null;
                        this.isPostingDisabled = true;
                        this.totalSum = 0;
                        if (data == true)
                            this.isProcessDisabled = true;
                        else
                            this.isProcessDisabled = false

                    },
                    error => {
                        console.log(error);
                    }
                );
        }
        else {
            this.messageService.add({ severity: 'warn', summary: 'Company & Batch No Empty', detail: 'Select Company & BatchNo First!' });
            this.tblDisburseModel.Batchno = '';
        }

    }

    uploadFile() {
        if (this.tblDisburseModel.Batchno) {
            this.tblDisburseModel.makerId = this.currentUserModel.user.username;
            this.isLoading = true;
            let formData = new FormData();
            formData.append('upload', this.fileInput.nativeElement.files[0])
            this.disbursementService.UploadExcel(formData, this.tblDisburseModel.OrganizationId, this.tblDisburseModel.Batchno, this.tblDisburseModel.makerId, this.tblDisburseModel.Amount).subscribe(result => {
                //this.message = result.toString();
                //this.loadAllUser();
                if (result.toString() == 'Excel file has been successfully uploaded')
                    this.messageService.add({ severity: 'success', summary: 'Uploaded successfully', detail: 'Excel file has been successfully uploaded' });
                else
                    this.messageService.add({ severity: 'warn', summary: 'Failed', detail: result.toString() });

                setTimeout(() => {
                    this.isLoading = false;
                    location.reload();
                }, 5000);
            });
        }
        else {
            this.messageService.add({ severity: 'warn', summary: 'Batch No Empty', detail: 'Create Batch No First!' });
        }

    }

    Process(): any {
        if (this.processBatchNo) {

            this.companyName = this.companyAndBatchNoList.find(it => {
                return it.value.toLowerCase().includes(this.processBatchNo.toLowerCase());
            }).label;


            this.isLoading = true;
            this.disbursementService.Process(this.processBatchNo, this.companyName)
                .pipe(first())
                .subscribe(
                    data => {
                        this.isLoading = false;
                        if (data == 'SUCCESS')
                            this.messageService.add({ severity: 'success', summary: 'Processed successfully', detail: 'Disburse process successfully done' });
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

    ShowValidInvaildData(validOrInvalid) {
        if (this.processBatchNo) {
            this.isLoading = true;
            //this.disbursementService.getValidOrInvalidData(this.processBatchNo, this.validOrInvalid)
            this.disbursementService.getValidOrInvalidData(this.processBatchNo, validOrInvalid, null)
                .pipe(first())
                .subscribe(
                    data => {
                        this.isLoading = false;
                        if (data.length > 0) {
                            this.tblDisburseInvalidDataList = data;
                            //this.totalSum = Math.round((data[data.length - 1].totalSum + Number.EPSILON) * 100) / 100;
                            this.gridConfig.dataSource = this.tblDisburseInvalidDataList;
                            
                            this.totalSum = data[data.length - 1].totalSum.toFixed(3);
                            if (validOrInvalid == 'V')
                                this.isPostingDisabled = false;
                            else
                                this.isPostingDisabled = true;
                        }
                        else {
                            this.tblDisburseInvalidDataList = null;
                            this.gridConfig.dataSource = null;
                            this.totalSum = 0;
                            this.isPostingDisabled = true;
                        }


                    },
                    error => {
                        console.log(error);
                    }
                );
            //this.cols = [
            //    { field: 'sl', header: 'SL No.', width: '10%' },
            //    { field: 'acNo', header: 'A/C No.', width: '20%', filter: this.gridSettingService.getFilterableNone() },
            //    { field: 'batchno', header: 'Batch No', width: '25%', filter: this.gridSettingService.getFilterableNone() },
            //    { field: 'amount', header: 'Amount', width: '20%', filter: this.gridSettingService.getFilterableNone() },
            //    { field: 'remarks', header: 'Remarks', width: '25%', filter: this.gridSettingService.getFilterableNone() }
            //];
        }
        else {
            this.messageService.add({ severity: 'warn', summary: 'Company & Batch No Empty', detail: 'Select Company & BatchNo First!' });
        }

    }


    SendToPostingLevel(): any {
        if (this.processBatchNo) {
            this.isLoading = true;
            this.disbursementService.SendToPostingLevel(this.processBatchNo, this.totalSum)
                .pipe(first())
                .subscribe(
                    data => {
                        this.isLoading = false;
                        if (data == 'SUCCESS')
                            this.messageService.add({ severity: 'success', summary: 'Posted successfully', detail: 'Disburse posting successfully done' });
                        else
                            this.messageService.add({ severity: 'warn', summary: 'Failed', detail: data.toString() });

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

    initialiseGridConfig(): any {

        this.gridConfig.dataSource = [];

        this.gridConfig.autoUpdateDataSource = false;
        this.gridConfig.autoIndexing = true;

        this.gridConfig.gridName = "Valid/Invalid Data";
        this.gridConfig.gridIconClass = 'fas fa-thumbtack';

        this.gridConfig.hasEditState = true;
        this.gridConfig.showUniversalFilter = false;

        this.gridConfig.columnList = [
            { field: 'sl', header: 'SL No.', width: '10%' },
            { field: 'acNo', header: 'A/C No.', width: '20%', filter: this.gridSettingService.getFilterableNone() },
            { field: 'batchno', header: 'Batch No', width: '25%', filter: this.gridSettingService.getFilterableNone() },
            { field: 'amount', header: 'Amount', width: '20%', filter: this.gridSettingService.getFilterableNone() },
            { field: 'remarks', header: 'Remarks', width: '25%', filter: this.gridSettingService.getFilterableNone() }
        ];
    };

}
