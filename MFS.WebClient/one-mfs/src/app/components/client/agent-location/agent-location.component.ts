import { Component, OnInit, ViewChild } from '@angular/core';
import { OutboxService } from 'src/app/services/client/outbox.service';
import { GridSettingService } from 'src/app/services/grid-setting.service';
import { AuthenticationService } from 'src/app/shared/_services';
import { MfsSettingService } from 'src/app/services/mfs-setting.service';
import { MfsUtilityService } from 'src/app/services/mfs-utility.service';
import { GenericGridComponent } from 'src/app/shared/directives/generic-grid/generic-grid.component';
import { DistributorService, AgentService } from 'src/app/services/distribution';
import { first } from 'rxjs/operators';

@Component({
  selector: 'app-agent-location',
  templateUrl: './agent-location.component.html',
  styleUrls: ['./agent-location.component.css']
})
export class AgentLocationComponent implements OnInit {

    gridConfig: any;
    currentUserModel: any;
    @ViewChild(GenericGridComponent) child: GenericGridComponent;
    showGrid: boolean = false;
    model: any = {};

    regionList: any = [];
    areaList: any = [];
    territoryList: any = [];
    clusterList: any = [];

    dateObj: any = {};
    isEditMode: boolean = false;

    constructor(private outboxService: OutboxService, private gridSettingService: GridSettingService, private authService: AuthenticationService
        , private mfsSettingService: MfsSettingService, private mfsUtilityService: MfsUtilityService, private distributionService: DistributorService
        , private agentService: AgentService) {
        this.gridConfig = {};

        this.authService.currentUser.subscribe(x => {
            this.currentUserModel = x;
        });
    }

    ngOnInit() {
        this.getRegionListForDDL();
        this.initialiseGridConfig();
    }

    onSearch() {        
        this.showGrid = true;

        if (this.model.cluster && this.model.cluster!='') {
            this.model.searchOption = this.model.cluster;
        }

        this.gridConfig.dataSourcePath = this.mfsSettingService.distributionApiServer + '/Agent/GetAgentListByClusterCode?cluster=' + this.model.searchOption;

        if (this.child) {
            this.child.updateDataSource();
        }
        
    }

    initialiseGridConfig(): any {
        this.gridConfig.dataSource = [];

        this.gridConfig.dataSourcePath = this.mfsSettingService.distributionApiServer + '/Agent/GetAgentListByClusterCode';
        this.gridConfig.autoUpdateDataSource = true;
        this.gridConfig.autoIndexing = true;

        this.gridConfig.gridName = "Agent Location";
        this.gridConfig.gridIconClass = 'fas fa-thumbtack';

        this.gridConfig.entityField = 'mphone';
        this.gridConfig.hasCustomContent = false;
        this.gridConfig.showUniversalFilter = true;

        this.gridConfig.detailsStateUrl = '/agent/details/';

        this.gridConfig.columnList = [
            { field: 'mphone', header: 'Agent AC No', width: '10%', filter: this.gridSettingService.getDefaultFilterable()},            
            { field: 'companyName', header: 'Company Name', width: '15%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'offAddr', header: 'Business Address', width: '25%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'perAddr', header: 'Permanent Address', width: '25%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'mphone', header: 'Details', width: '7%', isDetailsColumn: true, filter: this.gridSettingService.getFilterableNone() }
        ];        
    };

    onResend(event) {
        console.log(event);
    }

    async getRegionListForDDL() {
        this.distributionService.getRegionList()
            .pipe(first())
            .subscribe(
                data => {
                    this.regionList = data;
                },
                error => {
                    console.log(error);
                }
            );
    }

    fillAreaDDL() {
        this.distributionService.getAreaListByRegion(this.model.region)
            .pipe(first())
            .subscribe(
                data => {
                    this.areaList = data;
                },
                error => {
                    console.log(error);
                }
        );
        this.model.searchOption = this.model.region;
    }

    fillTerritoryDDL() {
        this.distributionService.getTerritoryListByArea(this.model.area)
            .pipe(first())
            .subscribe(
                data => {
                    this.territoryList = data;
                },
                error => {
                    console.log(error);
                }
        );
        this.model.searchOption = this.model.area;
    }

    getclusterByTerritoryCode() {
        this.agentService.GetclusterByTerritoryCode(this.model.territory)
            .pipe(first())
            .subscribe(
                data => {
                    this.clusterList = data;
                },
                error => {
                    console.log(error);
                }
        );
        this.model.searchOption = this.model.territory;
    }

}
