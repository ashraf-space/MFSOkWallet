import { Component, OnInit } from '@angular/core';
import { MfsSettingService } from 'src/app/services/mfs-setting.service';
import { GridSettingService } from 'src/app/services/grid-setting.service';
import { AuthenticationService } from 'src/app/shared/_services';

@Component({
    selector: 'app-merchant-config-list',
    templateUrl: './merchant-config-list.component.html',
    styleUrls: ['./merchant-config-list.component.css']
})
export class MerchantConfigListComponent implements OnInit {
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

        this.gridConfig.dataSourcePath = this.mfsSettingService.distributionApiServer + '/Merchant/GetAllMerchant';
        this.gridConfig.autoUpdateDataSource = true; // --- FOR AUTO BINDING THE DATA AFTER DATA RETRIVAL FROM THE API SERVER
        this.gridConfig.autoIndexing = true;

        this.gridConfig.gridName = "Merchant Config list";
        this.gridConfig.gridIconClass = 'fas fa-list';
        this.gridConfig.createStateUrl = '/merchant-config/addoredit/';
        this.gridConfig.hasEditState = true;
        this.gridConfig.entityField = 'mphone';
        this.gridConfig.hideCreateState = true;
        this.gridConfig.columnList = [
            { field: 'MPHONE', header: 'Merchant AC No', width: '10%' },
            { field: 'NAME', header: 'Name', width: '20%', filter: this.gridSettingService.getDefaultFilterable() },
           // { field: 'MAXTRANSAMT', header: 'Merchant Code', width: '10%', filter: this.gridSettingService.getDefaultFilterable() },
           // { field: 'MINTRANSAMT', header: 'Code', width: '10%', filter: this.gridSettingService.getDefaultFilterable() },
           // { field: 'CUSTOMERSERVICECHARGEPER', header: 'Code', width: '10%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'CONMOB', header: 'Contact No', width: '10%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'COMPANYNAME', header: 'Company Name', width: '15%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'OFFADDR', header: 'Address', width: '15%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'MPHONE', header: 'Action', width: '10%', isEditColumn: true, filter: this.gridSettingService.getFilterableNone(), actionDisableParam: 'regStatus', disableValue: 'P' }
            //{ field: 'mphone', header: 'Register', width: '10%', isRegisterColumn: true, filter: this.gridSettingService.getFilterableNone() }
        ];


    };

}
