import { Component, OnInit, ViewChild, Input, Output } from '@angular/core';
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
    selector: 'app-client-profile',
    templateUrl: './client-profile.component.html',
    styleUrls: ['./client-profile.component.css'],
    providers: [ConfirmationService]
})
export class ClientProfileComponent implements OnInit {

    @Input() model: any;
    entityId: any;
    isDetailMode: boolean = false;
    isEditAllow: boolean = false;
    isLoading: boolean = false;
    showCloseModal: boolean = false;
    showBlackModal: boolean = false;
    showDormantModal: boolean = false;
    showEditModal: boolean = false;
    showGenerateRequestModal: boolean = false;
    isActionEligible: boolean = false;
    showLienModal: boolean = false;
    currentUserModel: any;
    closeStatus: string;
    dormantModel: any;
    dormantStatus: string;
    remarks: string;
    requestList: any;
    requestModel: any;
    actionList: any;
    disableButton: boolean = true;
    blackListed: any;
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
            { label: 'On Process', value: 'P', icon:'fa fa-spinner'}
        ];
    }

    ngOnInit() {
        if (!this.model) {
            this.isLoading = true;
            this.model = {};
            this.entityId = this.route.snapshot.paramMap.get('id');
            this.isDetailMode = true;
            this.getProfileDetails(this.entityId);
        }
        else {
            this.dormantModel.catId = this.model.catId;
            this.dormantModel.mphone = this.model.mphone;
            this.dormantStatus = this.model.status == 'D' ? 'Revoke' : 'Invoke';
            this.closeStatus = this.model.status == 'C' ? 'Open' : 'Close';
            this.blackListed = this.model.blackList == 'Y' ? 'No': 'Yes';
        }
    }
    ngOnChanges() { 
        if (this.model) {
            this.dormantModel.catId = this.model.catId;
            this.dormantModel.mphone = this.model.mphone;
            this.closeStatus = this.model.status == 'C' ? 'Open' : 'Close';
            this.dormantStatus = this.model.status == 'D' ? 'Revoke' : 'Invoke';
            this.blackListed = this.model.blackList == 'Y' ? 'No' : 'Yes';
        }
    }
    blackListClient() {
        this.disableButton = true;
        this.kycService.blackListClient(this.model, this.remarks).pipe(first())
            .subscribe(
                data => {
                    this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Action Performed Successfully' });
                    if (!this.model) {
                        this.isLoading = true;
                        this.getProfileDetails(this.entityId);
                    }
                    else {
                        this.isLoading = true;
                        this.getProfileDetails(this.model.mphone);
                    }
                    this.disableButton = false;
                    this.showBlackModal = false;
                },
                error => {
                    console.log(error);
                });
    }
    async getProfileDetails(entity) {
        this.distributorService.GetDistributorByMphone(entity).pipe(first())
            .subscribe(
                data => {
                    if (data) {
                        this.model = data;
                        this.getClientDistLocationInfo();
                        this.getPhotoIdTypeByCode();
                        this.getBranchNameByCode();
                        this.isLoading = false;
                        this.dormantStatus = data.status == 'D' ? 'Revoke' : 'Invoke';
                        this.closeStatus = data.status == 'C' ? 'Open' : 'Close';
                        this.dormantStatus = data.status == 'D' ? 'Revoke' : 'Invoke';
                        this.blackListed = this.model.blackList == 'Y' ? 'No' : 'Yes';
                        this.dormantModel.catId = data.catId;
                        this.dormantModel.mphone = data.mphone;
                    }                   
                },
                error => {
                    console.log(error);
                });
    }
    getBranchNameByCode() {
        this.isLoading = true;
        this.kycService.getBranchNameByCode(this.model.branchCode).pipe(first())
            .subscribe(
                data => {
                    this.isLoading = false;
                    if (data) {
                        this.model.branchName = data.value;
                    }                   
                },
                error => {
                    this.isLoading = false;
                    console.log(error);
                });
        this.isLoading = false;
    }
    getPhotoIdTypeByCode() {
        this.isLoading = true;
        this.kycService.getPhotoIdTypeByCode(this.model.photoIdTypeCode).pipe(first())
            .subscribe(
                data => {
                    this.isLoading = false;
                    if (data) {
                        this.model.photoIdType = data.value;
                    }                    
                },
                error => {
                    this.isLoading = false;
                    console.log(error);
                });
        this.isLoading = false;
    }
    getClientDistLocationInfo() {
        this.isLoading = true;
        this.kycService.getClientDistLocationInfo(this.model.distCode, this.model.locationCode).pipe(first())
            .subscribe(
                data => {
                    this.isLoading = false;
                    if (data) {
                        this.model.division = data.division;
                        this.model.district = data.district;
                        this.model.thana = data.thana;
                        this.model.region = data.region;
                        this.model.area = data.area;
                        this.model.territory = data.territory;
                    }                    
                },
                error => {
                    this.isLoading = false;
                    console.log(error);
                });
        this.isLoading = false;
    }
    goBack() {
        window.history.back();
    }

    openModal(modal) {
        this.remarks = "";
        switch (modal) {
            case 'dormant':
                this.dormantModel._ActionBy = this.currentUserModel.user.username;
                this.showDormantModal = true;
                break;
            case 'edit':
                this.showEditModal = true;
                break;
            case 'generate-request':
                this.requestModel = {};
                this.requestModel.mphone = this.model.mphone;
                this.requestModel.checkedBy = this.currentUserModel.user.username;
                this.requestModel.status = 'O';
                this.showGenerateRequestModal = true;
                break;
            case 'pin-reset':
                this.confirmationService.confirm({
                    message: 'Are you sure that you want to reset pin?',
                    header: 'Confirmation',
                    icon: 'pi pi-exclamation-triangle',
                    accept: () => {
                        this.onPinReset();
                    },
                    reject: () => {
                        this.messageService.add({ severity: 'info', summary: 'Rejected', detail: 'Action Rejected' });
                    }
                });
                break;
            case 'unlock':
                this.confirmationService.confirm({
                    message: 'Are you sure that you want to unlock this user?',
                    header: 'Confirmation',
                    icon: 'pi pi-exclamation-triangle',
                    accept: () => {
                        this.accountUnlock();
                    },
                    reject: () => {
                        this.messageService.add({ severity: 'info', summary: 'Rejected', detail: 'Action Rejected' });
                    }
                });
                break;
            case 'lien':                               
                this.model.updateBy = this.currentUserModel.user.username;
                this.showLienModal = true;
                break;
            case 'close':               
                this.model.updateBy = this.currentUserModel.user.username;
                this.showCloseModal = true;
                break;
            case 'black':
                this.model.updateBy = this.currentUserModel.user.username;
                this.showBlackModal = true;
                break;
            default:
        }
    }
    closeClient() {
        this.disableButton = true;
        this.kycService.clientClose(this.model, this.remarks).pipe(first())
            .subscribe(
                data => {
                    this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Close Successfully' });
                    if (!this.model) {
                        this.isLoading = true;
                        this.getProfileDetails(this.entityId);
                    }
                    else {
                        this.isLoading = true;
                        this.getProfileDetails(this.model.mphone);
                    }
                    this.disableButton = false;                  
                    this.showCloseModal = false;
                },
                error => {
                    console.log(error);
                });
    }
    addRemoveLien() {
        this.kycService.addRemoveLien(this.model, this.remarks).pipe(first())
            .subscribe(
                data => {
                    this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Operation Successfully' });
                    if (!this.model) {
                        this.isLoading = true;
                        this.getProfileDetails(this.entityId);
                    }
                    else {
                        this.isLoading = true;
                        this.getProfileDetails(this.model.mphone);
                    }
                    this.showLienModal = false;
                },
                error => {
                    console.log(error);
                });
    }
    addRemoveDormant() {
        this.disableButton = true;
        this.distributorService.addRemoveDormant(this.dormantModel, this.model.status, this.remarks).pipe(first())        
            .subscribe(
                data => {
                    this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Success! Dormant Invoke or Revoked Successfully' });
                    
                    if (!this.model) {
                        this.isLoading = true;
                        this.getProfileDetails(this.entityId);
                    }
                    else {
                        this.isLoading = true;
                        this.getProfileDetails(this.model.mphone);
                    }
                    this.disableButton = false;
                    this.showDormantModal = false;
                },
                error => {
                    console.log(error);
                });
    }

    addNewRequest() {
        this.disableButton = true;
        this.customerRequestService.save(this.requestModel).pipe(first())
            .subscribe(
                data => {
                    this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Success! Request Generated Successfully' });
                    this.disableButton = false;
                    this.showGenerateRequestModal = false;
                },
                error => {
                    this.messageService.add({ severity: 'error', summary: 'Error', detail: error });
                });
    }

    onPinReset() {
        this.distributorService.pinReset(this.model).pipe(first())
            .subscribe(
                data => {
                    this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Success! Pin Reset Successfully' });
                },
                error => {
                    this.messageService.add({ severity: 'error', summary: 'Error', detail: error });
                });
    }

    accountUnlock() {
        this.distributorService.accountUnlock(this.model).pipe(first())
            .subscribe(
                data => {
                    this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Success! Account Unlocked Successfully' });
                },
                error => {
                    this.messageService.add({ severity: 'error', summary: 'Error', detail: error });
                });
    }
}
