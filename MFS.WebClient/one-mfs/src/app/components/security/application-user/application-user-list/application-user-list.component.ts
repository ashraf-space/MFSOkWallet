import { Component, OnInit } from '@angular/core';
import { ApplicationUserService } from '../../../../services/security';
import { first } from 'rxjs/operators';
import { Router, ActivatedRoute } from '@angular/router';
import { GridSettingService } from '../../../../services/grid-setting.service';

@Component({
  selector: 'app-application-user-list',
  templateUrl: './application-user-list.component.html',
  styleUrls: ['./application-user-list.component.css']
})
export class ApplicationUserListComponent implements OnInit {

    gridConfig: any;

    constructor(private applicationUserService: ApplicationUserService, private gridSettingService: GridSettingService) {
        this.gridConfig = {};
    }

    ngOnInit() {
        this.initialiseGridConfig();
    }

    initialiseGridConfig(): any {
        this.gridConfig.dataSource = [];
        this.gridConfig.gridName = ' Application User List';
        this.gridConfig.gridIconClass = 'fab fa-accusoft';
        this.gridConfig.createStateUrl = '/application-user/add-edit/';
        this.gridConfig.hasEditState = true;
        this.gridConfig.entityField = 'id';
        //this.gridConfig.detailsStateUrl = '/application-user/details/'

        this.gridConfig.columnList = [
            { field: 'name', header: 'Full Name', width: '20%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'username', header: 'User Name', width: '15%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'mobileNo', header: 'Mobile Number#', width: '20%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'emailId', header: 'Email Id', width: '20%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'logInStatus', header: 'Log In Status', width: '15%', filter: this.gridSettingService.getDefaultFilterable(), template: this.gridSettingService.getYesNoTemplateForRowData() },
            { field: 'pstatus', header: 'Password Status', width: '15%', filter: this.gridSettingService.getDefaultFilterable(), template: this.gridSettingService.getPinStatusTemplateForRowData() },
            { field: 'id', header: 'Edit', width: '15%', isEditColumn: true, filter: this.gridSettingService.getFilterableNone() },
            //{ field: 'id', header: 'Details', width: '15%', isDetailsColumn: true, filter: this.gridSettingService.getFilterableNone() }
        ];

        this.applicationUserService.getApplicationUserWorklist().pipe()
            .subscribe(data => {
                this.gridConfig.dataSource = data;
            })
    };

}
