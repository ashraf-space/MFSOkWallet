import { Component, OnInit } from '@angular/core';
import { DistributorService, AgentService } from 'src/app/services/distribution';
import { first } from 'rxjs/operators';
import { GridSettingService } from 'src/app/services/grid-setting.service';
import { AuthenticationService } from 'src/app/shared/_services';
import { MfsSettingService } from 'src/app/services/mfs-setting.service';
import { GenericGridComponent } from 'src/app/shared/directives/generic-grid/generic-grid.component';
import { MessageService } from 'primeng/api';

@Component({
    selector: 'app-agent-replace',
    templateUrl: './agent-replace.component.html',
    styleUrls: ['./agent-replace.component.css']
})
export class AgentReplaceComponent implements OnInit {
    agentReplaceModel: any = {};
    selectedAgentReplaceModel: any = {};
    distributorList: any = {};
    isLoading: boolean = false;
    mobileNo: string = null;
    exSelectedTerritory: string = "0";
    newSelectedTerritory: string = "0";
    selectedTerritory: string = "0";
    selectedCluster: string = "0";
    exClusterList: any = {};
    newClusterList: any = {};
    isActionDisable: boolean = true;
    exGridConfig: any;
    newGridConfig: any;
    currentUserModel: any = {};
    error: boolean = false;
    AgentPhoneCodeModel: any = {};
    entryBy: any;

    constructor(private distributorService: DistributorService, private agentService: AgentService
        , private gridSettingService: GridSettingService, private authService: AuthenticationService,
        private mfsSettingService: MfsSettingService, private messageService: MessageService) {
        this.exGridConfig = {};
        this.newGridConfig = {};

        this.authService.currentUser.subscribe(x => {
            this.currentUserModel = x;
        });
    }

    ngOnInit() {
        this.getDistributorList();
        this.loadExInitialiseGrid();
        this.loadNewInitialiseGrid();
    }
    getDistributorList(): any {
        this.isLoading = true;
        this.distributorService.getDistributorListForDDL()
            .pipe(first())
            .subscribe(
                data => {
                    this.isLoading = false;
                    this.distributorList = data;
                },
                error => {
                    this.isLoading = false;
                    console.log(error);
                }
            );
    }

    getRegionDetailsByMobileNo(exOrNew): any {

        if (exOrNew == 'Ex')
            this.mobileNo = this.agentReplaceModel.exMobileNo;
        else
            this.mobileNo = this.agentReplaceModel.newMobileNo;
        this.isLoading = true
        this.distributorService.getRegionDetailsByMobileNo(this.mobileNo)
            .pipe(first())
            .subscribe(
                data => {
                    this.isLoading = false;
                    if (data['DISTCODE'] != null) {
                        if (exOrNew == 'Ex') {
                            this.agentReplaceModel.exDistCode = data['DISTCODE'];
                            this.agentReplaceModel.exRegion = data['REGION'];
                            this.agentReplaceModel.exArea = data['AREA'];
                            this.agentReplaceModel.exTerritory = data['TERRITORY'];

                            this.exSelectedTerritory = this.agentReplaceModel.exDistCode.substring(0, 6);
                            this.GetClusterByTerritoryCode(exOrNew);
                        }

                        else {
                            this.agentReplaceModel.newDistCode = data['DISTCODE'];
                            this.agentReplaceModel.newRegion = data['REGION'];
                            this.agentReplaceModel.newArea = data['AREA'];
                            this.agentReplaceModel.newTerritory = data['TERRITORY'];

                            this.newSelectedTerritory = this.agentReplaceModel.newDistCode.substring(0, 6);
                            this.GetClusterByTerritoryCode(exOrNew);
                        }
                    }
                    else {
                        if (exOrNew == 'Ex') {
                            this.agentReplaceModel.exDistCode = null;
                            this.agentReplaceModel.exRegion = null;
                            this.agentReplaceModel.exArea = null;
                            this.agentReplaceModel.exTerritory = null;

                            this.exSelectedTerritory = null;
                            //this.GetClusterByTerritoryCode(exOrNew);
                            this.exClusterList = null;
                            this.exGridConfig.dataSource = [];
                        }

                        else {
                            this.agentReplaceModel.newDistCode = null;
                            this.agentReplaceModel.newRegion = null;
                            this.agentReplaceModel.newArea = null;
                            this.agentReplaceModel.newTerritory = null;

                            this.newSelectedTerritory = null;
                            //this.GetClusterByTerritoryCode(exOrNew);
                            this.newClusterList = null;
                            this.newGridConfig.dataSource = [];
                        }
                    }
                    
                },
                error => {
                    this.isLoading = false;
                    console.log(error);
                }
            );

    }

    GetClusterByTerritoryCode(exOrNew: string) {
        if (exOrNew == 'Ex')
            this.selectedTerritory = this.exSelectedTerritory;
        else
            this.selectedTerritory = this.newSelectedTerritory;

        this.agentService.GetclusterByTerritoryCode(this.selectedTerritory)
            .pipe(first())
            .subscribe(
                data => {

                    if (exOrNew == 'Ex') {
                        this.exClusterList = data;
                        this.exClusterList.unshift({ label: 'Select Cluster', value: null });
                    }

                    else {
                        this.newClusterList = data;
                        this.newClusterList.unshift({ label: 'Select Cluster', value: null });
                    }
                        
                },
                error => {
                    console.log(error);
                }
            );
    }

    loadExInitialiseGrid(): any {
        this.exGridConfig.dataSource = [];

        this.exGridConfig.autoUpdateDataSource = true;
        this.exGridConfig.autoIndexing = true;
        this.exGridConfig.isBatchSwitchBoxEdit = true;

        //this.exGridConfig.gridName = "From Agent";
        //this.exGridConfig.gridIconClass = 'fas fa-thumbtack';

        //this.exGridConfig.hasEditState = true;
        this.exGridConfig.showUniversalFilter = false;


        this.exGridConfig.columnList = [
            { field: 'agentPhone', header: 'Agent Phone', width: '40%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'agentCode', header: 'Agent Code', width: '30%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'makeStatus', header: 'Action', width: '10%', isSwitchBoxColumn: true, filter: this.gridSettingService.getFilterableNoneAndToggleSelectAll() }
        ];
    }

    loadNewInitialiseGrid(): any {
        this.newGridConfig.dataSource = [];

        this.newGridConfig.autoUpdateDataSource = true;
        this.newGridConfig.autoIndexing = true;

        //this.exGridConfig.gridName = "From Agent";
        //this.exGridConfig.gridIconClass = 'fas fa-thumbtack';

        this.newGridConfig.hasEditState = true;
        this.newGridConfig.showUniversalFilter = false;


        this.newGridConfig.columnList = [
            { field: 'agentPhone', header: 'Agent Phone', width: '40%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'agentCode', header: 'Agent Code', width: '30%', filter: this.gridSettingService.getDefaultFilterable() },

        ];
    }

    getAgentListByCluster(exOrNew: string): any {
        if (exOrNew == 'Ex')
            this.selectedCluster = this.agentReplaceModel.exCluster;
        else
            this.selectedCluster = this.agentReplaceModel.newCluster;
        if (this.selectedCluster != null && this.selectedCluster != '0' && this.selectedCluster.length > 0) {
            this.isLoading = true;
            this.agentService.GetAgentPhoneCodeListByCluster(this.selectedCluster)
                .pipe(first())
                .subscribe(
                    data => {
                        this.isLoading = false;
                        if (data.length > 0) {
                            if (exOrNew == 'Ex') {
                                this.exGridConfig.dataSource = data;
                                this.AgentPhoneCodeModel = data;
                            }
                            else
                                this.newGridConfig.dataSource = data;

                            //enable save button
                            if (this.agentReplaceModel.exCluster && this.agentReplaceModel.exCluster != '0' && this.agentReplaceModel.exCluster != ''
                                && this.agentReplaceModel.newCluster && this.agentReplaceModel.newCluster != '0' && this.agentReplaceModel.newCluster != '') {
                                this.isActionDisable = false;
                            }
                            else {
                                this.isActionDisable = true;
                            }
                        }
                        else {
                            if (exOrNew == 'Ex') {
                                this.exGridConfig.dataSource = [];
                                this.AgentPhoneCodeModel = null;
                            }
                            else
                                this.newGridConfig.dataSource = [];
                        }

                    },
                    error => {
                        this.isLoading = false;
                        this.isActionDisable = true;
                        console.log(error);
                    }
                );


        }
        else {
            if (exOrNew == 'Ex') {
                this.exGridConfig.dataSource = [];
                this.AgentPhoneCodeModel = null;
            }
            else
                this.newGridConfig.dataSource = [];
        }

    }

    ExecuteAgentReplace(): any {
        this.entryBy = this.currentUserModel.user.username;

        this.selectedAgentReplaceModel = this.AgentPhoneCodeModel.filter(it => {
            return it.makeStatus == true;
        });


        if (this.selectedAgentReplaceModel.length > 0) {
            this.isLoading = true;
            this.agentService.ExecuteAgentReplace(this.agentReplaceModel.exMobileNo, this.agentReplaceModel.newMobileNo, this.agentReplaceModel.exCluster, this.agentReplaceModel.newCluster, this.entryBy, this.selectedAgentReplaceModel).pipe(first())
                .subscribe(
                    data => {
                        this.isLoading = false;
                        if (data == 1) {
                            this.messageService.add({ severity: 'success', summary: 'Replace successfully', detail: 'Agent replacement done' });
                        }
                        else {
                            this.messageService.add({ severity: 'error', summary: 'Not replacement', detail: data });
                        }

                        setTimeout(() => {
                            this.isLoading = false;
                            location.reload();
                        }, 5000);
                    },
                    error => {
                        console.log(error);
                    });

        }
        else {
            this.messageService.add({ severity: 'error', summary: 'No data is selected' });
        }
    }


}
