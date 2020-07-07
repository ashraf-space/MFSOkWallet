import { Component, OnInit, Input } from '@angular/core';
import { GridSettingService } from '../../../../services/grid-setting.service';
import { AgentService } from '../../../../services/distribution';
import { MfsSettingService } from '../../../../services/mfs-setting.service';
import { AuthenticationService } from 'src/app/shared/_services';
@Component({
    selector: 'app-agent-list',
    templateUrl: './agent-list.component.html',
    styleUrls: ['./agent-list.component.css']
})
export class AgentListComponent implements OnInit {
    @Input() distributor: any;
    @Input() agents; any;
    gridConfig: any;
    currentUserModel: any = {};
    constructor(private agentService: AgentService,
        private gridSettingService: GridSettingService,
        private mfsSettingService: MfsSettingService,
        private authService: AuthenticationService) {
        this.gridConfig = {};
        this.authService.currentUser.subscribe(x => {
            this.currentUserModel = x;
        });
    }
    ngOnInit() {
        this.initialiseGridConfig();
    }
    ngOnChanges() {
        if (this.distributor|| this.agents) {
            this.initialiseGridConfig();
        }
    }
    initialiseGridConfig(): any {
        this.gridConfig.dataSource = [];
        this.gridConfig.gridName = 'Agent List';
        this.gridConfig.gridIconClass = 'fas fa-list';
        this.gridConfig.createStateUrl = 'agent/addoredit/';
        this.gridConfig.hasEditState = true;
        this.gridConfig.entityField = 'mphone';
        this.gridConfig.showUniversalFilter = false;
        this.gridConfig.detailsStateUrl = '/agent/details/';
        this.gridConfig.autoUpdateDataSource = true;
        this.gridConfig.columnList = [
            { field: 'mphone', header: 'Agent AC No', width: '10%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'name', header: 'Name', width: '20%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'distCode', header: 'Agent Code', width: '10%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'conMob', header: 'Contact No', width: '10%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'companyName', header: 'Company Name', width: '15%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'offAddr', header: 'Address', width: '15%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'mphone', header: 'Details', width: '7%', isDetailsColumn: true, filter: this.gridSettingService.getFilterableNone() }
        ];
        this.gridConfig.customFilterOptionPath = this.mfsSettingService.distributionApiServer + '/Kyc/GetFilteringListForDdl';
        if (this.distributor && this.agents) {
            this.gridConfig.showCaption = false;
            this.gridConfig.autoUpdateDataSource = false;
            this.gridConfig.dataSource = this.agents;
        }
        else if (this.distributor && !this.agents) {
            this.gridConfig.showCaption = false;
            this.gridConfig.dataSourcePath = this.mfsSettingService.distributionApiServer + '/Agent/GetAgentListByParent?code=' + this.distributor.mphone + '&CatId=A';
        }
        else {
            this.gridConfig.columnList.push({ field: 'mphone', header: 'Action', width: '10%', isEditColumn: true, filter: this.gridSettingService.getFilterableNone(), actionDisableParam: 'regStatus', disableValue: 'P' });
            //this.gridConfig.dataSourcePath = this.mfsSettingService.distributionApiServer + '/Kyc/GetRegInfoListByCatIdBranchCode?BranchCode=' + this.currentUserModel.user.branchCode + '&CatId=A';
            this.gridConfig.dataSourcePath = this.mfsSettingService.distributionApiServer + '/Kyc/GetRegInfoListByCatIdBranchCode?BranchCode=' + this.currentUserModel.user.branchCode + '&CatId=A' + '&filterId=';

        }

    };
}
