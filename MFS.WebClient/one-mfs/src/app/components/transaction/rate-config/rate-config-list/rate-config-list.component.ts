import { Component, OnInit } from '@angular/core';
import { GridSettingService } from 'src/app/services/grid-setting.service';
import { MfsSettingService } from 'src/app/services/mfs-setting.service';

@Component({
  selector: 'app-rate-config-list',
  templateUrl: './rate-config-list.component.html',
  styleUrls: ['./rate-config-list.component.css']
})
export class RateConfigListComponent implements OnInit {

    gridConfig: any;
    optionList: any;

    constructor(private gridSettingService: GridSettingService, private mfsSettingService: MfsSettingService) {
        this.gridConfig = {};
        this.optionList = [{ label: "USSD", value: "U" }, { label: "Apps", value: "A" }, { label: "Common", value: "C" }, { label: "All", value: "All" }];
    }

    ngOnInit() {
        this.initialiseGridConfig();
    }

    initialiseGridConfig(): any {
        this.gridConfig.dataSource = [];
        this.gridConfig.gridName = ' Rate Config';
        this.gridConfig.gridIconClass = 'fas fa-file-contract';
        
        this.gridConfig.customFilterOptionList = this.optionList;
        this.gridConfig.selectedCustomFilter = 'U';
        
        this.gridConfig.dataSourcePath = this.mfsSettingService.transactionApiServer + '/RateconfigMst/GetRateConfigList?param='; //-- FOR DATA SOURCE PATH 

        this.gridConfig.autoUpdateDataSource = true; // --- FOR AUTO BINDING THE DATA AFTER DATA RETRIVAL FROM THE API SERVER
        this.gridConfig.autoIndexing = true;  // FOR INDEXING THE DATA GRID WHICH CAN BE ACCESSED BY --INDEX--
        this.gridConfig.showUniversalFilter = false; //-- ---- CAN TURN IT OFF ANY TIME IF NOT NEEDED

        this.gridConfig.createStateUrl = '/rate-config/addoredit/';
        this.gridConfig.hasEditState = true;
        this.gridConfig.entityField = 'configId';

        this.gridConfig.columnList = [
            { field: 'rateconfig_for', header: 'Name', width: '12%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'fromCatId', header: 'From Category', width: '8%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'toCatId', header: 'To Category', width: '8%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'particular', header: 'Particular', width: '35%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'balanceCheck', header: 'Balance Check', width: '8%', filter: this.gridSettingService.getDefaultFilterable(), template: this.gridSettingService.getYesNoTemplateForRowData() },
            { field: 'status', header: 'Status', width: '8%', filter: this.gridSettingService.getDefaultFilterable(), template: this.gridSettingService.getStatusTemplateForRowData() },
            { field: 'configId', header: 'Action', width: '10%', isEditColumn: true, filter: this.gridSettingService.getFilterableNone() }
        ];

    };

}
