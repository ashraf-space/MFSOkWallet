import { Component, OnInit } from '@angular/core';
import { GridSettingService } from 'src/app/services/grid-setting.service';
import { MfsSettingService } from 'src/app/services/mfs-setting.service';
import { AuthenticationService } from 'src/app/shared/_services';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-company-list',
  templateUrl: './company-list.component.html',
  styleUrls: ['./company-list.component.css']
})
export class CompanyListComponent implements OnInit {

    gridConfig: any;
    ProgressSpinnerDlg: boolean = false;
    isRegistrationPermitted: boolean = false;

    constructor(private gridSettingService: GridSettingService, private mfsSettingService: MfsSettingService,
        private authService: AuthenticationService, private route: ActivatedRoute) {
        this.gridConfig = {};
    }


    ngOnInit() {
        this.isRegistrationPermitted = this.authService.checkRegisterPermissionAccess(this.route.snapshot.routeConfig.path);
        this.initialiseGridConfig();
        
    }

    initialiseGridConfig(): any {
        this.gridConfig.dataSource = [];
        this.gridConfig.gridName = ' Disbursement Company List';
        this.gridConfig.gridIconClass = 'fas fa-file-contract';

        this.gridConfig.dataSourcePath = this.mfsSettingService.transactionApiServer + '/Disbursement/GetDisbursementCompanyList'; //-- FOR DATA SOURCE PATH 

        this.gridConfig.autoUpdateDataSource = true; // --- FOR AUTO BINDING THE DATA AFTER DATA RETRIVAL FROM THE API SERVER
        //this.gridConfig.autoIndexing = true;  // FOR INDEXING THE DATA GRID WHICH CAN BE ACCESSED BY --INDEX--
        //this.gridConfig.showUniversalFilter = false; //-- ---- CAN TURN IT OFF ANY TIME IF NOT NEEDED

        this.gridConfig.createStateUrl = '/disbursement-company/addoredit/';
        this.gridConfig.hasEditState = true;
        this.gridConfig.entityField = 'companyId';

        this.gridConfig.columnList = [
            { field: 'companyId', header: 'ID', width: '5%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'companyName', header: 'Company Name', width: '20%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'enterprize_AccCode', header: 'Enterprise Acc Code', width: '10%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'bala_nce', header: 'Balance', width: '10%',style:'text-align:right', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'address', header: 'Address', width: '40%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'phone', header: 'Phone', width: '10%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'fax', header: 'Fax', width: '5%', filter: this.gridSettingService.getDefaultFilterable(), template: this.gridSettingService.getYesNoTemplateForRowData() }
            
        ];

        if (this.isRegistrationPermitted) {
            this.gridConfig.columnList.push(
                { field: 'companyId', header: 'Refund', width: '10%', isEditColumn: true, filter: this.gridSettingService.getFilterableNone() });
        }

    };

}
