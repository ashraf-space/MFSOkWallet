import { Component, OnInit } from '@angular/core';
import { ApplicationUserService } from '../../../../services/security';
import { first } from 'rxjs/operators';
import { Router, ActivatedRoute } from '@angular/router';
import { GridSettingService } from '../../../../services/grid-setting.service';
import { AuthenticationService } from 'src/app/shared/_services';

@Component({
  selector: 'app-application-user-list',
  templateUrl: './application-user-list.component.html',
  styleUrls: ['./application-user-list.component.css']
})
export class ApplicationUserListComponent implements OnInit {

    gridConfig: any;
    roleName: any;
    currentUserModel: any = {};

    constructor(private applicationUserService: ApplicationUserService, private gridSettingService: GridSettingService,
        private authenticationService: AuthenticationService) {
        this.gridConfig = {};
        this.authenticationService.currentUser.subscribe(x => {
            this.currentUserModel = x;
        });
    }

    ngOnInit() {
        this.roleName = this.currentUserModel.user.role_Name;
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
            { field: 'NAME', header: 'Full Name', width: '20%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'USERNAME', header: 'User Name', width: '15%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'ROLENAME', header: 'Role Name', width: '15%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'MOBILE_NO', header: 'Mobile Number#', width: '15%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'EMAIL_ID', header: 'Email Id', width: '15%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'LOG_IN_STATUS', header: 'Log In Status', width: '10%', filter: this.gridSettingService.getDefaultFilterable(), template: this.gridSettingService.getYesNoTemplateForRowData() },
            { field: 'PSTATUS', header: 'Password Status', width: '15%', filter: this.gridSettingService.getDefaultFilterable(), template: this.gridSettingService.getPinStatusTemplateForRowData() },
            { field: 'ID', header: 'Edit', width: '15%', isEditColumn: true, filter: this.gridSettingService.getFilterableNone() },
            //{ field: 'id', header: 'Details', width: '15%', isDetailsColumn: true, filter: this.gridSettingService.getFilterableNone() }
        ];

        this.applicationUserService.getApplicationUserWorklist(this.roleName).pipe()
            .subscribe(data => {
                this.gridConfig.dataSource = data;
            })
    };

}
