import { Component, OnInit } from '@angular/core';
import { RoleService } from '../../../../services/security';
import { first } from 'rxjs/operators';
import { Router, ActivatedRoute } from '@angular/router';
import { GridSettingService } from '../../../../services/grid-setting.service';

@Component({
  selector: 'app-role-list',
  templateUrl: './role-list.component.html',
  styleUrls: ['./role-list.component.css']
})
export class RoleListComponent implements OnInit {

    gridConfig: any;

    constructor(private roleService: RoleService, private gridSettingService: GridSettingService) {
        this.gridConfig = {};
    }

    ngOnInit() {
        this.initialiseGridConfig();
    }

    initialiseGridConfig(): any {
        this.gridConfig.dataSource = [];
        this.gridConfig.gridName = ' Secured Role List';
        this.gridConfig.gridIconClass = 'fas fa-dice-d20';
        this.gridConfig.createStateUrl = '/role/add-edit/';
        this.gridConfig.hasEditState = true;
        this.gridConfig.entityField = 'id';

        this.gridConfig.columnList = [
            { field: 'name', header: 'Role Name', width: '42%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'description', header: 'Role Description', width: '98%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'priority', header: 'Priority', width: '60%', filter: this.gridSettingService.getDefaultNumberFilterable(0, 10) },
            { field: 'id', header: 'Edit', width: '15%', isEditColumn: true, filter: this.gridSettingService.getFilterableNone() }
        ];

        this.roleService.getRoleWorklist().pipe()
            .subscribe(data => {
                this.gridConfig.dataSource = data;
            })
    };

}
