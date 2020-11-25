
import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { AuditTrailService } from '../../../../shared/_services/audit-trail.service';
import { GridSettingService } from 'src/app/services/grid-setting.service';
import { AuthenticationService } from 'src/app/shared/_services';
import { MfsSettingService } from 'src/app/services/mfs-setting.service';
import { MfsUtilityService } from 'src/app/services/mfs-utility.service';
import { GenericGridComponent } from 'src/app/shared/directives/generic-grid/generic-grid.component';
import { first } from 'rxjs/operators';

@Component({
    selector: 'app-audit-trail',
    templateUrl: './audit-trail.component.html',
    styleUrls: ['./audit-trail.component.css']
})
export class AuditTrailComponent implements OnInit {

    gridConfig: any;
    currentUserModel: any;
    @ViewChild(GenericGridComponent) child: GenericGridComponent;
    @Input() auditId: string;
    showGrid: boolean = false;
    auditObj: any = {};
    messageTypeList: any;
    userList: any;
    actionList: any;
    parentMenuList: any;
    showModal: any;
    constructor(private auditTrailService: AuditTrailService, private gridSettingService: GridSettingService, private authService: AuthenticationService
        , private mfsSettingService: MfsSettingService, private mfsUtilityService: MfsUtilityService) {
        this.gridConfig = {};
        this.authService.currentUser.subscribe(x => {
            this.currentUserModel = x;
        });
    }

    ngOnInit() {
        this.actionList = this.mfsUtilityService.getActionList();
        this.parentMenuList = this.mfsUtilityService.getParentMenuList();
        this.getUserListDdl();
        this.initialiseGridConfig();
    }
    getUserListDdl() {
        this.auditTrailService.getUserListDdl()
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
        if (this.auditObj.user && this.auditObj.fromDate && this.auditObj.toDate) {
            this.gridConfig.dataSourcePath = this.mfsSettingService.securityApiServer + '/AuditTrail/GetAuditTrail?fromDate=' + this.mfsUtilityService.renderDate(this.auditObj.fromDate, true) +
                '&ToDate=' + this.mfsUtilityService.renderDate(this.auditObj.toDate, true) + '&user=' + this.auditObj.user + '&userAction=' + this.auditObj.action + '&menu=' + this.auditObj.menu;
        }
        else if ((!this.auditObj.fromDate || !this.auditObj.toDate)) {
            this.gridConfig.dataSourcePath = this.mfsSettingService.securityApiServer + '/AuditTrail/GetAuditTrail?user=' + this.auditObj.user + '&userAction=' + this.auditObj.action + '&menu=' + this.auditObj.menu;
        }
        else {
        }
        if (this.child) {
            this.child.updateDataSource();
        }
    }
    ondetail(event) {
        this.auditId = event;
        this.showModal = true;
    }
    initialiseGridConfig(): any {
        this.gridConfig.dataSource = [];
        this.gridConfig.dataSourcePath = [];
        this.gridConfig.autoUpdateDataSource = true;
        this.gridConfig.autoIndexing = true;
        this.gridConfig.gridName = "Trail list";
        this.gridConfig.gridIconClass = 'fas fa-envelope-open-text';
        this.gridConfig.entityField = 'id';
        this.gridConfig.createStateUrl = 'audit/audit-trail-dtl/';
        this.gridConfig.hasEditState = true;
        this.gridConfig.showUniversalFilter = true;
        this.gridConfig.hideCreateState = true;
        this.gridConfig.columnList = [
            { field: 'who', header: 'User Name', width: '10%' },
            { field: 'when', header: 'When Date', width: '10%', filter: this.gridSettingService.getDefaultFilterable(), template: this.gridSettingService.getDateTemplateForRowData()  },
            { field: 'action', header: 'Action', width: '5%', filter: this.gridSettingService.getDefaultDateFilterable()},
            { field: 'menu', header: 'Menu', width: '5%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'whichId', header: 'which Id', width: '10%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'response', header: 'Response', width: '15%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'id', header: 'id', width: '10%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'id', header: 'View', width: '5%', isCustomAction: true, customActionIcon: 'fas fa-info-circle', filter: this.gridSettingService.getFilterableNone(), actionDisableParam: 'id', disableValue: null}
        ];
    }

}
