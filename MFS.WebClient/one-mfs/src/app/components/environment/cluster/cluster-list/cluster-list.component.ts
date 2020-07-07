import { Component, OnInit } from '@angular/core';
import { ClusterService } from '../../../../services/environment/cluster.service';
import { GridSettingService } from '../../../../services/grid-setting.service';
import { MfsSettingService } from '../../../../services/mfs-setting.service';

@Component({
  selector: 'app-cluster-list',
  templateUrl: './cluster-list.component.html',
  styleUrls: ['./cluster-list.component.css']
})
export class ClusterListComponent implements OnInit {
    gridConfig: any;
    constructor(private clusterService: ClusterService,
        private gridSettingService: GridSettingService,
        private mfsSettingService: MfsSettingService) {
        this.gridConfig = {};
    }

    ngOnInit() {
        this.initialiseGridConfig();
    }

    initialiseGridConfig(): any {
        this.gridConfig.dataSource = [];
        this.gridConfig.gridName = 'Cluster List';
        this.gridConfig.gridIconClass = 'fas fa-list';
        this.gridConfig.createStateUrl = '/cluster/addoredit/';
        this.gridConfig.hasEditState = true;
        this.gridConfig.entityField = 'clCode';


        this.gridConfig.dataSourcePath = this.mfsSettingService.environmentApiServer + '/Cluster/GetAllClusters';
        this.gridConfig.autoUpdateDataSource = true;

        this.gridConfig.columnList = [
            { field: 'rCode', header: 'Region Code', width: '15%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'rName', header: 'Region Name', width: '20%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'aCode', header: 'Area Code', width: '15%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'aName', header: 'Area Name', width: '20%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'tCode', header: 'Territory Code', width: '15%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'tName', header: 'Territory Name', width: '20%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'clName', header: 'Cluster Name', width: '15%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'clCode', header: 'Cluster Code', width: '20%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'clCode', header: 'Edit', width: '10%', isEditColumn: true, filter: this.gridSettingService.getFilterableNone() }
        ];

        //this.clusterService.GetAllClusters().pipe()
        //    .subscribe(data => {
        //        this.gridConfig.dataSource = data;
        //    });
    };

}
