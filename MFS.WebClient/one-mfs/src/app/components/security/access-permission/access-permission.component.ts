import { Component, OnInit } from '@angular/core';
import { PermissionService } from '../../../services/security';
import { first } from 'rxjs/operators';
import { Router, ActivatedRoute } from '@angular/router';
import { GridSettingService } from '../../../services/grid-setting.service';
import { MfsUtilityService } from '../../../services/mfs-utility.service';
import { MfsSettingService } from '../../../services/mfs-setting.service';

@Component({
  selector: 'app-access-permission',
  templateUrl: './access-permission.component.html',
  styleUrls: ['./access-permission.component.css']
})
export class AccessPermissionComponent implements OnInit {

    gridConfig: any;

    constructor(private gridSettingService: GridSettingService, private mfsSettingService: MfsSettingService) {
        this.gridConfig = {};
    }

    ngOnInit() {
        this.initialiseGridConfig();
    }

    initialiseGridConfig(): any {
        this.gridConfig.dataSource = [];
        this.gridConfig.gridName = ' Access Permission';
        this.gridConfig.gridIconClass = 'fas fa-universal-access';        
        this.gridConfig.isBatchSwitchBoxEdit = true;

        this.gridConfig.customFilterOptionPath = this.mfsSettingService.securityApiServer + '/Role/GetRoleListForDDL';
        this.gridConfig.selectedCustomFilter = 1;

        this.gridConfig.batchUpdatePath = this.mfsSettingService.securityApiServer + '/Permission/UpdatePermissions';
        this.gridConfig.dataSourcePath = this.mfsSettingService.securityApiServer + '/permission/GetPermissionWorklist?roleId='; //-- FOR DATA SOURCE PATH 

        this.gridConfig.autoUpdateDataSource = true; // --- FOR AUTO BINDING THE DATA AFTER DATA RETRIVAL FROM THE API SERVER
        this.gridConfig.autoIndexing = true;  // FOR INDEXING THE DATA GRID WHICH CAN BE ACCESSED BY --INDEX--
        this.gridConfig.showUniversalFilter = false; //-- ---- CAN TURN IT OFF ANY TIME IF NOT NEEDED

        this.gridConfig.columnList = [
            { field: 'index', header: 'Index', width: '12.5%', filter: this.gridSettingService.getFilterableNone() },
            { field: 'featureName', header: 'Feature', width: '40%', filter: this.gridSettingService.getDefaultFilterable() },  
            { field: 'isAddPermitted', header: 'Add Permission', width: '25%', isSwitchBoxColumn: true, filter: this.gridSettingService.getFilterableNoneAndToggleSelectAll() },
            { field: 'isEditPermitted', header: 'Edit Permission', width: '25%', isSwitchBoxColumn: true, filter: this.gridSettingService.getFilterableNoneAndToggleSelectAll() },
            { field: 'isDeletePermitted', header: 'Delete Permission', width: '25%', isSwitchBoxColumn: true, filter: this.gridSettingService.getFilterableNoneAndToggleSelectAll() },            
            { field: 'isRegistrationPermitted', header: 'Registration Permission', width: '25%', isSwitchBoxColumn: true, filter: this.gridSettingService.getFilterableNoneAndToggleSelectAll() },
            { field: 'isSecuredviewPermitted', header: 'Secured Data View Permission', width: '25%', isSwitchBoxColumn: true, filter: this.gridSettingService.getFilterableNoneAndToggleSelectAll() },
            { field: 'isViewPermitted', header: 'View Permission', width: '25%', isSwitchBoxColumn: true, filter: this.gridSettingService.getFilterableNoneAndToggleSelectAll() }            
        ];

    };

}
