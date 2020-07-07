import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { OutboxService } from 'src/app/services/client/outbox.service';
import { GridSettingService } from 'src/app/services/grid-setting.service';
import { AuthenticationService } from 'src/app/shared/_services';
import { MfsSettingService } from 'src/app/services/mfs-setting.service';
import { MfsUtilityService } from 'src/app/services/mfs-utility.service';
import { GenericGridComponent } from 'src/app/shared/directives/generic-grid/generic-grid.component';

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

    dateObj: any = {};    
    messageTypeList: any;

    constructor(private outboxService: OutboxService, private gridSettingService: GridSettingService, private authService: AuthenticationService
        , private mfsSettingService: MfsSettingService, private mfsUtilityService: MfsUtilityService) {
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
        
        this.child.updateDataSource();
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
    }
;
}
