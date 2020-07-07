import { Component, OnInit, Input } from '@angular/core';
import { DsrService, DistributorService } from 'src/app/services/distribution';
import { GridSettingService } from 'src/app/services/grid-setting.service';
import { MfsSettingService } from 'src/app/services/mfs-setting.service';
import { AuthenticationService } from 'src/app/shared/_services';

@Component({
    selector: 'app-dsr-list',
    templateUrl: './dsr-list.component.html',
    styleUrls: ['./dsr-list.component.css']
})
export class DsrListComponent implements OnInit {
    gridConfig: any;
    currentUserModel: any = {};

    @Input() distributor: any;
    @Input() dsr: any;
    constructor(private dsrService: DsrService, private gridSettingService: GridSettingService, private distributorService: DistributorService,
        private mfsSettingService: MfsSettingService, private authService: AuthenticationService) {
        this.gridConfig = {};
        this.authService.currentUser.subscribe(x => {
            this.currentUserModel = x;
        });
    }

    ngOnInit() {
        this.initialiseGridConfig();
    }
    ngOnChanges() {
        if (this.distributor || this.dsr) {
            this.initialiseGridConfig();
        }
    }
    initialiseGridConfig(): any {
        this.gridConfig.dataSource = [];

        //this.gridConfig.dataSourcePath = this.mfsSettingService.distributionApiServer + '/Dsr/GetDsrListData';
        //this.gridConfig.dataSourcePath = this.mfsSettingService.distributionApiServer + '/Distributor/GetRegInfoListByCatIdBranchCode?BranchCode=' + this.currentUserModel.user.branchCode + '&CatId=R';
        this.gridConfig.customFilterOptionPath = this.mfsSettingService.distributionApiServer + '/Kyc/GetFilteringListForDdl';
        
        this.gridConfig.autoUpdateDataSource = true; // --- FOR AUTO BINDING THE DATA AFTER DATA RETRIVAL FROM THE API SERVER
        this.gridConfig.autoIndexing = true;

        this.gridConfig.gridName = "DSR list";
        this.gridConfig.gridIconClass = 'fas fa-list';
        this.gridConfig.createStateUrl = '/dsr/addoredit/';
        this.gridConfig.hasEditState = true;
        this.gridConfig.entityField = 'mphone';
        this.gridConfig.showUniversalFilter = false;
        this.gridConfig.detailsStateUrl = 'dsr/details/';

        this.gridConfig.columnList = [
            { field: 'mphone', header: 'DSR AC No', width: '10%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'name', header: 'Name', width: '20%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'distCode', header: 'DSR Code', width: '10%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'conMob', header: 'Contact No', width: '10%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'companyName', header: 'Company Name', width: '15%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'offAddr', header: 'Address', width: '15%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'mphone', header: 'Details', width: '7%', isDetailsColumn: true, filter: this.gridSettingService.getFilterableNone() }
        ];
        if (this.distributor && this.dsr) {
            this.gridConfig.showCaption = false;
            this.gridConfig.autoUpdateDataSource = false;
            this.gridConfig.dataSource = this.dsr;
        }
        else if (this.distributor && !this.dsr) {           
            this.gridConfig.showCaption = false;            
            this.gridConfig.dataSourcePath = this.mfsSettingService.distributionApiServer + '/Agent/GetAgentListByParent?code=' + this.distributor.mphone + '&CatId=R';
        }
        else {
            this.gridConfig.columnList.push({ field: 'mphone', header: 'Action', width: '10%', isEditColumn: true, filter: this.gridSettingService.getFilterableNone(), actionDisableParam: 'regStatus', disableValue: 'P' });
            //this.gridConfig.dataSourcePath = this.mfsSettingService.distributionApiServer + '/Kyc/GetRegInfoListByCatIdBranchCode?BranchCode=' + this.currentUserModel.user.branchCode + '&CatId=R';
            this.gridConfig.dataSourcePath = this.mfsSettingService.distributionApiServer + '/Kyc/GetRegInfoListByCatIdBranchCode?BranchCode=' + this.currentUserModel.user.branchCode + '&CatId=R' + '&filterId=';

        }
    };
}
