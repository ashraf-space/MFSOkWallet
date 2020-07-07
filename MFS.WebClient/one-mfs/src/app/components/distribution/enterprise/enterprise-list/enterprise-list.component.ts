import { Component, OnInit } from '@angular/core';
import { GridSettingService } from '../../../../services/grid-setting.service';
import { CustomerService } from '../../../../services/distribution/customer.service';
import { AgentService } from '../../../../services/distribution';
import { MfsSettingService } from '../../../../services/mfs-setting.service';
import { AuthenticationService } from 'src/app/shared/_services';

@Component({
  selector: 'app-enterprise-list',
  templateUrl: './enterprise-list.component.html',
  styleUrls: ['./enterprise-list.component.css']
})
export class EnterpriseListComponent implements OnInit {

    gridConfig: any;
    currentUserModel: any = {};
    searchOptionType: any;
    constructor(private customerService: CustomerService,
        private gridSettingService: GridSettingService,
        private agentService: AgentService,
        private mfsSettingService: MfsSettingService,
        private authService: AuthenticationService) {
        this.gridConfig = {};

        this.authService.currentUser.subscribe(x => {
            this.currentUserModel = x;
        });
    }

    ngOnInit() {
        this.initialiseGridConfig();
        this.searchOptionType = [
            { label: 'All', value: 'A' },
            { label: 'Pending', value: 'P' }
        ];
    }
    initialiseGridConfig(): any {
        this.gridConfig.dataSource = [];
        this.gridConfig.gridName = 'Enterprise List';
        this.gridConfig.gridIconClass = 'fas fa-list';
        this.gridConfig.createStateUrl = 'enterprise/addoredit/';
        this.gridConfig.hasEditState = true;
        this.gridConfig.entityField = 'mphone';
        this.gridConfig.customFilterOptionPath = this.mfsSettingService.distributionApiServer + '/Kyc/GetFilteringListForDdl';
        this.gridConfig.dataSourcePath = this.mfsSettingService.distributionApiServer + '/Kyc/GetRegInfoListByCatIdBranchCode?BranchCode=' + this.currentUserModel.user.branchCode + '&CatId=E' + '&filterId=';
        this.gridConfig.autoUpdateDataSource = true;
        this.gridConfig.showUniversalFilter = false;
        this.gridConfig.detailsStateUrl = 'enterprise/details/';

        this.gridConfig.columnList = [
            { field: 'mphone', header: 'Enterprise AC No', width: '10%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'name', header: 'Name', width: '20%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'mphone', header: 'Details', width: '7%', isDetailsColumn: true, filter: this.gridSettingService.getFilterableNone() },
            { field: 'mphone', header: 'Action', width: '10%', isEditColumn: true, filter: this.gridSettingService.getFilterableNone(), actionDisableParam: 'regStatus', disableValue: 'P' }
        ];
    };

}
