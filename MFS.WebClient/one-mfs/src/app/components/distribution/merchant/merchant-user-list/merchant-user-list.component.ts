import { Component, OnInit } from '@angular/core';
import { MfsSettingService } from 'src/app/services/mfs-setting.service';
import { GridSettingService } from 'src/app/services/grid-setting.service';
import { AuthenticationService } from 'src/app/shared/_services';
@Component({
    selector: 'app-merchant-user-list',
    templateUrl: './merchant-user-list.component.html',
    styleUrls: ['./merchant-user-list.component.css']
})
export class MerchantUserListComponent implements OnInit {
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

        this.gridConfig.dataSourcePath = this.mfsSettingService.distributionApiServer + '/Merchant/GetMerchantUserList';
        this.gridConfig.autoUpdateDataSource = true; // --- FOR AUTO BINDING THE DATA AFTER DATA RETRIVAL FROM THE API SERVER
        this.gridConfig.autoIndexing = true;

        this.gridConfig.gridName = "Merchant User list";
        this.gridConfig.gridIconClass = 'fas fa-list';
        this.gridConfig.createStateUrl = '/merchant-user/addoredit/';
        this.gridConfig.hasEditState = true;
        this.gridConfig.entityField = 'ID';
        this.gridConfig.columnList = [          
            { field: 'MOBILE_NO', header: 'Merchant AC No', width: '40%' },
            { field: 'USERNAME', header: 'User Name ', width: '40%', filter: this.gridSettingService.getDefaultFilterable()},
            { field: 'COMPANY_NAME', header: 'Merchant Name', width: '40%', filter: this.gridSettingService.getDefaultFilterable() },
            //{ field: 'USER_ID', header: 'LogIn Id', width: '20%', filter: this.gridSettingService.getDefaultFilterable() },
            //{ field: 'STATUS', header: 'STATUS', width: '15%', filter: this.gridSettingService.getDefaultFilterable(), template: this.gridSettingService.getStatusTemplateForRowData() },
            { field: 'ID', header: 'Action', width: '10%', isEditColumn: true, filter: this.gridSettingService.getFilterableNone() }
        ];


    };
}
