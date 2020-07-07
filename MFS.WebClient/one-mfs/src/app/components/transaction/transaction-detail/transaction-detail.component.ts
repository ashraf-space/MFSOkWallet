import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { GridSettingService } from 'src/app/services/grid-setting.service';
import { MfsSettingService } from 'src/app/services/mfs-setting.service';
import { GenericGridComponent } from 'src/app/shared/directives/generic-grid/generic-grid.component';
import { TransactionDetailService } from 'src/app/services/transaction/transaction-detail.service';
import { first } from 'rxjs/operators';

@Component({
  selector: 'app-transaction-detail',
  templateUrl: './transaction-detail.component.html',
  styleUrls: ['./transaction-detail.component.css']
})
export class TransactionDetailComponent implements OnInit {

    gridConfig: any;
    @ViewChild(GenericGridComponent) child: GenericGridComponent;
    @Input() transactionModel: any;
    
    constructor(private gridSettingService: GridSettingService, private mfsSettingService: MfsSettingService, private transactionDetailService: TransactionDetailService) {
        this.gridConfig = {};
    }

    ngOnInit() {
        if (!this.transactionModel || !this.transactionModel.transNo) {           
            this.transactionModel = {};
        }
        
        this.initialiseGridConfig();
    }    

    onSearch() {        
        this.gridConfig.dataSourcePath = this.mfsSettingService.transactionApiServer + '/TransactionDetail/GetTransactionDetailL?transactionNumber=' + this.transactionModel.transNo;
        this.child.updateDataSource();
        this.getTransactionMasterByTransNo();
    }

    getTransactionMasterByTransNo(): any {
        this.transactionDetailService.getTransactionMasterByTransNo(this.transactionModel.transNo).pipe(first())
            .subscribe(
            data => {
                this.transactionModel = data;
                console.log(this.transactionModel);
                },
                error => {
                    console.log(error);
                });
    }

    initialiseGridConfig(): any {
        this.gridConfig.scrollHeight = 'calc(42vh - (89px + 1.25em))';
        this.gridConfig.dataSource = [];

        this.gridConfig.columnList = [
            { field: 'transDate', header: 'Date', width: '9%', template: this.gridSettingService.getDateTemplateForRowData() },
            { field: 'coaCode', header: 'GL Code', width: '9%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'accType', header: 'Account Type', width: '9%', filter: this.gridSettingService.getDefaultFilterable(), template: this.gridSettingService.getAccountTypesTemplateForRowData() },
            { field: 'drAmt', header: 'Debit', width: '9%', filter: this.gridSettingService.getDefaultNumberFilterable(0, 100000), template: this.gridSettingService.getMoneyTemplateForRowData() },
            { field: 'crAmt', header: 'Credit', width: '8%', filter: this.gridSettingService.getDefaultNumberFilterable(0, 100000), template: this.gridSettingService.getMoneyTemplateForRowData() },
            { field: 'shortName', header: 'GL Description', width: '20%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'particular', header: 'Event', width: '32%', filter: this.gridSettingService.getDefaultFilterable() }
        ];

        this.gridConfig.dataSourcePath = this.mfsSettingService.transactionApiServer + '/TransactionDetail/GetTransactionDetailL?transactionNumber=' + this.transactionModel.transNo;

        this.gridConfig.autoUpdateDataSource = true;        
        
        this.gridConfig.showCaption = false;
        this.gridConfig.showPaginator = false;

        this.gridConfig.footerTemplate = { captionIndex: 2 };
        //this.gridConfig.footerTemplate.columns = [
        //    { field: 'drAmt', action: 'sum' },
        //    { field: 'crAmt', action: 'sum' }
        //];

        this.gridConfig.footerTemplate.footerRow = { drAmt: 0, crAmt: 0 };

    };    

}
