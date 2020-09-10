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
    @Input() isCustomerCare: boolean = false;
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
    disableButton: boolean = false;
    blackListed: any;
    isHidefromCustomerCare: boolean = false;
    showPinResetModal: boolean = false;
    isKycExecutive: boolean = false;
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
        if (!this.model) {
            this.isLoading = true;
            this.model = {};
            this.entityId = this.route.snapshot.paramMap.get('id');
            this.isDetailMode = true;
            this.checkIsKycSales();
            this.getProfileDetails(this.entityId);
        }
        else {
            this.dormantModel.catId = this.model.catId;
            this.dormantModel.mphone = this.model.mphone;
            this.dormantStatus = this.model.status == 'D' ? 'Release' : 'Make';
            this.closeStatus = this.model.status == 'C' ? 'Open' : 'Close';
            this.blackListed = this.model.blackList == 'Y' ? 'No' : 'Yes';
            this.getBalanceInfoByMphone(this.model.mphone);
        }
        if (this.isCustomerCare) {
            this.isHidefromCustomerCare = true;
        }
    }
    ngOnChanges() {
        if (this.model) {
            this.dormantModel.catId = this.model.catId;
            this.dormantModel.mphone = this.model.mphone;
            this.closeStatus = this.model.status == 'C' ? 'Open' : 'Close';
            this.dormantStatus = this.model.status == 'D' ? 'Release' : 'Make';
            this.blackListed = this.model.blackList == 'Y' ? 'No' : 'Yes';
            this.checkIsKycSales();
        }
    }

    checkIsKycSales() {
        if (this.currentUserModel.user.role_Name.trim() === 'KYC Sales Maker'.trim() || this.currentUserModel.user.role_Name.trim() === 'KYC Sales Checker'.trim() ||
            this.currentUserModel.user.role_Name.trim() === 'Sales Executive'.trim()) {
            this.isKycExecutive = true;
        }
        else {
            this.isKycExecutive = false;
        }
    }
    blackListClient() {
        this.disableButton = true;
        this.model.updateBy = this.currentUserModel.user.username;
        this.kycService.blackListClient(this.model, this.remarks).pipe(first())
            .subscribe(
                data => {
                    if (data === 200) {
                        this.messageService.add({ severity: 'success', summary: 'Success', sticky: true, detail: 'Action Performed Successfully: ' + this.model.mphone });
                    }
                    else {
                        this.messageService.add({ severity: 'error', summary: 'Error', sticky: true, detail: 'Action Performed Failed in: ' + this.model.mphone });
                    }
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
                        this.getBalanceInfoByMphone(entity);
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
    getBalanceInfoByMphone(entity) {
        this.isLoading = true;
        this.kycService.getBalanceInfoByMphone(entity).pipe(first())
            .subscribe(
                data => {
                    this.isLoading = false;
                    if (data) {
                        this.model.balanceM = data.balanceM;
                        this.model.lienM = data.lienM;
                        this.model.balanceC = data.balanceC;
                        this.model.lienC = data.lienC;
                    }
                },
                error => {
                    this.isLoading = false;
                    console.log(error);
                });
        this.isLoading = false;
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
                //this.confirmationService.confirm({
                //    message: 'Are you sure that you want to reset pin?',
                //    header: 'Confirmation',
                //    icon: 'pi pi-exclamation-triangle',
                //    accept: () => {
                //        this.onPinReset();
                //    },
                //    reject: () => {
                //        this.messageService.add({ severity: 'info', summary: 'Rejected', detail: 'Action Rejected' });
                //    }
                //});
                this.model.updateBy = this.currentUserModel.user.username;
                this.showPinResetModal = true;
                break;
            case 'release-bind':
                this.confirmationService.confirm({
                    message: 'Are you sure that you want release bind the Device?',
                    header: 'Confirmation',
                    icon: 'pi pi-exclamation-triangle',
                    accept: () => {
                        this.onReleaseBindDevice();
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
    onReleaseBindDevice() {      
        this.model.updateBy = this.currentUserModel.user.username;
        this.kycService.onReleaseBindDevice(this.model).pipe(first())
            .subscribe(
                data => {
                    if (data === 200) {
                        this.messageService.add({ severity: 'success', summary: 'Success', sticky: true, detail: 'Release Performed Successfully in: ' + this.model.mphone });
                    }
                    else {
                        this.messageService.add({ severity: 'error', summary: 'Error', sticky: true, detail: 'Release Performed Failed in: ' + this.model.mphone });
                    }
                    if (!this.model) {
                        this.isLoading = true;
                        this.getProfileDetails(this.entityId);
                    }
                    else {
                        this.isLoading = true;
                        this.getProfileDetails(this.model.mphone);
                    }                    
                },
                error => {
                    console.log(error);
                });
    }
    closeClient() {
        this.disableButton = true;
        this.model.updateBy = this.currentUserModel.user.username;
        this.kycService.clientClose(this.model, this.remarks).pipe(first())
            .subscribe(
                data => {
                    if (data === 200) {
                        this.messageService.add({ severity: 'success', summary: 'Success', sticky: true, detail: 'Action Performed Successfully in: ' + this.model.mphone });
                    }
                    else {
                        this.messageService.add({ severity: 'error', summary: 'Error', sticky: true, detail: 'Action Performed Failed in: ' + this.model.mphone });
                    }
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
        this.model.updateBy = this.currentUserModel.user.username;
        this.kycService.addRemoveLien(this.model, this.remarks).pipe(first())
            .subscribe(
                data => {
                    if (data) {
                        this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Action Performed Successfully' });
                    }
                    else {
                        this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Action Performed Failed' });
                    }

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
                    if (data === 200) {
                        this.isLoading = false;
                        this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Action Performed Successfully in: ' + this.dormantModel.mphone });
                    }
                    else {
                        this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Action Performed Failed in: ' + this.dormantModel.mphone });
                    }

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
                    this.isLoading = false;
                },
                error => {
                    console.log(error);
                });
    }

    addNewRequest() {
        this.disableButton = true;
        this.model.updateBy = this.currentUserModel.user.username;
        this.customerRequestService.save(this.requestModel).pipe(first())
            .subscribe(
                data => {
                    if (data) {
                        this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Success! Request Generated Successfully' });
                        this.disableButton = false;
                        this.showGenerateRequestModal = false;
                    }
                    else {
                        this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Action Performed Failed' });
                    }
                },
                error => {
                    this.messageService.add({ severity: 'error', summary: 'Error', detail: error });
                });
    }

    onPinReset() {
        this.isLoading = true;
        this.disableButton = true;
        this.model.updateBy = this.currentUserModel.user.username;
        this.model.remarks = this.remarks;
        this.distributorService.pinReset(this.model).pipe(first())
            .subscribe(
                data => {
                    if (data === 200) {
                        this.isLoading = false;
                        this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Success! Pin Reset Successfully' });
                    }
                    else {
                        this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Action Performed Failed' });
                    }
                    if (!this.model) {
                        this.isLoading = true;
                        this.getProfileDetails(this.entityId);
                    }
                    else {
                        this.isLoading = true;
                        this.getProfileDetails(this.model.mphone);
                    }
                    this.disableButton = false;
                    this.showPinResetModal = false;
                    this.isLoading = false;
                },
                error => {
                    this.messageService.add({ severity: 'error', summary: 'Error', detail: error });
                    this.isLoading = false;
                });
    }

    accountUnlock() {
        this.model.updateBy = this.currentUserModel.user.username;
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
