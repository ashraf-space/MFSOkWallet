import { Component, OnInit, ViewChild } from '@angular/core';
import { GridSettingService } from '../../../../services/grid-setting.service';
import { CustomerService } from '../../../../services/distribution/customer.service';
import { AgentService } from '../../../../services/distribution';
import { MfsSettingService } from '../../../../services/mfs-setting.service';
import { AuthenticationService } from 'src/app/shared/_services';
import { GenericGridComponent } from 'src/app/shared/directives/generic-grid/generic-grid.component';
import { MessageService, MenuItem } from 'primeng/api';

@Component({
    selector: 'app-customer-list',
    templateUrl: './customer-list.component.html',
    styleUrls: ['./customer-list.component.css']
})
export class CustomerListComponent implements OnInit {
    gridConfig: any;
    currentUserModel: any = {};
    searchOptionType: any;
    regInfoModel: any = {};
    isAllowUser: boolean = false;
    @ViewChild(GenericGridComponent) child: GenericGridComponent;
    constructor(private customerService: CustomerService,
        private gridSettingService: GridSettingService,
        private agentService: AgentService,
        private mfsSettingService: MfsSettingService,
        private authService: AuthenticationService,
        private messageService: MessageService) {
        this.gridConfig = {};

        this.authService.currentUser.subscribe(x => {
            this.currentUserModel = x;
        });
    }

    ngOnInit() {
        this.initialiseGridConfig();
        if (this.currentUserModel.user.branchCode === '0000') {
            this.isAllowUser = true;
        }
        this.searchOptionType = [
            { label: 'All', value: 'A' },
            { label: 'Pending', value: 'P' }
        ];
    }

    initialiseGridConfig(): any {
        this.gridConfig.dataSource = [];
        this.gridConfig.gridName = 'Customer List';
        this.gridConfig.gridIconClass = 'fas fa-list';
        this.gridConfig.createStateUrl = 'customer/addoredit/';
        this.gridConfig.hasEditState = true;
        this.gridConfig.entityField = 'mphone';
        this.gridConfig.customFilterOptionPath = this.mfsSettingService.distributionApiServer + '/Kyc/GetFilteringListForDdl';
        this.gridConfig.dataSourcePath = this.mfsSettingService.distributionApiServer + '/Kyc/GetRegInfoListByCatIdBranchCode?BranchCode=' + this.currentUserModel.user.branchCode + '&CatId=C' + '&filterId=';
        this.gridConfig.autoUpdateDataSource = true;
        this.gridConfig.showUniversalFilter = false;
        this.gridConfig.detailsStateUrl = 'customer/details/';
         this.gridConfig.hasCustomContent = true;
        this.gridConfig.columnList = [
            { field: 'mphone', header: 'Customer AC No', width: '10%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'name', header: 'Name', width: '20%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'photoId', header: 'Photo Id No', width: '10%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'mphone', header: 'Details', width: '7%', isDetailsColumn: true, filter: this.gridSettingService.getFilterableNone() },
            ////{ field: 'mphone', header: 'Action', width: '10%', isEditColumn: true, filter: this.gridSettingService.getFilterableNone(), actionDisableParam: 'regStatus', disableValue: 'P' }
            { field: 'mphone', header: 'Edit', width: '10%', isEditColumn: true, filter: this.gridSettingService.getFilterableNone()}

        ];
    };

    onCustomerSearch() {
        if (this.regInfoModel.mphone && this.regInfoModel.mphone.length==11) {
            this.gridConfig.dataSourcePath = this.mfsSettingService.distributionApiServer + '/Kyc/GetCustomerByMphone?mPhone=' + this.regInfoModel.mphone + '&CatId=C';
            this.child.updateDataSource();
        }
        else {
            this.messageService.add({ severity: 'error', summary: 'Invalid Mobile No', detail: 'Please Input Valid Account', closable: true });
        }
        
    }

}
