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

@Component({
    selector: 'app-cbs-mapping-check',
    templateUrl: './cbs-mapping-check.component.html',
    styleUrls: ['./cbs-mapping-check.component.css']
})
export class CbsMappingCheckComponent implements OnInit {
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
    optionList = [
    { label: '', value: 'Y', icon: 'fas fa-check' },
    { label: '', value: 'N', icon: 'fas fa-times' },
    { label: '', value: 'P', icon: 'far fa-clock' }];

@ViewChild(GenericGridComponent) childGrid: GenericGridComponent;
constructor(private gridSettingService: GridSettingService,
    private authService: AuthenticationService,
    private mfsSettingService: MfsSettingService,
    private mfsUtilityService: MfsUtilityService,
    private messageService: MessageService,
    private customerAccountMappingService: CustomerAccountMappingService) {
    this.gridConfig = {};

    this.authService.currentUser.subscribe(x => {
        this.currentUserModel = x;
    });
}

ngOnInit() {
    this.initialiseGridConfig();
}
initialiseGridConfig(): any {
    this.gridConfig.dataSource = [];
    this.gridConfig.batchUpdatePath = this.mfsSettingService.transactionApiServer + '/CbsMappedAccount/SaveActionPendingCbsAccounts';

    this.gridConfig.dataSourcePath = this.mfsSettingService.transactionApiServer + '/CbsMappedAccount/GetPendingCbsAccounts?branchCode=' + this.currentUserModel.user.branchCode;

    this.gridConfig.autoUpdateDataSource = true;
    this.gridConfig.autoIndexing = false;
    this.gridConfig.paramForBatchUpdate = this.currentUserModel.user.username;
    this.gridConfig.isBatchSwitchBoxEdit = true;
    //this.gridConfig.createStateUrl = 'customer-accounts-mapping/addoredit';
    this.gridConfig.gridName = "Customer A/C Mapping Check";
    this.gridConfig.gridIconClass = 'fas fa-thumbtack';

    this.gridConfig.hasEditState = true;
    this.gridConfig.entityField = 'mphone';
    this.gridConfig.showUniversalFilter = false;

    this.gridConfig.paramForBatchUpdate = this.currentUserModel.user.username;

    this.gridConfig.columnList = [
        { field: 'MPHONE', header: 'Mobile', width: '14%' },
        { field: 'CUSTID', header: 'Cust Id', width: '13%', filter: this.gridSettingService.getDefaultFilterable() },
        { field: 'NAME', header: 'Name', width: '16%', filter: this.gridSettingService.getDefaultFilterable() },
        { field: 'ACCNO', header: 'A/C No', width: '16%', filter: this.gridSettingService.getDefaultFilterable() },
        { field: 'BRANCH', header: 'Branch', width: '8%', filter: this.gridSettingService.getDefaultFilterable() },
        { field: 'CLASS', header: 'class', width: '8%', filter: this.gridSettingService.getDefaultFilterable() },
        { field: 'ACCSTAT', header: 'A/C Status', width: '8%', filter: this.gridSettingService.getDefaultFilterable() },
        { field: 'FROZEN', header: 'Frozen', width: '8%', filter: this.gridSettingService.getDefaultFilterable() },
        { field: 'DORM', header: 'Dorment ', width: '10%', filter: this.gridSettingService.getDefaultFilterable() },
        { field: 'NATIONID', header: 'NID No', width: '12%', filter: this.gridSettingService.getDefaultFilterable() },
        { field: 'STATUS', header: 'Current Status', width: '10%', filter: this.gridSettingService.getDefaultFilterable() },
        { field: 'MAKE_STATUS', header: 'Requested Status', width: '12%', filter: this.gridSettingService.getDefaultFilterable() },
        { field: 'ENTRY_BY', header: 'Maker Info', width: '10%', filter: this.gridSettingService.getDefaultFilterable() },
        { field: 'CHECK_STATUS', header: 'Action', width: '18%', isSelectButtonColumn: true, filter: this.gridSettingService.getFilterableNone(), optionList: this.optionList }

    ];
};
}
