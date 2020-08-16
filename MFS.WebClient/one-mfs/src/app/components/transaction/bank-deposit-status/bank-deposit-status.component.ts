import { Component, OnInit, ViewChild } from '@angular/core';
import { FundTransferService, TransactionMasterService } from 'src/app/services/transaction';
import { MfsSettingService } from 'src/app/services/mfs-setting.service';
import { GridSettingService } from 'src/app/services/grid-setting.service';
import { ActivatedRoute } from '@angular/router';
import { MessageService, Message } from 'primeng/api';
import { AuthenticationService, AuditTrailService } from 'src/app/shared/_services';
import { MfsUtilityService } from 'src/app/services/mfs-utility.service';
import { GenericGridComponent } from 'src/app/shared/directives/generic-grid/generic-grid.component';
import { first } from 'rxjs/operators';

@Component({
    selector: 'app-bank-deposit-status',
    templateUrl: './bank-deposit-status.component.html',
    styleUrls: ['./bank-deposit-status.component.css']
})
export class BankDepositStatusComponent implements OnInit {
    tblBdStatusModel: any = {};
    currentUserModel: any = {};
    gridConfig: any;
    fromDate: any = {};
    toDate: any = {};
    balanceTypeList: any;
    selectedBalanceType: any;
    @ViewChild(GenericGridComponent) child: GenericGridComponent;
    roleName: any;
    userName: any;
    isRegistrationPermitted: boolean = false;
    dateObj: any = {};
    loading: boolean = false;
    error: boolean = false;
    msgs: Message[] = [];
    checkingDataList: any;
    auditTrailModel: any = {};

    constructor(private fundTransferService: FundTransferService, private mfsSettingService: MfsSettingService, private gridSettingService: GridSettingService
        , private authService: AuthenticationService, private messageService: MessageService, private route: ActivatedRoute, private mfsUtilityService: MfsUtilityService
        , private transactionMasterService: TransactionMasterService,
        private auditTrailService: AuditTrailService) {
        this.gridConfig = {};

        this.authService.currentUser.subscribe(x => {
            this.currentUserModel = x;
        });
    }


    ngOnInit() {
        this.isRegistrationPermitted = this.authService.checkRegisterPermissionAccess(this.route.snapshot.routeConfig.path);
        //this.fromDate = this.mfsUtilityService.getFullDateByMonthParam(0, 1);
        //this.toDate = this.mfsUtilityService.getFullDateByMonthParam(0, 1);
        this.balanceTypeList = [
            { label: 'Commision Balance', value: 'Commision Balance' },
            { label: 'Main Balance', value: 'Main Balance' }
        ];
        this.selectedBalanceType = null;
        this.initialiseGridConfig();
    }


    getBankDepositStatusBySearch(): any {
        //this.fromDate = this.mfsUtilityService.getFullDateByMonthParam(0, 1);
        //this.toDate = this.mfsUtilityService.getFullDateByMonthParam(0, 1);
        if (!this.fromDate.year ||
            this.selectedBalanceType == '0' || this.selectedBalanceType == '' || this.selectedBalanceType == null) {
            this.msgs = [];
            this.msgs.push({ severity: 'error', summary: 'Warning! ', detail: 'Cannot be left blank' });
            this.error = true;
        }
        else {
            this.roleName = this.currentUserModel.user.role_Name;
            this.gridConfig.dataSourcePath = this.mfsSettingService.transactionApiServer +
                '/TransactionMaster/GetBankDepositStatus?fromDate=' + this.mfsUtilityService.renderDate(this.fromDate, true) + '&toDate=' + this.mfsUtilityService.renderDate(this.toDate, true) + '&balanceType=' + this.selectedBalanceType + '&roleName=' + this.roleName;

            this.child.updateDataSource();

            //insert into audit trial 
            this.insertDataToAuditTrail();
        }
    }

    insertDataToAuditTrail() {
        this.auditTrailModel.Who = this.currentUserModel.user.username;
        this.auditTrailModel.WhatAction = 'SEARCH';
        this.auditTrailModel.WhatActionId = this.auditTrailService.getWhatActionId('SEARCH');
        var eventLog = JSON.parse(sessionStorage.getItem('currentEvent'));
        this.auditTrailModel.WhichMenu = eventLog.item.label.trim();
        this.auditTrailModel.WhichParentMenu = this.currentUserModel.featureList.find(it => {
            return it.FEATURENAME.includes(this.auditTrailModel.WhichMenu);
        }).CATEGORYNAME;
        this.auditTrailModel.WhichParentMenuId = this.auditTrailService.getWhichParentMenuId(this.auditTrailModel.WhichParentMenu);
        this.auditTrailModel.inputFeildAndValue = [{ whichFeildName: 'fromDate', whatValue: this.mfsUtilityService.renderDate(this.fromDate, true) }
            , { whichFeildName: 'toDate', whatValue: this.mfsUtilityService.renderDate(this.toDate, true) }
            , { whichFeildName: 'BalanceType', whatValue: this.selectedBalanceType }];
        this.auditTrailService.insertIntoAuditTrail(this.auditTrailModel).pipe(first())
            .subscribe(
                data => {
                    if (data) {
                    }
                },
                error => {

                });
    }


    initialiseGridConfig(): any {
        //this.roleName = 'SOM';
        this.roleName = this.currentUserModel.user.role_Name;
        this.gridConfig.dataSource = [];
        //if (this.fromDate && this.fromDate != '' && this.toDate && this.toDate != '') {
        //this.gridConfig.dataSourcePath = this.mfsSettingService.transactionApiServer +
        //    '/TransactionMaster/GetBankDepositStatus?fromDate=' + this.mfsUtilityService.renderDate(this.dateObj.fromDate, true) + '&toDate=' + this.mfsUtilityService.renderDate(this.dateObj.toDate , true) + '&balanceType=' + this.selectedBalanceType + '&isRegistrationPermitted=' + this.isRegistrationPermitted;

        //}

        this.gridConfig.autoUpdateDataSource = true;
        this.gridConfig.autoIndexing = true;

        //this.gridConfig.isBatchSwitchBoxEdit = true;
        this.gridConfig.gridName = "Bank Deposit Status";
        this.gridConfig.gridIconClass = 'fas fa-thumbtack';

        this.gridConfig.hasEditState = true;
        //this.gridConfig.entityField = 'mphone';
        this.gridConfig.showUniversalFilter = false;


        this.gridConfig.columnList = [
            { field: 'tranno', header: 'Tnx Id', width: '10%' },
            { field: 'transDate', header: 'Tnx Date', width: '8%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'transTime', header: 'Tnx Time', width: '8%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'category', header: 'Category', width: '7%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'distributorHouse', header: 'Distributor House', width: '13%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'phone', header: 'Phone', width: '10%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'distributorCode', header: 'Distributor Code', width: '7%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'branchName', header: 'Branch Name', width: '15%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'bankAcNo', header: 'BankAcNo ', width: '10%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'tranAmt', header: 'Refund Amt', width: '7%', filter: this.gridSettingService.getDefaultFilterable() },
            //{ field: 'make_status_dump', header: 'Action', width: '5%', isSwitchBoxColumn: true, filter: this.gridSettingService.getDefaultFilterable() }
            { field: 'makeStatus', header: 'Action', width: '5%', isSwitchBoxColumn: true, filter: this.gridSettingService.getDefaultFilterable() }
        ];
    };

    approveOrReject(event): any {
        this.tblBdStatusModel = this.child.getDatafromDataSource();

        if (this.tblBdStatusModel.length != 0) {

            //for checking purpose
            //this.getBankDepositStatusForChecking(event);

            //if (this.tblBdStatusModel.length == this.checkingDataList.length) {
            this.roleName = this.currentUserModel.user.role_Name;
            this.userName = this.currentUserModel.user.username;
            this.transactionMasterService.approveOrRejectBankDepositStatus(this.tblBdStatusModel, this.roleName, this.userName, event).pipe(first())
                .subscribe(
                    data => {
                        if (data == 'MissMatch')
                            this.messageService.add({ severity: 'error', summary: 'Data mismatch', detail: 'Please reload data again!!' });
                        else {
                            if (event == 'reject')
                                this.messageService.add({ severity: 'error', summary: 'Reject successfully', detail: 'Bank deposit status rejected' });
                            else if (event == 'register')
                                this.messageService.add({ severity: 'success', summary: 'Approved successfully', detail: 'Bank deposit status approved' });
                            else
                                this.messageService.add({ severity: 'success', summary: 'Pass successfully', detail: 'Bank deposit status passed' });
                        }


                        setTimeout(() => {
                            location.reload();
                        }, 5000);
                    },
                    error => {
                        console.log(error);
                    });
            //}
            //else {
            //    this.messageService.add({ severity: 'error', summary: 'Data mismatch', detail:'Please reload data again!!' });
            //}
        }
        else {
            this.messageService.add({ severity: 'error', summary: 'No data is selected' });
        }

    }

    getBankDepositStatusForChecking(event): any {
        //this.fromDate = this.mfsUtilityService.getFullDateByMonthParam(0, 1);
        //this.toDate = this.mfsUtilityService.getFullDateByMonthParam(0, 1);
        if (!this.fromDate.year ||
            this.selectedBalanceType == '0' || this.selectedBalanceType == '' || this.selectedBalanceType == null) {
            this.msgs = [];
            this.msgs.push({ severity: 'error', summary: 'Warning! ', detail: 'Cannot be left blank' });
            this.error = true;
        }
        else {
            this.roleName = this.currentUserModel.user.role_Name;
            //this.gridConfig.dataSourcePath = this.mfsSettingService.transactionApiServer +
            //    '/TransactionMaster/GetBankDepositStatus?fromDate=' + this.mfsUtilityService.renderDate(this.fromDate, true) + '&toDate=' + this.mfsUtilityService.renderDate(this.toDate, true) + '&balanceType=' + this.selectedBalanceType + '&roleName=' + this.roleName;

            this.transactionMasterService.getBankDepositStatusForChecking(this.mfsUtilityService.renderDate(this.fromDate, true), this.mfsUtilityService.renderDate(this.toDate, true), this.selectedBalanceType, this.roleName).pipe(first())
                .subscribe(
                    res => {
                        this.checkingDataList = res;

                        if (this.tblBdStatusModel.length == this.checkingDataList.length) {
                            this.roleName = this.currentUserModel.user.role_Name;
                            this.userName = this.currentUserModel.user.username;
                            this.transactionMasterService.approveOrRejectBankDepositStatus(this.tblBdStatusModel, this.roleName, this.userName, event).pipe(first())
                                .subscribe(
                                    data => {
                                        if (event == 'reject')
                                            this.messageService.add({ severity: 'error', summary: 'Reject successfully', detail: 'Bank deposit status rejected' });
                                        else if (event == 'register')
                                            this.messageService.add({ severity: 'success', summary: 'Approved successfully', detail: 'Bank deposit status approved' });
                                        else
                                            this.messageService.add({ severity: 'success', summary: 'Pass successfully', detail: 'Bank deposit status passed' });

                                        setTimeout(() => {
                                            location.reload();
                                        }, 5000);
                                    },
                                    error => {
                                        console.log(error);
                                    });
                        }
                        else {
                            this.messageService.add({ severity: 'error', summary: 'Data mismatch', detail: 'Please reload data again!!' });
                        }


                    },
                    error => {
                        console.log(error);
                    });
        }




    }

}
