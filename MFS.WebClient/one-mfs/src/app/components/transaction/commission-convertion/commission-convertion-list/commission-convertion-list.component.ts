import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { GridSettingService } from 'src/app/services/grid-setting.service';
import { MfsSettingService } from 'src/app/services/mfs-setting.service';
import { AuthenticationService } from 'src/app/shared/_services';

@Component({
  selector: 'app-commission-convertion-list',
  templateUrl: './commission-convertion-list.component.html',
  styleUrls: ['./commission-convertion-list.component.css']
})
export class CommissionConvertionListComponent implements OnInit {
    gridConfig: any;
    ProgressSpinnerDlg: boolean = false;
    currentUserModel: any = {};
    isRegistrationPermitted: boolean = false;
    transAmtLimit: any;

    constructor(private gridSettingService: GridSettingService, private authService: AuthenticationService
        , private mfsSettingService: MfsSettingService, private route: ActivatedRoute) {
        this.gridConfig = {};

        this.authService.currentUser.subscribe(x => {
            this.currentUserModel = x;
        });
    }

    ngOnInit() {
        this.transAmtLimit = this.currentUserModel.user.tranAmtLimit;
        this.isRegistrationPermitted = this.authService.checkRegisterPermissionAccess(this.route.snapshot.routeConfig.path);
        this.initialiseGridConfig();
    }

    initialiseGridConfig(): any {
        this.gridConfig.dataSource = [];
        this.gridConfig.columnList = [];

        this.gridConfig.dataSourcePath = this.mfsSettingService.transactionApiServer + '/CommissionConversion/GetCommissionConversionList?isRegistrationPermitted=' + this.isRegistrationPermitted + '&transAmtLimit=' + this.transAmtLimit;
        this.gridConfig.autoUpdateDataSource = true; // --- FOR AUTO BINDING THE DATA AFTER DATA RETRIVAL FROM THE API SERVER
        this.gridConfig.autoIndexing = true;

        this.gridConfig.gridName = "Commission Conversion list";
        this.gridConfig.gridIconClass = 'fas fa-list';
        this.gridConfig.createStateUrl = '/commission-convertion/addoredit/';
        this.gridConfig.hasEditState = true;
        this.gridConfig.entityField = 'transNo';

        this.gridConfig.detailsStateUrl = 'distributor-deposit/details/';

        this.gridConfig.columnList = [
            { field: 'transNo', header: 'Transaction No', width: '15%' },
            { field: 'mphone', header: 'A/C No', width: '15%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'amount', header: 'Amount', width: '10%', filter: this.gridSettingService.getDefaultFilterable(), template: this.gridSettingService.getMoneyTemplateForRowData() },
            { field: 'createDate', header: 'Transaction Date', width: '10%', filter: this.gridSettingService.getDefaultFilterable(), template: this.gridSettingService.getDateTemplateForRowData() },
            { field: 'createUser', header: 'Create User', width: '10%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'status', header: 'Status', width: '15%', filter: this.gridSettingService.getDefaultFilterable(), template: this.gridSettingService.getFinancialStatusTemplateForRowData() }
        ];

        if (this.isRegistrationPermitted) {
            this.gridConfig.columnList.push(
                { field: 'transNo', header: 'Action', width: '10%', isEditColumn: true, filter: this.gridSettingService.getFilterableNone(), isReciprocal: true, actionDisableParam: 'status', disableValue: null });
        }
        else {
            this.gridConfig.columnList.push(
                { field: 'transNo', header: 'Action', width: '10%', isEditColumn: true, filter: this.gridSettingService.getFilterableNone(), isReciprocal: true, actionDisableParam: 'status', disableValue: 'M' });
        }
    };



}
