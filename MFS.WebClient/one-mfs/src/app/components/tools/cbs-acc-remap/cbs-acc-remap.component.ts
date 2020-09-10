import { Component, OnInit, ViewChild } from '@angular/core';
import { GridSettingService } from 'src/app/services/grid-setting.service';
import { AuthenticationService } from 'src/app/shared/_services';
import { MfsSettingService } from 'src/app/services/mfs-setting.service';
import { MfsUtilityService } from 'src/app/services/mfs-utility.service';
import { GenericGridComponent } from 'src/app/shared/directives/generic-grid/generic-grid.component';
import { first } from 'rxjs/operators';
import { CustomerAccountMappingService } from '../../../services/tools/customer-account-mapping.service';
import { Router, ActivatedRoute } from '@angular/router';
import { MessageService, MenuItem } from 'primeng/api';
import { Message } from 'primeng/components/common/api';
@Component({
    selector: 'app-cbs-acc-remap',
    templateUrl: './cbs-acc-remap.component.html',
    styleUrls: ['./cbs-acc-remap.component.css']
})
export class CbsAccRemapComponent implements OnInit {

    mtCbsInfoModel: any = {};
    showGrid: boolean = false;
    gridConfig: any;
    currentUserModel: any;
    cbsResponseData: any;
    cbsResponseDataArray: any;
    cbsResponseDataModel: any;
    isAccountValid: any;
    mblAcc: any;
    mtCbsName: any;
    loading: boolean = false;
    loadingCbsInfo: boolean = false;
    showAccountValidationMsg: boolean = false;
    isRegPermit = false;
    changeby: any;
    showMsg: boolean = false;
    msgs: Message[] = [];
    isMapDisable: boolean = true;
    isSearchDisable: boolean = false;
    class: any;
    ubranch: any;

    @ViewChild(GenericGridComponent) childGrid: GenericGridComponent;
    constructor(private gridSettingService: GridSettingService,
        private authService: AuthenticationService,
        private mfsSettingService: MfsSettingService,
        private mfsUtilityService: MfsUtilityService,
        private messageService: MessageService,
        private customerAccountMappingService: CustomerAccountMappingService,
        private route: ActivatedRoute) {
        this.gridConfig = {};

        this.authService.currentUser.subscribe(x => {
            this.currentUserModel = x;
        });
    }
    ngOnInit() {
        this.initialiseGridConfig();
        this.isRegPermit = this.authService.checkRegisterPermissionAccess(this.route.snapshot.routeConfig.path);
        this.changeby = this.currentUserModel.user.username;
        this.ubranch = this.currentUserModel.user.branchCode;
    }

    getParameter(): any {
        if (this.mtCbsInfoModel.mphone) {
            const objectParameter = {
                objectModel: this.mtCbsInfoModel,
                ubranch: this.ubranch,
                changeby: this.changeby
            }
            return objectParameter;
        }
        else {
            const objectParameter = {
                objectModel: null,
                ubranch: this.ubranch,
                changeby: this.changeby
            }
            return objectParameter;
        }
    }
    checkAccountValidityByCount(): any {
        this.loadingCbsInfo = true;
        this.customerAccountMappingService.checkAccountValidityByCount(this.mblAcc)
            .pipe(first())
            .subscribe(
                data => {
                    this.loadingCbsInfo = false;
                    if (data.count <= 1) {
                        this.getNameByMphone();
                    } else {
                        this.mtCbsName = 'This account is not eligible';
                    }
                },
                error => {
                    console.log(error);
                });
    }

    checkPendingAccountByMphone(): any {
        this.loadingCbsInfo = true;
        this.customerAccountMappingService.checkPendingAccountByMphone(this.mblAcc)
            .pipe(first())
            .subscribe(
                data => {
                    this.loadingCbsInfo = false;
                    if (data === 0) {
                        this.getNameByMphone();
                    } else {
                        this.mtCbsInfoModel = {};
                        this.messageService.add({
                            severity: 'error',
                            summary: 'Pending Issue',
                            detail: 'This account has pending Issue Resolve that first',
                            closable: true
                        });
                    }
                },
                error => {
                    console.log(error);
                });
    }
    getNameByMphone() {
        this.loading = true;
        if (!this.gridConfig.paramForBatchUpdate) {
            this.gridConfig.paramForBatchUpdate = JSON.stringify(this.getParameter());
        }
        this.customerAccountMappingService.getNameByMphone(this.mblAcc)
            .pipe(first())
            .subscribe(
                data => {
                    if (data != 'PEXIST') {
                        this.mtCbsName = '';
                        this.loading = false;
                        this.mtCbsName = data.NAME;
                        this.getMappedAccountByMblNo();
                    }
                    else if (data === 'PEXIST') {
                        this.gridConfig.dataSourcePath = null;
                        this.messageService.add({
                            severity: 'error',
                            summary: 'Pending Issue',
                            detail: 'This account has pending Issue Resolve that first',
                            closable: true
                        });
                        this.loading = false;
                    }
                    else {
                        this.loading = false;
                        this.mtCbsName = 'No Data Found';
                    }
                },
                error => {
                    console.log(error);
                });
    }
    getMappedAccountByMblNo(): any {
        this.gridConfig.dataSourcePath = this.mfsSettingService.transactionApiServer +
            '/CbsMappedAccount/GetMappedAccountByMblNo?mblNo=' + this.mblAcc;
        if (this.childGrid) {
            this.childGrid.updateDataSource();
        }
    }


    clearData(): any {
        this.mtCbsInfoModel = {};
        this.mblAcc = null;
        this.isSearchDisable = false;
        this.mtCbsName = null;
        this.gridConfig.dataSourcePath = this.mtCbsInfoModel;
        if (this.childGrid) {
            this.childGrid.clearDataSource();
        }
    }

    checkIsAccountValid(): any {
        this.loadingCbsInfo = true;
        this.customerAccountMappingService.checkIsAccountValid(this.mblAcc, this.mtCbsInfoModel.accno)
            .pipe(first())
            .subscribe(
                data => {
                    if (data) {
                        this.loadingCbsInfo = false;
                        this.isAccountValid = data;
                        this.checkCbsValidClass();
                        this.loadingCbsInfo = false;
                    }
                    else {
                        this.messageService.add({
                            severity: 'error',
                            summary: 'Miss Matched Mobile',
                            detail: 'Ok Wallet acc no and CBS mobile No should be same',
                            closable: true
                        });
                    }
                },
                error => {
                    console.log(error);
                });
    }


    checkCbsValidClass(): any {
        this.loadingCbsInfo = true;
        this.class = this.mtCbsInfoModel.class;
        this.customerAccountMappingService.checkIfCbsClassValid(this.mtCbsInfoModel.class)
            .pipe(first())
            .subscribe(
                data => {
                    if (data) {
                        this.loadingCbsInfo = false;
                        this.isAccountValid = data;
                        this.onSearch();
                        this.loadingCbsInfo = false;
                    }
                    else {
                        this.messageService.add({
                            severity: 'error',
                            summary: 'Invalid Class',
                            detail: 'Invalid Class, Please enter valid class CBS account',
                            closable: true
                        });
                    }
                },
                error => {
                    console.log(error);
                });
    }

    onMap() {
        if (this.isAccountValid === 1) {
            this.gridConfig.dataSourcePath = this.mfsSettingService.transactionApiServer +
                '/CbsMappedAccount/GetCbsCustomerInfo?accNo=' + this.mtCbsInfoModel.accno + '&reqType=m';

            if (this.childGrid) {
                this.childGrid.updateDataSource();
            }
        } else {
            this.msgs = [];
            this.msgs.push({ severity: 'error', summary: 'Mobile No mismatched', detail: 'Ok wallet mobile and CBS mobile should be same', closable: true });
            this.showMsg = true;
        }

    }


    checkActivatdAccountByMphone(): any {
        this.loadingCbsInfo = true;
        this.customerAccountMappingService.checkActivatdAccountByMphone(this.mblAcc)
            .pipe(first())
            .subscribe(
                data => {
                    if (data >= 2) {
                        this.messageService.add({
                            severity: 'error',
                            summary: 'Already Mapped',
                            detail: 'This account is already mapped with two account.Fisrt inactive a account first',
                            closable: true
                        });
                        this.loadingCbsInfo = false;
                    } else {
                        this.checkAccNoIsMappedByMblNo();
                        this.loadingCbsInfo = false;
                    }
                },
                error => {
                    console.log(error);
                }
            );
    }


    checkAccNoIsMappedByMblNo() {
        this.loadingCbsInfo = true;
        this.customerAccountMappingService.checkAccNoIsMappedByMblNo(this.mblAcc, this.mtCbsInfoModel.accno)
            .pipe(first())
            .subscribe(
                data => {
                    if (data.count === 1) {
                        this.messageService.add({
                            severity: 'error',
                            summary: 'Already Mapped',
                            detail: 'This account is already mapped',
                            closable: true
                        });
                        this.loadingCbsInfo = false;
                    } else {
                        this.checkIsAccountValid();
                        this.loadingCbsInfo = false;
                    }
                },
                error => {
                    console.log(error);
                }
            );
    }
    disableCbsSearch() {
        if (this.mtCbsInfoModel.accno && this.mtCbsInfoModel.accno.length != 13) {
            this.isSearchDisable = true;
        }     
    }

    onCbsSearch() {
        this.loadingCbsInfo = true;
        this.customerAccountMappingService.onCbsSearch(this.mtCbsInfoModel.accno, this.mblAcc)
            .pipe(first())
            .subscribe(
                data => {
                    if (data === 'EACC') {
                        this.loadingCbsInfo = false;
                        this.messageService.add({
                            severity: 'error',
                            summary: 'Already Exist',
                            detail: 'This CBS account is already exist',
                            sticky: true
                        });
                    }
                    else if (data === 'PE') {
                        this.loadingCbsInfo = false;
                        this.messageService.add({
                            severity: 'warn',
                            summary: 'Pending Exist',
                            detail: 'Please resolved the pending issue',
                            sticky: true
                        });
                    }
                    else if (data === 'MCC') {
                        this.loadingCbsInfo = false;
                        this.messageService.add({
                            severity: 'error',
                            summary: 'Missmatched CBS class',
                            detail: 'Please select authorized cbs class account',
                            sticky: true
                        });
                    }
                    else if (data === 'MMCAMA') {
                        this.loadingCbsInfo = false;
                        this.messageService.add({
                            severity: 'error',
                            summary: 'Missmatched mobile number',
                            detail: 'CBS Account mobile no and ok wallet mobile number should be same',
                            sticky: true
                        });
                    }
                    else if (data === 'EACCM3') {
                        this.loadingCbsInfo = false;
                        this.messageService.add({
                            severity: 'error',
                            summary: 'Exceed limit for mapping',
                            detail: 'Please Inactive a existing account for remapping new account',
                            sticky: true
                        });
                    }
                    else if (data === 500) {
                        this.loadingCbsInfo = false;
                        this.isSearchDisable = false;                      
                        this.isMapDisable = true;
                        this.messageService.add({
                            severity: 'error',
                            summary: 'Not Found',
                            detail: 'No cbs accont found',
                            sticky: true
                        });
                    }

                    else {
                        this.loadingCbsInfo = false;
                        this.mtCbsInfoModel = data;
                        this.isMapDisable = false;

                        this.gridConfig.paramForBatchUpdate = JSON.stringify(this.getParameter());
                    }
                },
                error => {
                    console.log(error);
                });
    }



    onSearch() {
        this.loadingCbsInfo = true;
        if (this.isAccountValid === 1) {
            this.customerAccountMappingService.getCbsInfoByAccNo(this.mtCbsInfoModel.accno, 's')
                .pipe(first())
                .subscribe(
                    data => {
                        if (data) {
                            this.loadingCbsInfo = false;
                            this.mtCbsInfoModel = data;
                            this.isMapDisable = true;
                            this.gridConfig.paramForBatchUpdate = JSON.stringify(this.getParameter());
                            //if (this.childGrid) {
                            //    this.childGrid.updateDataSource();
                            //}
                        } else {
                            this.loadingCbsInfo = false;
                        }
                    },
                    error => {
                        console.log(error);
                    });
        } else {
            this.msgs = [];
            this.msgs.push({ severity: 'error', summary: 'Mobile No mismatched', detail: 'Ok wallet mobile and CBS mobile should be same', closable: true });
            this.showMsg = true;
            this.loadingCbsInfo = false;
        }
    }

    checkEligiblity() {
        if (!this.mtCbsInfoModel.isMapped) {
            if (this.childGrid) {
                this.childGrid.pushDataInDataSource(this.mtCbsInfoModel);
                this.mtCbsInfoModel.isMapped = true;
            }
        }
        this.isSearchDisable = true;
    }

    updateBatch(data: any) {
        this.customerAccountMappingService.SaveMapOrRemapCbsAccount(data)
            .pipe(first())
            .subscribe(
                data => {
                    if (data === 200) {
                        this.getMappedAccountByMblNo();
                        this.isMapDisable = true;
                        this.isSearchDisable = false;
                        this.messageService.add({
                            severity: 'success',
                            summary: 'Success',
                            detail: 'Action performed successfully',
                            closable: true
                        });
                        this.childGrid.updateDataSource();
                    }
                    else if (data.length != 3) {
                        this.messageService.add({
                            severity: 'error',
                            summary: 'Attention Please',
                            detail: data,
                            closable: true
                        });
                    }
                    else {
                        this.messageService.add({
                            severity: 'error',
                            summary: 'Internal Server error. please call administrator',
                            detail: data,
                            closable: true
                        });
                    }
                },
                error => {
                    console.log(error);
                }
            );
    }

    initialiseGridConfig(): any {
        this.gridConfig.dataSource = [];
        //this.gridConfig.batchUpdatePath = this.mfsSettingService.transactionApiServer + '/CbsMappedAccount/SaveMapOrRemapCbsAccount';

        this.gridConfig.dataSourcePath = this.mfsSettingService.transactionApiServer +
            '/CbsMappedAccount/GetCbsCustomerInfo?accNo=' +
            this.mtCbsInfoModel.accno;

        this.gridConfig.autoUpdateDataSource = false;
        this.gridConfig.autoIndexing = true;

        this.gridConfig.isBatchSwitchBoxEdit = true;
        //this.gridConfig.createStateUrl = 'customer-accounts-mapping/addoredit';
        this.gridConfig.gridName = "Customer A/C Mapping";
        this.gridConfig.gridIconClass = 'fas fa-thumbtack';

        this.gridConfig.hasEditState = true;
        this.gridConfig.entityField = 'mphone';
        this.gridConfig.showUniversalFilter = false;

        //this.gridConfig.paramForBatchUpdate = JSON.stringify(this.getParameter());

        this.gridConfig.columnList = [
            { field: 'mobnum', header: 'Mobile No', width: '10%' },
            { field: 'custid', header: 'Customer Id', width: '15%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'name', header: 'Customer Name', width: '20%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'accno', header: 'Account No', width: '25%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'branch', header: 'Branch Code', width: '10%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'class', header: 'class', width: '10%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'accstat', header: 'A/C Status', width: '10%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'frozen', header: 'Frozen', width: '10%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'dorm', header: 'Dorment ', width: '15%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'nationid', header: 'National Id', width: '20%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'checkStatus', header: 'Check Status', width: '13%', filter: this.gridSettingService.getDefaultFilterable(), template: this.gridSettingService.getCheckStatusTemplateForRowData() },
            { field: 'status', header: 'Current status', width: '13%', filter: this.gridSettingService.getDefaultFilterable(), template: this.gridSettingService.getStatusTemplateForRowData() },
            { field: 'make_status_dump', header: 'Status', width: '10%', isSwitchBoxColumn: true, filter: this.gridSettingService.getDefaultFilterable() }
        ];
    };



}
