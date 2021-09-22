import { Component, OnInit, Input } from '@angular/core';
import { GridSettingService } from '../../../services/grid-setting.service';
import { MfsSettingService } from '../../../services/mfs-setting.service';
import { AuthenticationService } from 'src/app/shared/_services';

@Component({
  selector: 'app-status-list',
  templateUrl: './status-list.component.html',
  styleUrls: ['./status-list.component.css']
})
export class StatusListComponent implements OnInit {
    gridConfig: any;
    currentUserModel: any = {};
    showModal: any;

    constructor(
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
    initialiseGridConfig(): any {
        this.gridConfig.dataSource = [];
        this.gridConfig.gridName = 'Close List';
        this.gridConfig.gridIconClass = 'fas fa-list';
        this.gridConfig.hasEditState = false;
        this.gridConfig.entityField = 'MPHONE';
        this.gridConfig.dataSourcePath = this.mfsSettingService.distributionApiServer + '/Kyc/GetCloseAccount';
        this.gridConfig.autoUpdateDataSource = true;
        this.gridConfig.showUniversalFilter = true;
        this.gridConfig.detailsStateUrl = 'status/details/';
        this.gridConfig.hasCustomContent = true;
        this.gridConfig.hideCreateState = true;
        this.gridConfig.columnList = [
            { field: 'MPHONE', header: 'Channel AC No', width: '10%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'NAME', header: 'Name', width: '20%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'PHOTO_ID', header: 'Photo Id No', width: '10%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'MPHONE', header: 'Details', width: '7%', isDetailsColumn: true, filter: this.gridSettingService.getFilterableNone() }
        ];
    };

}
