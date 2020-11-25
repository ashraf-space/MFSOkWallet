import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { AuditTrailService } from '../../../../shared/_services/audit-trail.service';
import { GridSettingService } from 'src/app/services/grid-setting.service';
import { AuthenticationService } from 'src/app/shared/_services';
import { MfsSettingService } from 'src/app/services/mfs-setting.service';
import { MfsUtilityService } from 'src/app/services/mfs-utility.service';
import { GenericGridComponent } from 'src/app/shared/directives/generic-grid/generic-grid.component';
import { first } from 'rxjs/operators';
import { ApplicationUserService } from '../../../../services/security/application-user.service';
import { MessageService, MenuItem } from 'primeng/api';

@Component({
    selector: 'app-error-list',
    templateUrl: './error-list.component.html',
    styleUrls: ['./error-list.component.css']
})
export class ErrorListComponent implements OnInit {

    gridConfig: any;
    currentUserModel: any;
    @ViewChild(GenericGridComponent) child: GenericGridComponent;
    @Input() mPhone: string;
    showGrid: boolean = false;
    errorObj: any = {};
    messageTypeList: any;
    userList: any;
    actionList: any;
    parentMenuList: any;
    constructor(private messageService: MessageService,
        private applicationUserService: ApplicationUserService,
        private gridSettingService: GridSettingService,
        private authService: AuthenticationService,
        private mfsSettingService: MfsSettingService,
        private mfsUtilityService: MfsUtilityService) {
        this.gridConfig = {};
        this.authService.currentUser.subscribe(x => {
            this.currentUserModel = x;
        });
    }


    ngOnInit() {
        this.getUserListDdl();
        this.initialiseGridConfig();
    }
    getUserListDdl() {
        this.applicationUserService.getAppUserListDdl()
            .pipe(first())
            .subscribe(
                data => {
                    this.userList = data;
                },
                error => {
                    console.log(error);
                }
            );
    }
    onSearch() {
        this.showGrid = true;
        if (this.errorObj.fromDate && this.errorObj.toDate) {
            this.gridConfig.dataSourcePath = this.mfsSettingService.clientApiServer + '/Errors/GetErrorByFiltering?fromDate=' + this.mfsUtilityService.renderDate(this.errorObj.fromDate, true) +
                '&ToDate=' + this.mfsUtilityService.renderDate(this.errorObj.toDate, true) + '&user=' + this.errorObj.user;
        }
        else {
            this.messageService.add({ severity: 'error', summary: 'Select Option', detail:'Select Date Option', closable: true });
        }
        if (this.child) {
            this.child.updateDataSource();
        }
    }
    initialiseGridConfig(): any {
        this.gridConfig.dataSource = [];
        this.gridConfig.gridName = ' Error Log';
        this.gridConfig.gridIconClass = 'fas fa-bug';

        this.gridConfig.dataSourcePath = this.mfsSettingService.clientApiServer + '/Errors/GetErrorList'; //-- FOR DATA SOURCE PATH 

        this.gridConfig.autoUpdateDataSource = true; // --- FOR AUTO BINDING THE DATA AFTER DATA RETRIVAL FROM THE API SERVER        

        this.gridConfig.columnList = [
            { field: 'message', header: 'Message', width: '20%' },
            { field: 'funcName', header: 'Function Name', width: '20%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'date', header: 'Date', width: '10%', filter: this.gridSettingService.getDefaultFilterable(), template: this.gridSettingService.getDateTemplateForRowData() },
            { field: 'userName', header: 'User Name', width: '10%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'userRole', header: 'Role', width: '10%', filter: this.gridSettingService.getDefaultFilterable() }
        ];

    };

}
