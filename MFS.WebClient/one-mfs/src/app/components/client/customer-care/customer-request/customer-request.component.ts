import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { OutboxService } from 'src/app/services/client/outbox.service';
import { GridSettingService } from 'src/app/services/grid-setting.service';
import { AuthenticationService, AuditTrailService } from 'src/app/shared/_services';
import { MfsSettingService } from 'src/app/services/mfs-setting.service';
import { MfsUtilityService } from 'src/app/services/mfs-utility.service';
import { GenericGridComponent } from 'src/app/shared/directives/generic-grid/generic-grid.component';
import { CustomerRequestService } from 'src/app/services/client/customer-request.service';
import { first } from 'rxjs/operators';
import { MessageService, MenuItem } from 'primeng/api';

@Component({
  selector: 'app-customer-request',
  templateUrl: './customer-request.component.html',
  styleUrls: ['./customer-request.component.css']
})
export class CustomerRequestComponent implements OnInit {

    gridConfig: any;
    searchObj: any;
    @ViewChild(GenericGridComponent) child: GenericGridComponent;
    actionColumn: any;
    showRequestAction: boolean = false;
    actionList: any;
    model: any;
    currentUserModel: any = {};
    auditTrailModel: any = {};
    constructor(private customerRequestService: CustomerRequestService, private gridSettingService: GridSettingService, private authService: AuthenticationService
        , private mfsSettingService: MfsSettingService, private mfsUtilityService: MfsUtilityService, private messageService: MessageService, private auditTrailService: AuditTrailService) {
        this.gridConfig = {};
        this.searchObj = {};
        this.model = {};
        this.searchObj.statusList = [{ label: 'Pending', value: 'Pending' }, { label: 'Resolved', value: 'Resolved' }, { label: 'On Process', value: 'OnProcess'}];
        this.actionColumn = { field: 'mphone', header: 'Action', width: '7%', isCustomAction: true, customActionIcon: 'far fa-bell', filter: this.gridSettingService.getFilterableNone() };
        this.authService.currentUser.subscribe(x => {
            this.currentUserModel = x;
        });
    }

    @Input() mPhone: string;
    ngOnChanges() {
        if (this.mPhone) {
            this.initialiseGridConfig();
        }
    }
    ngOnInit() {
        this.searchObj.selectedStatus = "Pending";
        this.initialiseGridConfig();
        this.actionList = [
            { label: 'Open', value: 'O', icon: 'far fa-clock' },
            { label: 'Close', value: 'C', icon: 'fas fa-times' },
            { label: 'Resolved', value: 'Y', icon: 'fas fa-check' },
            { label: 'On Process', value: 'P', icon: 'fa fa-spinner' }
        ];        
    }

    initialiseGridConfig(): any {
        this.gridConfig.dataSource = [];

        this.gridConfig.columnList = [
            { field: 'reqDate', header: 'Request Date', width: '12%', filter: this.gridSettingService.getDefaultDateFilterable(), template: this.gridSettingService.getDateTemplateForRowData() },
            { field: 'remarks', header: 'Remarks', width: '20%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'request', header: 'Request', width: '20%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'mphone', header: 'A/C #', width: '10%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'status', header: 'Status', width: '12%', filter: this.gridSettingService.getDefaultFilterable(), template: this.gridSettingService.getOpenCloseStatusTemplateForRowData() }
        ];

        this.gridConfig.columnList.push(this.actionColumn);

        if (this.mPhone) {            
            this.searchObj.mphone = this.mPhone;
            this.gridConfig.dataSourcePath = this.mfsSettingService.clientApiServer + '/CustomerRequest/GetCustomerRequestHistory?status=' + this.searchObj.selectedStatus + '&mphone=' + this.searchObj.mphone;
        }
        else {           
            this.gridConfig.dataSourcePath = this.mfsSettingService.clientApiServer + '/CustomerRequest/GetCustomerRequestHistory?status=' + this.searchObj.selectedStatus;
        }

        this.gridConfig.autoUpdateDataSource = true;
        
        this.gridConfig.gridName = "Request List";
        this.gridConfig.gridIconClass = 'fas fa-phone-volume';

        this.gridConfig.entityField = 'mphone';
        this.gridConfig.hasCustomContent = true;
        this.gridConfig.showUniversalFilter = false;
    }

    onSearch() {
        this.gridConfig.dataSourcePath = this.searchObj.mphone != null ? this.mfsSettingService.clientApiServer + '/CustomerRequest/GetCustomerRequestHistory?status=' + this.searchObj.selectedStatus + '&mphone=' + this.searchObj.mphone : 
            this.mfsSettingService.clientApiServer + '/CustomerRequest/GetCustomerRequestHistory?status=' + this.searchObj.selectedStatus;
        this.insertDataToAuditTrail();
        this.child.updateDataSource();
        this.addRemoveActionColumn();
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
        this.auditTrailModel.inputFeildAndValue = [{ whichFeildName: 'selectedStatus', whatValue: this.searchObj.selectedStatus }];
        if (this.searchObj.mphone) {
            this.auditTrailModel.inputFeildAndValue.push({ whichFeildName: 'mphone', whatValue: this.searchObj.mphone })
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
    onAction(event) {
        console.log(event);
        this.model = event;
        this.showRequestAction = true;
        this.model.prev_status = this.model.status;
    }

    confirmRequestAction() {
        this.model.handledBy = this.currentUserModel.user.username;
        this.customerRequestService.save(this.model).pipe(first())
            .subscribe(
                data => {
                    this.messageService.add({ severity: 'success', summary: 'Update successfully', detail: 'Success! Customer Reqyest History Updated Successfully' });
                    this.showRequestAction = false;
                    this.child.updateDataSource();
                },
                error => {
                    console.log(error);
                    this.messageService.add({ severity: 'error', summary: 'Erros Occured', detail: error, closable: false });
                });
    }

    addRemoveActionColumn() {
        if (this.searchObj.selectedStatus == 'Pending') {
            if (this.gridConfig.columnList.length == 5) {
                this.gridConfig.columnList.push(this.actionColumn);
            }
        }
        else {
            if (this.gridConfig.columnList.length != 5) {
                this.gridConfig.columnList.pop();
            }
        }

    }
}
