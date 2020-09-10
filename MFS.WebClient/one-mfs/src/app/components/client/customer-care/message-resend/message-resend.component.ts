import { Component, OnInit, ViewChild } from '@angular/core';
import { OutboxService } from 'src/app/services/client/outbox.service';
import { GridSettingService } from 'src/app/services/grid-setting.service';
import { AuthenticationService, AuditTrailService} from 'src/app/shared/_services';
import { MfsSettingService } from 'src/app/services/mfs-setting.service';
import { MfsUtilityService } from 'src/app/services/mfs-utility.service';
import { GenericGridComponent } from 'src/app/shared/directives/generic-grid/generic-grid.component';
import { first } from 'rxjs/operators';
import { MessageService, ConfirmationService, Message } from 'primeng/api';

@Component({
  selector: 'app-message-resend',
  templateUrl: './message-resend.component.html',
    styleUrls: ['./message-resend.component.css'],
    providers: [ConfirmationService]
})
export class MessageResendComponent implements OnInit {

    gridConfig: any;
    currentUserModel: any;
    @ViewChild(GenericGridComponent) child: GenericGridComponent;

    dateObj: any = {};
    auditTrailModel: any = {};
    constructor(private outboxService: OutboxService,
        private gridSettingService: GridSettingService,
        private authService: AuthenticationService,
        private auditTrailService: AuditTrailService,
        private mfsSettingService: MfsSettingService,
        private mfsUtilityService: MfsUtilityService,
        private messageService: MessageService,
        private confirmationService: ConfirmationService) {
        this.gridConfig = {};
        this.authService.currentUser.subscribe(x => {
            this.currentUserModel = x;
        });
    }

    ngOnInit() {
        this.dateObj.toDate = this.mfsUtilityService.getFullDateByMonthParam(0, 1);
        this.dateObj.fromDate = this.mfsUtilityService.getFullDateByMonthParam(0, 1);
        this.initialiseGridConfig();
    }

    onDateChange() {
        var mphoneQuery = this.dateObj.mphone;  
        this.gridConfig.dataSourcePath = this.mfsSettingService.clientApiServer + '/Outbox/GetOutboxList?fromDate=' + this.mfsUtilityService.renderDate(this.dateObj.fromDate, true) +
            '&ToDate=' + this.mfsUtilityService.renderDate(this.dateObj.toDate, true) + '&mphone=' + this.dateObj.mphone + '&forMessageResend=true'; 
        this.insertDataToAuditTrail();
        this.child.updateDataSource();
    }

    initialiseGridConfig(): any {
        this.gridConfig.dataSource = [];

        this.gridConfig.dataSourcePath = this.mfsSettingService.clientApiServer + '/Outbox/GetOutboxList?forMessageResend=true';
        this.gridConfig.autoUpdateDataSource = true;
        this.gridConfig.autoIndexing = true;

        this.gridConfig.gridName = "Message Resend";
        this.gridConfig.gridIconClass = 'fas fa-paper-plane';

        this.gridConfig.entityField = 'mphone';
        this.gridConfig.hasCustomContent = true;
        this.gridConfig.showUniversalFilter = false;
        this.gridConfig.showExport = false;
        this.gridConfig.columnList = [
            { field: 'mphone', header: 'A/C #', width: '10%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'name', header: 'Name', width: '7%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'catDesc', header: 'Customer Type', width: '8%', filter: this.gridSettingService.getDefaultFilterable() },            
            { field: 'outMsg', header: 'Reply Message', width: '35%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'inTime', header: 'Date', width: '8%', filter: this.gridSettingService.getDefaultDateFilterable(), template: this.gridSettingService.getDateTemplateForRowData() },
            { field: 'msgChannel', header: 'Type', width: '7%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'mphone', header: 'Resend', width: '5%', isCustomAction: true, customActionIcon: 'fas fa-paper-plane', filter: this.gridSettingService.getFilterableNone() }
        ];
    };

    onResend(event) {
        this.confirmationService.confirm({
            message: 'Are you sure that you want to proceed?',
            header: 'Confirmation',
            icon: 'pi pi-exclamation-triangle',
            accept: () => {
                this.outboxService.resendMessage(event).pipe(first())
                    .subscribe(
                        data => {
                            this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Success! Message Resend Successfully' });                          
                            this.child.updateDataSource();
                        },
                        error => {
                            this.messageService.add({ severity: 'error', summary: 'Error', detail: error });
                        });
            },
            reject: () => {
                this.messageService.add({ severity: 'info', summary: 'Rejected', detail: 'You have rejected' });                
            }
        });
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
            { whichFeildName: 'To Date', whatValue: this.mfsUtilityService.renderDate(this.dateObj.toDate) }           
        ];
        if (this.dateObj.mphone) {
            this.auditTrailModel.inputFeildAndValue.push({ whichFeildName: 'Account No', whatValue: this.dateObj.mphone })
        }
        if (this.dateObj.messageBody) {
            this.auditTrailModel.inputFeildAndValue.push({ whichFeildName: 'Messege Body', whatValue: this.dateObj.messageBody })
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

}
