import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { OutboxService } from 'src/app/services/client/outbox.service';
import { GridSettingService } from 'src/app/services/grid-setting.service';
import { AuthenticationService, AuditTrailService } from 'src/app/shared/_services';
import { MfsSettingService } from 'src/app/services/mfs-setting.service';
import { MfsUtilityService } from 'src/app/services/mfs-utility.service';
import { GenericGridComponent } from 'src/app/shared/directives/generic-grid/generic-grid.component';
import { first } from 'rxjs/operators';

@Component({
  selector: 'app-outbox',
  templateUrl: './outbox.component.html',
    styleUrls: ['./outbox.component.css']
})
export class OutboxComponent implements OnInit {
    gridConfig: any;
    currentUserModel: any;
    @ViewChild(GenericGridComponent) child: GenericGridComponent;
    @Input() mPhone: string;
    auditTrailModel: any = {};
    dateObj: any = {};    
    messageTypeList: any;

    constructor(private outboxService: OutboxService, private gridSettingService: GridSettingService, private authService: AuthenticationService
        , private mfsSettingService: MfsSettingService, private mfsUtilityService: MfsUtilityService, private auditTrailService: AuditTrailService) {
        this.gridConfig = {};
        this.messageTypeList = [{ label: 'SMS', value: true }, { label: 'USSD/Flash', value: false }];
        this.authService.currentUser.subscribe(x => {
            this.currentUserModel = x;
        });
    }
    ngOnChanges() {
        if (this.mPhone) {
            this.initialiseGridConfig();
        }
    }
    ngOnInit() {        
        this.dateObj.toDate = this.mfsUtilityService.getFullDateByMonthParam(0, 1);
        this.dateObj.fromDate = this.mfsUtilityService.getFullDateByMonthParam(0, 1);
        this.dateObj.messageType = true;
        this.initialiseGridConfig();
    }

    onDateChange() {
        if (this.dateObj.mPhone && this.dateObj.mPhone != '') {
            this.gridConfig.dataSourcePath = this.mfsSettingService.clientApiServer + '/Outbox/GetOutboxList?fromDate=' + this.mfsUtilityService.renderDate(this.dateObj.fromDate, true) +
                '&ToDate=' + this.mfsUtilityService.renderDate(this.dateObj.toDate, true) + '&mPhone=' + this.dateObj.mPhone + '&forMessageResend=' + this.dateObj.messageType;
        }
        else {
            this.gridConfig.dataSourcePath = this.mfsSettingService.clientApiServer + '/Outbox/GetOutboxList?fromDate=' + this.mfsUtilityService.renderDate(this.dateObj.fromDate, true) +
                '&ToDate=' + this.mfsUtilityService.renderDate(this.dateObj.toDate, true) + '&forMessageResend=' + this.dateObj.messageType;;
        }
        this.insertDataToAuditTrail();
        this.child.updateDataSource();
    }
    insertDataToAuditTrail() {
        this.auditTrailModel.Who = this.currentUserModel.user.username;
        this.auditTrailModel.WhatAction = 'SEARCH';
        this.auditTrailModel.WhatActionId = this.auditTrailService.getWhatActionId('SEARCH');
        var eventLog = JSON.parse(sessionStorage.getItem('currentEvent'));
        this.auditTrailModel.WhichMenu = eventLog.item.label.trim();
        this.auditTrailModel.WhichParentMenu = this.currentUserModel.featureList.find(it => {
            return it.FEATURENAME.includes(this.auditTrailModel.WhichMenu);
        }).CATEGORYNAME;
        this.auditTrailModel.WhichParentMenuId = this.auditTrailService.getWhichParentMenuId(this.auditTrailModel.WhichParentMenu);
        this.auditTrailModel.inputFeildAndValue = [
            { whichFeildName: 'From Date', whatValue: this.mfsUtilityService.renderDate(this.dateObj.fromDate) },
            { whichFeildName: 'To Date', whatValue: this.mfsUtilityService.renderDate(this.dateObj.toDate) },
            { whichFeildName: 'Select Gateway', whatValue: this.dateObj.messageType },
        ];
        if (this.dateObj.mPhone) {
            this.auditTrailModel.inputFeildAndValue.push({ whichFeildName: 'mphone', whatValue: this.dateObj.mPhone })
        }
        this.auditTrailService.insertIntoAuditTrail(this.auditTrailModel).pipe(first())
            .subscribe(
                data => {
                    if (data) {
                    }
                },
                error => {

                });
    }
    initialiseGridConfig(): any {
        this.gridConfig.dataSource = [];

        this.gridConfig.columnList = [
            { field: 'inMsg', header: 'Recieved Message', width: '10%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'outMsg', header: 'Reply Message', width: '25%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'inTime', header: 'Date', width: '10%', filter: this.gridSettingService.getDefaultDateFilterable(), template: this.gridSettingService.getDateTemplateForRowData() },
            { field: 'msgChannel', header: 'Type', width: '5%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'status', header: 'status', width: '10%', filter: this.gridSettingService.getDefaultFilterable(), template: this.gridSettingService.getOutboxStatusTemplateForRowData() }
        ];

        if (this.mPhone) {
            this.dateObj.mPhone = this.mPhone;
            this.dateObj.fromDate = this.mfsUtilityService.getFullDateByMonthParam(0,1);
            this.gridConfig.dataSourcePath = this.mfsSettingService.clientApiServer + '/Outbox/GetOutboxList?mPhone=' + this.dateObj.mPhone + '&fromDate=' + this.mfsUtilityService.renderDate(this.dateObj.fromDate, true) + '&forMessageResend=' + this.dateObj.messageType;;
        }
        else {
            this.gridConfig.columnList.push({ field: 'mphone', header: 'A/C #', width: '10%', filter: this.gridSettingService.getDefaultFilterable() },
                { field: 'name', header: 'Name', width: '10%', filter: this.gridSettingService.getDefaultFilterable() },
                { field: 'catDesc', header: 'Customer Type', width: '10%', filter: this.gridSettingService.getDefaultFilterable() });
            this.gridConfig.dataSourcePath = this.mfsSettingService.clientApiServer + '/Outbox/GetOutboxList';
        }
        
        this.gridConfig.autoUpdateDataSource = true; 
        this.gridConfig.autoIndexing = true;

        this.gridConfig.gridName = "Outbox list";
        this.gridConfig.gridIconClass = 'fas fa-envelope-open-text';
        
        this.gridConfig.entityField = 'mphone';
        this.gridConfig.hasCustomContent = true;
        this.gridConfig.showUniversalFilter = false;
        this.gridConfig.showExport = false;
    }
;
}
