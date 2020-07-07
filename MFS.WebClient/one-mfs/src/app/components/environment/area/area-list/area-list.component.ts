import { Component, OnInit } from '@angular/core';
import { GridSettingService } from '../../../../services/grid-setting.service';
import { AreaService } from '../../../../services/environment';
import { MfsSettingService } from '../../../../services/mfs-setting.service';
import { AuthenticationService } from 'src/app/shared/_services';
@Component({
  selector: 'app-area-list',
  templateUrl: './area-list.component.html',
  styleUrls: ['./area-list.component.css']
})
export class AreaListComponent implements OnInit {
    gridConfig: any;      
    constructor(private areaService: AreaService,
        private gridSettingService: GridSettingService,
        private mfsSettingService: MfsSettingService,
        private authService: AuthenticationService) {
        this.gridConfig = {};
    }

    ngOnInit() {
        //this.progressSpinnerModule = true;
        this.initialiseGridConfig();
        //this.progressSpinnerModule = false;
    }
    initialiseGridConfig(): any {
        this.gridConfig.dataSource = [];
        this.gridConfig.gridName = 'Area List';
        this.gridConfig.gridIconClass = 'fab fa-phabricator';
        this.gridConfig.createStateUrl = '/area/addoredit/';
        this.gridConfig.hasEditState = true;
        this.gridConfig.entityField = 'aCode';

        this.gridConfig.dataSourcePath = this.mfsSettingService.environmentApiServer + '/location/getareas';
        this.gridConfig.autoUpdateDataSource = true;

        this.gridConfig.columnList = [
            { field: 'rCode', header: 'Region Code', width: '25%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'rName', header: 'Region Name', width: '30%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'aCode', header: 'Area Code', width: '20%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'aName', header: 'Area Name', width: '30%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'aCode', header: 'Edit', width: '10%', isEditColumn: true, filter: this.gridSettingService.getFilterableNone() }
        ];

        //this.areaService.getAreas().pipe()
        //    .subscribe(data => {
        //        this.gridConfig.dataSource = data;
        //    });
    };

}
