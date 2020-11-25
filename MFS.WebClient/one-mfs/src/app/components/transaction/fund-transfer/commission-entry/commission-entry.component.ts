import { Component, OnInit } from '@angular/core';
import { GridSettingService } from 'src/app/services/grid-setting.service';
import { AuthenticationService } from 'src/app/shared/_services';
import { MfsSettingService } from 'src/app/services/mfs-setting.service';
import { MessageService } from 'primeng/api';
import { FundTransferService } from 'src/app/services/transaction';
import { first } from 'rxjs/operators';

@Component({
    selector: 'app-commission-entry',
    templateUrl: './commission-entry.component.html',
    styleUrls: ['./commission-entry.component.css']
})
export class CommissionEntryComponent implements OnInit {
    isLoading: boolean = false;
    commissionGlList: any;
    gridConfig: any;
    isActionDisable: boolean = true;
    isDisableGL: boolean = true;
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
        this.checkPendingApproval();
    }
    checkPendingApproval(): any {
        this.isLoading = true;
        this.fundTransferService.checkPendingApproval()
            .pipe(first())
            .subscribe(
                data => {
                    this.isLoading = false;                   
                    if (data == 'M') {
                        this.messageService.add({ severity: 'warn', summary: 'Approval pending', sticky: true, detail: 'Action not yet performed by checker!' });
                        this.isDisableGL = true;
                    }
                    else {
                        this.isDisableGL = false;
                    }
                },
                error => {
                    this.isLoading = false;
                    console.log(error);
                }
            );
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
            { field: 'glCode', header: 'GL Code', width: '10%', filter: this.gridSettingService.getDefaultFilterable() },
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
            this.entryOrApproval = 'ForEntry';
            this.fundTransferService.GetCommssionMobileList(this.sysCoaCode,this.entryOrApproval)
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
                            this.totalAmount= sum;

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


   
    saveCommissionEntry(): any {
        this.entryBy = this.currentUserModel.user.username;
        this.entryBrCode = this.currentUserModel.user.branchCode;

        this.SelectedCommissionEntryModel = this.commissionEntryModel.filter(it => {
            return it.makeStatus == true;
        });

        this.toCatId = this.sysCoaCode == 'L40000000087' ? 'D' : 'A';


        if (this.SelectedCommissionEntryModel.length > 0) {
            this.isLoading = true;
            this.fundTransferService.SaveCommissionEntry(this.toCatId, this.entryBy, this.entryBrCode, this.SelectedCommissionEntryModel).pipe(first())
                .subscribe(
                    data => {
                        this.isLoading = false;
                        if (data == 1) {
                            this.messageService.add({ severity: 'success', summary: 'Save successfully', detail: 'Commission entry added' });
                        }
                        else {
                            this.messageService.add({ severity: 'error', summary: 'Not Save', detail: data });
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
}
