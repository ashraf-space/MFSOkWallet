import { Component, OnInit } from '@angular/core';
import { RegionListService } from  '../../../../../services/environment/regionList.service'
import { GridSettingService } from '../../../../../services/grid-setting.service';



@Component({
  selector: 'app-region-list',
  templateUrl: './region-list.component.html',
  styleUrls: ['./region-list.component.css']
})
export class RegionListComponent implements OnInit {
    gridConfig: any;      
    constructor(private regionListService: RegionListService, private gridSettingService: GridSettingService) {
        this.gridConfig = {};
    }

    ngOnInit() {
        this.initialiseGridConfig();
  }
    initialiseGridConfig(): any {
        this.gridConfig.dataSource = [];
        this.gridConfig.gridName = 'Region  List';
        this.gridConfig.gridIconClass = 'fas fa-list';
        this.gridConfig.createStateUrl = 'region/addoredit/';
        this.gridConfig.hasEditState = true;
        this.gridConfig.entityField = 'id';

        this.gridConfig.columnList = [
            { field: 'name', header: 'Category Name', width: '42%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'icon', header: 'Icon', width: '98%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'orderNo', header: 'Order Number', width: '60%', filter: this.gridSettingService.getDefaultNumberFilterable(0, 10) },
            { field: 'id', header: 'Edit', width: '10%', isEditColumn: true, filter: this.gridSettingService.getFilterableNone() }
        ];
    };
}
