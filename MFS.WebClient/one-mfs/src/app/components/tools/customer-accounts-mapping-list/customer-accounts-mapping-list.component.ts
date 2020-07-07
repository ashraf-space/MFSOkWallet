
import { Component, OnInit, ViewChild} from '@angular/core';
import { GridSettingService } from 'src/app/services/grid-setting.service';
import { AuthenticationService } from 'src/app/shared/_services';
import { MfsSettingService } from 'src/app/services/mfs-setting.service';
import { MfsUtilityService } from 'src/app/services/mfs-utility.service';
import { GenericGridComponent } from 'src/app/shared/directives/generic-grid/generic-grid.component';
import { first } from 'rxjs/operators';
import { CustomerAccountMappingService } from '../../../services/tools/customer-account-mapping.service';

@Component({
    selector: 'app-customer-accounts-mapping-list',
    templateUrl: './customer-accounts-mapping-list.component.html',
    styleUrls: ['./customer-accounts-mapping-list.component.css']
})
export class CustomerAccountsMappingListComponent implements OnInit {
    mtCbsInfoModel: any = {};
    gridConfig: any;
    currentUserModel: any;
    showGrid: boolean = false;
    @ViewChild(GenericGridComponent) childGrid: GenericGridComponent;
    constructor(private gridSettingService: GridSettingService,
        private authService: AuthenticationService,
        private mfsSettingService: MfsSettingService,
        private mfsUtilityService: MfsUtilityService) {
        this.gridConfig = {};

        this.authService.currentUser.subscribe(x => {
            this.currentUserModel = x;
        });
    }

    ngOnInit() {
        this.initialiseGridConfig();
    }
    onSearch() {
        this.showGrid = true;
        this.gridConfig.dataSourcePath = this.mfsSettingService.transactionApiServer + '/CbsMappedAccount/GetMappedAccountInfoByMphone?mphone=' + this.mtCbsInfoModel.mblAcc;

        if (this.childGrid) {
            this.childGrid.updateDataSource();
        }      
    }
    initialiseGridConfig(): any {
        this.gridConfig.dataSource = [];

        this.gridConfig.dataSourcePath = this.mfsSettingService.transactionApiServer + '/CbsMappedAccount/GetMappedAccountInfoByMphone';

        this.gridConfig.autoUpdateDataSource = true;
        this.gridConfig.autoIndexing = true;
        //this.gridConfig.createStateUrl = 'customer-accounts-mapping/addoredit';
        this.gridConfig.gridName = "CBS Mapped Customer Information";
        this.gridConfig.gridIconClass = 'fas fa-thumbtack';

        this.gridConfig.hasEditState = true;
        this.gridConfig.entityField = 'mphone';
        //this.gridConfig.showUniversalFilter = true;

        this.gridConfig.columnList = [
            { field: 'MPHONE', header: 'Mobile No', width: '20%'},
            { field: 'CUSTID', header: 'Customer Id', width: '20%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'NAME', header: 'Customer Name', width: '25%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'ACCNO', header: 'Mapped Account No', width: '25%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'CLASS', header: 'class', width: '10%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'MOBNUM', header: 'Mobile Number', width: '15%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'NATIONID', header: 'National Id', width: '25%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'STATUS', header: 'Status', width: '25%', filter: this.gridSettingService.getDefaultFilterable() }
        ];
    };
}
