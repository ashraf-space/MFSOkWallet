import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { GridSettingService } from 'src/app/services/grid-setting.service';
import { AuthenticationService } from 'src/app/shared/_services';
import { MfsSettingService } from 'src/app/services/mfs-setting.service';
import { MfsUtilityService } from 'src/app/services/mfs-utility.service';
import { GenericGridComponent } from 'src/app/shared/directives/generic-grid/generic-grid.component';
import { TransactionDetailComponent } from '../transaction-detail/transaction-detail.component';

@Component({
  selector: 'app-transaction-master',
  templateUrl: './transaction-master.component.html',
  styleUrls: ['./transaction-master.component.css']
})
export class TransactionMasterComponent implements OnInit {

    gridConfig: any;
    @ViewChild(GenericGridComponent) child: GenericGridComponent;
    @ViewChild(TransactionDetailComponent) transactionDetail: TransactionDetailComponent;
    @Input() mPhone: string;

    dateObj: any = {};
    showModal: boolean = false;

    detailObj: any = {};

    constructor(private gridSettingService: GridSettingService, private authService: AuthenticationService
        , private mfsSettingService: MfsSettingService, private mfsUtilityService: MfsUtilityService) {
        this.gridConfig = {};
    }

    ngOnInit() {
        this.dateObj.toDate = this.mfsUtilityService.getFullDateByMonthParam(0, 1);
        this.dateObj.fromDate = this.mfsUtilityService.getFullDateByMonthParam(0, 1);
        this.initialiseGridConfig();
    }

    onDateChange() {
        if (this.dateObj.mPhone && this.dateObj.mPhone != '') {
            this.gridConfig.dataSourcePath = this.mfsSettingService.transactionApiServer + '/TransactionMaster/GetTransactionMasterList?fromDate=' + this.mfsUtilityService.renderDate(this.dateObj.fromDate, true) +
                '&ToDate=' + this.mfsUtilityService.renderDate(this.dateObj.toDate, true) + '&mPhone=' + this.dateObj.mPhone;
        }
        else {
            this.gridConfig.dataSourcePath = this.mfsSettingService.transactionApiServer + '/TransactionMaster/GetTransactionMasterList?fromDate=' + this.mfsUtilityService.renderDate(this.dateObj.fromDate, true) +
                '&ToDate=' + this.mfsUtilityService.renderDate(this.dateObj.toDate, true);
        }

        this.child.updateDataSource();
    }
   
    ngOnChanges() {
        if (this.mPhone) {
            this.initialiseGridConfig();
        }
    }

    initialiseGridConfig(): any {
        this.gridConfig.dataSource = [];

        this.gridConfig.columnList = [
            { field: 'transDate', header: 'Date', width: '9%', filter: this.gridSettingService.getDefaultDateFilterable(), template: this.gridSettingService.getDateTemplateForRowData() },            
            { field: 'transFrom', header: 'From', width: '8%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'transTo', header: 'To', width: '8%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'payAmt', header: 'Amount ( SC + Amt )', width: '9%', filter: this.gridSettingService.getDefaultNumberFilterable(0,100000), template: this.gridSettingService.getMoneyTemplateForRowData() },
            { field: 'schargeAmt', header: 'Service Charge', width: '8%', filter: this.gridSettingService.getDefaultFilterable(), template: this.gridSettingService.getMoneyTemplateForRowData()},
            { field: 'particular', header: 'Event', width: '15%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'billno', header: 'Biller Id', width: '12%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'gateway', header: 'Gateway', width: '7%', filter: this.gridSettingService.getDefaultFilterable() }
        ];

        if (this.mPhone) {
            this.dateObj.mPhone = this.mPhone;
            this.dateObj.fromDate = this.mfsUtilityService.getFullDateByMonthParam(0, 1);
            this.gridConfig.columnList.push({ field: 'transNo', header: 'Detail', width: '5%', isCustomAction: true, customActionIcon: 'fas fa-info-circle', filter: this.gridSettingService.getFilterableNone() });
            this.gridConfig.dataSourcePath = this.mfsSettingService.transactionApiServer + '/TransactionMaster/GetTransactionMasterList?mPhone=' + this.dateObj.mPhone + '&fromDate=' + this.mfsUtilityService.renderDate(this.dateObj.fromDate, true);
        }
        else {
            this.gridConfig.columnList.push({ field: 'transNo', header: 'Transaction #', width: '11%', filter: this.gridSettingService.getDefaultFilterable() },
                { field: 'hotkey', header: 'Hot Key', width: '4%', filter: this.gridSettingService.getDefaultFilterable() },                
                { field: 'transNo', header: 'Detail', width: '5%', isCustomAction: true, customActionIcon: 'fas fa-info-circle', filter: this.gridSettingService.getFilterableNone() });
            this.gridConfig.dataSourcePath = this.mfsSettingService.transactionApiServer + '/TransactionMaster/GetTransactionMasterList';
        }

        this.gridConfig.autoUpdateDataSource = true;        

        this.gridConfig.gridName = "Transaction";
        this.gridConfig.gridIconClass = 'fas fa-receipt';

        this.gridConfig.entityField = 'mphone';
        this.gridConfig.hasCustomContent = true;
        this.gridConfig.showUniversalFilter = false;
    };

    ondetail(event) {
        this.detailObj = event;        
        this.showModal = true;
    }

}
