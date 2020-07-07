import { Component, OnInit, Input } from '@angular/core';
import { OutboxService } from 'src/app/services/client/outbox.service';
import { GridSettingService } from 'src/app/services/grid-setting.service';
import { AuthenticationService } from 'src/app/shared/_services';
import { MfsSettingService } from 'src/app/services/mfs-setting.service';
import { MfsUtilityService } from 'src/app/services/mfs-utility.service';
import { GenericGridComponent } from 'src/app/shared/directives/generic-grid/generic-grid.component';
import { DistributorService, AgentService } from 'src/app/services/distribution';
import { Router, ActivatedRoute } from '@angular/router';
import { first } from 'rxjs/operators';
import { MessageService, ConfirmationService, Message } from 'primeng/api';
import { CustomerRequestService } from 'src/app/services/client/customer-request.service';
import { KycService } from '../../../services/distribution/kyc.service'
@Component({
    selector: 'app-client-work-tab',
    templateUrl: './client-work-tab.component.html',
    styleUrls: ['./client-work-tab.component.css']
})
export class ClientWorkTabComponent implements OnInit {
    @Input() entity: any;
    entityId: any;
    isDetailMode: boolean = false;
    isLoading: boolean = false;
    showCloseModal: boolean = false;
    showDormantModal: boolean = false;
    showEditModal: boolean = false;
    showGenerateRequestModal: boolean = false;
    isActionEligible: boolean = false;
    showLienModal: boolean = false;
    currentUserModel: any;
    closeStatus: any;
    dormantModel: any;
    dormantStatus: string;
    remarks: string;
    requestList: any;
    requestModel: any;
    actionList: any;
    searchModel: any;
    loading: boolean = false;
    mphone: any;
    childList: any;
    index: any;
    showSearch: boolean = true;
    showTabForWork: boolean = false;
    showTabForCustomerPortal: boolean = false;
    constructor(private outboxService: OutboxService, private gridSettingService: GridSettingService, private authService: AuthenticationService
        , private mfsSettingService: MfsSettingService, private mfsUtilityService: MfsUtilityService, private distributorService: DistributorService
        , private agentService: AgentService, private router: Router, private route: ActivatedRoute, private messageService: MessageService,
        private customerRequestService: CustomerRequestService, private confirmationService: ConfirmationService,
        private kycService: KycService) {
        this.dormantModel = {};
        this.requestModel = {};       
        this.requestList = this.customerRequestService.requestList;
        this.authService.currentUser.subscribe(x => {
            this.currentUserModel = x;
        });
        this.actionList = [
            { label: 'Open', value: 'O', icon: 'far fa-clock' },
            { label: 'Close', value: 'C', icon: 'fas fa-times' },
            { label: 'Resolved', value: 'Y', icon: 'fas fa-check' },
            { label: 'On Process', value: 'P', icon: 'fa fa-spinner' }
        ];
    }

    ngOnInit() {
        if (this.entity) {
            this.showSearch = false;
            this.showTabForCustomerPortal = true;
        }
        else {
            this.entity = {}
        }
    }
    ngOnChanges() {
        this.index = 0;
    }
    handleChange(event) {
        this.index = event.index;
        if (this.index === 3) {
            if (this.entity.mphone) {
                var dataSourcePath = this.mfsSettingService.distributionApiServer + '/Agent/GetAgentListByParent?code=' + this.entity.mphone + '&CatId=A';
            }
            else {
                var dataSourcePath = this.mfsSettingService.distributionApiServer + '/Agent/GetAgentListByParent?code=' + this.mphone + '&CatId=A';

            }
            this.getChildListByParent(dataSourcePath);
        }
        if (this.index === 4) {
            if (this.entity.mphone) {
                var dataSourcePath = this.mfsSettingService.distributionApiServer + '/Agent/GetAgentListByParent?code=' + this.entity.mphone + '&CatId=R';
            }
            else {
                var dataSourcePath = this.mfsSettingService.distributionApiServer + '/Agent/GetAgentListByParent?code=' + this.mphone + '&CatId=R';

            }
            this.getChildListByParent(dataSourcePath);
        }
    }
    getChildListByParent(dataSourcePath: string) {
        this.gridSettingService.getWorklistForGridDataSource(dataSourcePath).pipe()
            .subscribe(data => {
                this.childList = data;
                this.isLoading = false;
            });
    }

    onSearch() {
        this.loading = true;
        this.distributorService.GetDistributorByMphone(this.mphone).pipe(first())
            .subscribe(
                data => {
                    this.loading = false;
                    if (data) {
                        this.searchModel = {};
                        this.searchModel = data;
                        this.showTabForWork = true;
                        this.index = 0;
                        this.getClientDistLocationInfo();
                        this.getPhotoIdTypeByCode();
                        this.getBranchNameByCode();
                    }
                    else {
                        this.searchModel = null;
                        this.messageService.add({ severity: 'error', summary: 'Warning! ', detail: this.mphone + ' could not be found' });
                    }
                },
                error => {
                    console.log(error);
                });
    }
    getBranchNameByCode() {
        this.loading = true;
        this.kycService.getBranchNameByCode(this.searchModel.branchCode).pipe(first())
            .subscribe(
                data => {
                    this.loading = false;
                    if (data) {
                        this.searchModel.branchName = data.value;
                    }
                    else {
                        this.searchModel = {};
                    }
                },
                error => {
                    this.loading = false;
                    console.log(error);
                });
        this.loading = false;
    }
    getPhotoIdTypeByCode() {
        this.loading = true;
        this.kycService.getPhotoIdTypeByCode(this.searchModel.photoIdTypeCode).pipe(first())
            .subscribe(
                data => {
                    this.loading = false;
                    if (data) {
                        this.searchModel.photoIdType = data.value;
                    }

                },
                error => {
                    this.loading = false;
                    console.log(error);
                });
        this.loading = false;
    }
    getClientDistLocationInfo() {
        this.loading = true;
        this.kycService.getClientDistLocationInfo(this.searchModel.distCode, this.searchModel.locationCode).pipe(first())
            .subscribe(
                data => {
                    this.loading = false;
                    if (data) {
                        this.searchModel.division = data.division;
                        this.searchModel.district = data.district;
                        this.searchModel.thana = data.thana;
                        this.searchModel.region = data.region;
                        this.searchModel.area = data.area;
                        this.searchModel.territory = data.territory;
                    }

                },
                error => {
                    this.loading = false;
                    console.log(error);
                });
        this.loading = false;
    }
}
