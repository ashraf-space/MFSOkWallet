import { Component, OnInit } from '@angular/core';
import { FundTransferService } from 'src/app/services/transaction';
import { GridSettingService } from 'src/app/services/grid-setting.service';
import { AuthenticationService } from 'src/app/shared/_services';
import { MfsSettingService } from 'src/app/services/mfs-setting.service';
import { MessageService } from 'primeng/api';
import { first } from 'rxjs/operators';

@Component({
    selector: 'app-commission-approval',
    templateUrl: './commission-approval.component.html',
    styleUrls: ['./commission-approval.component.css']
})
export class CommissionApprovalComponent implements OnInit {

    isLoading: boolean = false;
    commissionGlList: any;
    gridConfig: any;
    isActionDisable: boolean = true;
    error: boolean = false;
    totalAmount: any;
    currentUserModel: any = {};
    entryBy: any;
    commissionEntryModel: any = {};
    SelectedCommissionEntryModel: any = {};
    sysCoaCode: any;
    toCatId: any;
    entryBrCode: any;
    entryOrApproval: string = null;
    fromCatId: string;

    constructor(private fundTransferService: FundTransferService, private gridSettingService: GridSettingService, private authService: AuthenticationService,
        private mfsSettingService: MfsSettingService, private messageService: MessageService) {
        this.gridConfig = {};
        this.authService.currentUser.subscribe(x => {
            this.currentUserModel = x;
        });
    }

    ngOnInit() {
        this.getCommissionGlList();
        this.loadInitialiseGrid();
    }

    getCommissionGlList(): any {
        this.isLoading = true;
        this.fundTransferService.getCommissionGlListForDDL()
            .pipe(first())
            .subscribe(
                data => {
                    this.isLoading = false;
                    this.commissionGlList = data;
                },
                error => {
                    this.isLoading = false;
                    console.log(error);
                }
            );
    }

    loadInitialiseGrid(): any {
        this.gridConfig.dataSource = [];

        this.gridConfig.autoUpdateDataSource = true;
        //this.gridConfig.autoIndexing = true;
        this.gridConfig.isBatchSwitchBoxEdit = true;

        //this.gridConfig.gridName = "From Agent";
        //this.gridConfig.gridIconClass = 'fas fa-thumbtack';

        //this.gridConfig.hasEditState = true;
        this.gridConfig.showUniversalFilter = false;
        this.gridConfig.showPaginator = false;
        this.gridConfig.showExport = false;
        this.gridConfig.columnLength = false;
        this.gridConfig.scrollable = true;


        this.gridConfig.columnList = [
            { field: 'transNo', header: 'Tnx No', width: '10%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'name', header: 'Name', width: '30%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'mphone', header: 'Mphone', width: '20%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'amount', header: 'Amount', width: '15%', filter: this.gridSettingService.getDefaultFilterable() },
            //{ field: 'transAmount', header: 'Transfer Amount', width: '15%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'makeStatus', header: 'Action', width: '10%', isSwitchBoxColumn: true, filter: this.gridSettingService.getFilterableNoneAndToggleSelectAll() }
        ];
    }

    getCommssionMobileList(): any {

        if (this.sysCoaCode != null && this.sysCoaCode != '0' && this.sysCoaCode.length > 0) {
            this.isLoading = true;
            this.entryOrApproval = 'ForApproval';

            //this.toCatId = this.sysCoaCode == 'L40000000087' ? 'D' : 'A';
            if (this.sysCoaCode == 'L40000000087') {
                this.toCatId = 'D';
                this.fromCatId = 'S1'
            }
            else if (this.sysCoaCode == 'L40000000046') {
                this.toCatId = 'A';
                this.fromCatId = 'S1'
            }
            else {
                this.toCatId = 'D';
                this.fromCatId = 'S2'
            }

            this.fundTransferService.GetCommssionMobileList(this.toCatId, this.fromCatId, this.entryOrApproval)
                .pipe(first())
                .subscribe(
                    data => {
                        this.isLoading = false;
                        if (data.length > 0) {

                            this.gridConfig.dataSource = data;
                            this.commissionEntryModel = data;
                            this.isActionDisable = false;

                            let sum = 0;
                            for (let i = 0; i < data.length; i++) {
                                sum += data[i].amount;
                            }
                            this.totalAmount = sum;

                        }
                        else {
                            this.gridConfig.dataSource = [];
                            this.commissionEntryModel = null;
                            this.isActionDisable = true;
                            this.totalAmount = 0;
                        }

                    },
                    error => {
                        this.isLoading = false;
                        this.isActionDisable = true;
                        console.log(error);
                    }
                );


        }
        else {
            this.gridConfig.dataSource = [];
            this.commissionEntryModel = null;
        }

    }



    AproveOrRejectCommissionEntry(event): any {
        this.entryBy = this.currentUserModel.user.username;
        this.entryBrCode = this.currentUserModel.user.branchCode;

        this.SelectedCommissionEntryModel = this.commissionEntryModel.filter(it => {
            return it.makeStatus == true;
        });

        //this.toCatId = this.sysCoaCode == 'L40000000087' ? 'D' : 'A';


        if (this.SelectedCommissionEntryModel.length > 0) {
            this.isLoading = true;
            this.fundTransferService.AproveOrRejectCommissionEntry(event, this.entryBy, this.SelectedCommissionEntryModel).pipe(first())
                .subscribe(
                    data => {
                        this.isLoading = false;
                        if (event == "register") {
                            if (data == "1") {
                                this.messageService.add({ severity: 'success', summary: 'Approved successfully', detail: 'Commission entry approved' });
                            }
                            else if (data == "2") {
                                this.messageService.add({ severity: 'error', summary: 'Not Approved', detail: 'Sorry! Insufficient balance.' });
                            }
                            else if (data == "Failed") {
                                this.messageService.add({ severity: 'error', summary: 'Failed', detail: 'Action is already performed, reload page' });
                            }
                            else {
                                this.messageService.add({ severity: 'error', sticky: true, summary: 'Not Approved', detail: data.toString() });
                            }
                        }

                        else {
                            if (data == "Failed") {
                                this.messageService.add({ severity: 'error', summary: 'Failed', detail: 'Action is already performed, reload page' });
                            }
                            else {
                                this.messageService.add({ severity: 'success', summary: 'Reject successfully', detail: 'Commission entry rejected' });
                            }
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
        else {
            this.messageService.add({ severity: 'error', summary: 'No data is selected' });
        }
    }

    //AproveOrRejectCommissionEntry(event): any {
    //    if (this.fundTransferModel.transNo) {
    //        this.fundTransferModel.checkUser = this.currentUserModel.user.username;
    //    }
    //    if (!this.fundTransferModel.transNo || this.fundTransferModel.transNo == '') {
    //        this.msgs = [];
    //        this.msgs.push({ severity: 'error', summary: 'Warning! ', detail: 'Cannot be left blank' });
    //        this.error = true;
    //    }
    //    else {
    //        //if (this.fundTransferModel.transNo != "") {
    //        this.transType = "A2A";
    //        this.isLoading = true;
    //        this.fundTransferService.AproveOrRejectFundTransfer(this.fundTransferModel, event, this.transType).pipe(first())
    //            .subscribe(
    //                data => {
    //                    if (event == "register") {
    //                        if (data == "1") {
    //                            this.messageService.add({ severity: 'success', summary: 'Approved successfully', detail: 'Fund transfer AC to AC is transfered' });
    //                        }
    //                        else if (data == "2") {
    //                            this.messageService.add({ severity: 'error', summary: 'Not Approved', detail: 'Sorry! Insufficient balance.' });
    //                        }
    //                        else if (data == "Failed") {
    //                            this.messageService.add({ severity: 'error', summary: 'Failed', detail: 'Action is already performed, reload page' });
    //                        }
    //                        else {
    //                            this.messageService.add({ severity: 'error', summary: 'Not Approved', detail: 'Sorry! Something wrong.' });
    //                        }
    //                    }

    //                    else {
    //                        if (data == "Failed") {
    //                            this.messageService.add({ severity: 'error', summary: 'Failed', detail: 'Action is already performed, reload page' });
    //                        }
    //                        else {
    //                            this.messageService.add({ severity: 'success', summary: 'Reject successfully', detail: 'Fund transfer AC to AC rejected' });
    //                        }
    //                    }

    //                    setTimeout(() => {
    //                        this.isLoading = false;
    //                        location.reload();
    //                    }, 5000);

    //                },
    //                error => {
    //                    console.log(error);
    //                });

    //    }

    //}

}
