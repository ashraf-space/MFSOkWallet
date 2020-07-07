import { Component, OnInit } from '@angular/core';
import { TerritoryService } from '../../../../services/environment/territory.service';
import { GridSettingService } from '../../../../services/grid-setting.service';
import { MfsSettingService } from '../../../../services/mfs-setting.service';

@Component({
  selector: 'app-territory-list',
  templateUrl: './territory-list.component.html',
  styleUrls: ['./territory-list.component.css']
})
export class TerritoryListComponent implements OnInit {

    gridConfig: any;

    constructor(private territoryService: TerritoryService,
        private gridSettingService: GridSettingService,
        private mfsSettingService: MfsSettingService) {
        this.gridConfig = {};
    }


    ngOnInit() {
        this.initialiseGridConfig();
    }
    initialiseGridConfig(): any {
        this.gridConfig.dataSource = [];
        this.gridConfig.gridName = 'Territory List';
        this.gridConfig.gridIconClass = 'fas fa-list';
        this.gridConfig.createStateUrl = '/territory/addoredit/';
        this.gridConfig.hasEditState = true;
        this.gridConfig.entityField = 'tCode';

        this.gridConfig.dataSourcePath = this.mfsSettingService.environmentApiServer + '/Territory/GetTerritories';
        this.gridConfig.autoUpdateDataSource = true;


        this.gridConfig.columnList = [
            { field: 'rCode', header: 'Region Code', width: '25%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'rName', header: 'Region Name', width: '30%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'aCode', header: 'Area Code', width: '20%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'aName', header: 'Area Name', width: '30%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'tCode', header: 'Territory Code', width: '25%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'tName', header: 'Territory Name', width: '30%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'tCode', header: 'Edit', width: '10%', isEditColumn: true, filter: this.gridSettingService.getFilterableNone() }
        ];

        //this.territoryService.getTerrirories().pipe()
        //    .subscribe(data => {
        //        this.gridConfig.dataSource = data;
        //    });
    };

}
