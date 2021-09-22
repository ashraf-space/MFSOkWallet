import { Component, OnInit } from '@angular/core';
import { DistributorService } from 'src/app/services/distribution';
import { GridSettingService } from 'src/app/services/grid-setting.service';
import { AuthenticationService } from 'src/app/shared/_services';
import { MfsSettingService } from 'src/app/services/mfs-setting.service';

@Component({
  selector: 'app-b2b-retail-list',
  templateUrl: './b2b-retail-list.component.html',
  styleUrls: ['./b2b-retail-list.component.css']
})
export class B2bRetailListComponent implements OnInit {

    gridConfig: any;
    ProgressSpinnerDlg: boolean = false;
    currentUserModel: any = {};

    constructor(private distributorService: DistributorService, private gridSettingService: GridSettingService, private authService: AuthenticationService
        , private mfsSettingService: MfsSettingService) {
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
        this.gridConfig.customFilterOptionPath = this.mfsSettingService.distributionApiServer + '/Kyc/GetFilteringListForDdl';
        this.gridConfig.dataSourcePath = this.mfsSettingService.distributionApiServer + '/Kyc/GetRegInfoListByCatIdBranchCode?BranchCode=' + this.currentUserModel.user.branchCode + '&CatId=BR' + '&filterId=';
        //this.gridConfig.dataSourcePath = this.mfsSettingService.distributionApiServer + '/Kyc/GetRegInfoListByCatIdBranchCode?BranchCode=' + this.currentUserModel.user.branchCode+'&CatId=D';
        this.gridConfig.autoUpdateDataSource = true; // --- FOR AUTO BINDING THE DATA AFTER DATA RETRIVAL FROM THE API SERVER
        this.gridConfig.autoIndexing = true;

        this.gridConfig.gridName = "B2B Retail list";
        this.gridConfig.gridIconClass = 'fas fa-list';
        this.gridConfig.createStateUrl = '/b2bretail/addoredit/';
        this.gridConfig.hasEditState = true;
        this.gridConfig.entityField = 'mphone';
        this.gridConfig.showUniversalFilter = false;
        this.gridConfig.detailsStateUrl = 'b2bretail/details/';

        this.gridConfig.columnList = [
            { field: 'mphone', header: 'B2B Retail AC No', width: '10%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'name', header: 'Name', width: '20%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'distCode', header: 'Distributor Code', width: '10%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'conMob', header: 'Contact No', width: '10%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'companyName', header: 'Company Name', width: '15%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'offAddr', header: 'Address', width: '15%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'mphone', header: 'Details', width: '7%', isDetailsColumn: true, filter: this.gridSettingService.getFilterableNone() },
            //{ field: 'mphone', header: 'Action', width: '10%', isEditColumn: true, filter: this.gridSettingService.getFilterableNone(), actionDisableParam: 'regStatus', disableValue: 'P' }
            { field: 'mphone', header: 'Edit', width: '10%', isEditColumn: true, filter: this.gridSettingService.getFilterableNone() }
            //{ field: 'mphone', header: 'Register', width: '10%', isRegisterColumn: true, filter: this.gridSettingService.getFilterableNone() }
        ];

    };

}
