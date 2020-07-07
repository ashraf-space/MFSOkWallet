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
    selector: 'app-customer-accounts-mapping',
    templateUrl: './customer-accounts-mapping.component.html',
    styleUrls: ['./customer-accounts-mapping.component.css']
})
export class CustomerAccountsMappingComponent implements OnInit {
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
    msgs: Message[] = [];
    showMsg: boolean = false;
    validMblNo: boolean = true;

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
                            detail: 'This account is already mapped with this mobile no',
                            closable: true
                        });
                        this.loadingCbsInfo = false;
                    } else {
                        this.onSearch();
                        this.loadingCbsInfo = false;
                    }
                },
                error => {
                    console.log(error);
                }
            );
    }
    getParameter(): any {
        const objectParameter = {
            objectModel: this.mtCbsInfoModel,
            changeby: this.changeby
        }
        return objectParameter;
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
                        this.mtCbsInfoModel = null;
                    }
                },
                error => {
                    console.log(error);
                });
    }
    getNameByMphone() {
        this.loading = true;
        this.customerAccountMappingService.getNameByMphone(this.mblAcc)
            .pipe(first())
            .subscribe(
                data => {
                    if (data) {
                        this.validMblNo = false;
                        this.mtCbsName = '';
                        this.loading = false;
                        this.mtCbsName = data.NAME;
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
    checkIsAccountValid(): any {
        console.log(this.mblAcc);
        this.loadingCbsInfo = true;
        this.customerAccountMappingService.checkIsAccountValid(this.mblAcc, this.mtCbsInfoModel.accno)
            .pipe(first())
            .subscribe(
                data => {
                    this.loadingCbsInfo = false;
                    this.isAccountValid = data;
                    this.onMap();
                },
                error => {
                    console.log(error);
                });
    }
    onMap() {
        if (this.isAccountValid === 1) {
            this.gridConfig.dataSourcePath = this.mfsSettingService.transactionApiServer +
                '/CbsMappedAccount/GetCbsCustomerInfo?accNo=' +
                this.mtCbsInfoModel.accno +
                '&reqType=m';
            this.gridConfig.paramForBatchUpdate = JSON.stringify(this.getParameter());

            if (this.childGrid) {
                this.childGrid.updateDataSource();
            }
        } else {
            this.msgs = [];
            this.msgs.push({ severity: 'error', summary: 'Mobile No mismatched', detail: 'Ok wallet mobile and CBS mobile should be same', closable: true });
            this.showMsg = true;
            this.validMblNo = false;
        }

    }

    onSearch() {
        this.loadingCbsInfo = true;
        this.customerAccountMappingService.getCbsInfoByAccNo(this.mtCbsInfoModel.accno, 's')
            .pipe(first())
            .subscribe(
                data => {
                    if (data) {
                        this.loadingCbsInfo = false;
                        this.mtCbsInfoModel = data;
                    } else {
                        this.loadingCbsInfo = false;
                    }
                },
                error => {
                    console.log(error);
                });
    }


    initialiseGridConfig(): any {
        this.gridConfig.dataSource = [];
        this.gridConfig.batchUpdatePath = this.mfsSettingService.transactionApiServer + '/CbsMappedAccount/SaveMatchedCbsAccount';
        this.gridConfig.dataSourcePath = this.mfsSettingService.transactionApiServer +
            '/CbsMappedAccount/GetCbsCustomerInfo?accNo=' +
            this.mtCbsInfoModel.accno;

        this.gridConfig.autoUpdateDataSource = false;
        this.gridConfig.autoIndexing = false;

        this.gridConfig.isBatchSwitchBoxEdit = true;
        //this.gridConfig.createStateUrl = 'customer-accounts-mapping/addoredit';
        this.gridConfig.gridName = "Customer A/C Mapping";
        this.gridConfig.gridIconClass = 'fas fa-thumbtack';

        this.gridConfig.hasEditState = true;
        this.gridConfig.entityField = 'mphone';
        this.gridConfig.showUniversalFilter = false;

        this.gridConfig.paramForBatchUpdate = this.currentUserModel.user.username;

        this.gridConfig.columnList = [
            { field: 'mobnum', header: 'Mobile No', width: '10%' },
            { field: 'custid', header: 'Customer Id', width: '15%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'name', header: 'Customer Name', width: '20%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'accno', header: 'Account No', width: '20%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'branch', header: 'Branch Code', width: '10%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'class', header: 'class', width: '10%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'accstat', header: 'A/C Status', width: '10%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'frozen', header: 'Frozen', width: '10%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'dorm', header: 'Dorment ', width: '15%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'nationid', header: 'National Id', width: '20%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'status', header: 'Current status', width: '13%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'makestatus', header: 'Status', width: '10%', isSwitchBoxColumn: true, filter: this.gridSettingService.getDefaultFilterable() }
        ];
    };
}
