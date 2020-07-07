import { Component, OnInit } from '@angular/core';
import { MfsSettingService } from 'src/app/services/mfs-setting.service';
import { GridSettingService } from 'src/app/services/grid-setting.service';
import { AuthenticationService } from 'src/app/shared/_services';

@Component({
    selector: 'app-chain-merchant-list',
    templateUrl: './chain-merchant-list.component.html',
    styleUrls: ['./chain-merchant-list.component.css']
})
export class ChainMerchantListComponent implements OnInit {
    gridConfig: any;
    currentUserModel: any = {};
    constructor(private mfsSettingService: MfsSettingService, private gridSettingService: GridSettingService,
        private authService: AuthenticationService) {
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

        this.gridConfig.dataSourcePath = this.mfsSettingService.distributionApiServer + '/Kyc/GetChainMerchantList?filterId=';
        this.gridConfig.customFilterOptionPath = this.mfsSettingService.distributionApiServer + '/Kyc/GetFilteringListForDdl';
        this.gridConfig.autoUpdateDataSource = true; // --- FOR AUTO BINDING THE DATA AFTER DATA RETRIVAL FROM THE API SERVER
        this.gridConfig.autoIndexing = true;
        this.gridConfig.detailsStateUrl = 'chain-merchant/details/';
        this.gridConfig.gridName = "Chain Merchant list";
        this.gridConfig.gridIconClass = 'fas fa-list';
        this.gridConfig.createStateUrl = '/chain-merchant/addoredit/';
        this.gridConfig.hasEditState = true;
        this.gridConfig.entityField = 'mphone';
        this.gridConfig.showUniversalFilter = false;
        this.gridConfig.columnList = [
            { field: 'mphone', header: 'Merchant AC No', width: '10%', filter: this.gridSettingService.getDefaultFilterable() },
            //{ field: 'company_name', header: 'Name', width: '30%', filter: this.gridSettingService.getDefaultFilterable() }, 
            { field: 'name', header: 'Name', width: '20%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'conMob', header: 'Contact No', width: '10%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'companyName', header: 'Company Name', width: '15%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'offAddr', header: 'Address', width: '15%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'mphone', header: 'Details', width: '7%', isDetailsColumn: true, filter: this.gridSettingService.getFilterableNone() },
            { field: 'mphone', header: 'Action', width: '10%', isEditColumn: true, filter: this.gridSettingService.getFilterableNone(), actionDisableParam: 'regStatus', disableValue: 'P' }
            //{ field: 'mphone', header: 'Register', width: '10%', isRegisterColumn: true, filter: this.gridSettingService.getFilterableNone() }
        ];


    };
}
