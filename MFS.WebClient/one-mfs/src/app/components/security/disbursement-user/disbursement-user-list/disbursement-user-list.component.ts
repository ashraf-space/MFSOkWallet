import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from 'src/app/shared/_services';
import { GridSettingService } from 'src/app/services/grid-setting.service';
import { DisbursementUserService } from 'src/app/services/security/disbursement-user.service';

@Component({
  selector: 'app-disbursement-user-list',
  templateUrl: './disbursement-user-list.component.html',
  styleUrls: ['./disbursement-user-list.component.css']
})
export class DisbursementUserListComponent implements OnInit {

    gridConfig: any;
    roleName: any;
    currentUserModel: any = {};

    constructor(private disbursementUserService: DisbursementUserService, private gridSettingService: GridSettingService,
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
        this.gridConfig.gridName = ' Disbursement User List';
        this.gridConfig.gridIconClass = 'fab fa-accusoft';
        this.gridConfig.createStateUrl = '/disbursement-user/add-edit/';
        this.gridConfig.hasEditState = true;
        this.gridConfig.entityField = 'id';
        //this.gridConfig.detailsStateUrl = '/application-user/details/'

        this.gridConfig.columnList = [
            { field: 'COMPANY_NAME', header: 'Company Name', width: '20%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'USERNAME', header: 'User Name', width: '20%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'MOBILE_NO', header: 'Mobile Number#', width: '20%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'LOG_IN_STATUS', header: 'Log In Status', width: '20%', filter: this.gridSettingService.getDefaultFilterable(), template: this.gridSettingService.getYesNoTemplateForRowData() },
            { field: 'ID', header: 'Edit', width: '15%', isEditColumn: true, filter: this.gridSettingService.getFilterableNone() },
            //{ field: 'id', header: 'Details', width: '15%', isDetailsColumn: true, filter: this.gridSettingService.getFilterableNone() }
        ];

        this.disbursementUserService.getDisbursementUserWorklist(this.roleName).pipe()
            .subscribe(data => {
                this.gridConfig.dataSource = data;
            })
    };

}
