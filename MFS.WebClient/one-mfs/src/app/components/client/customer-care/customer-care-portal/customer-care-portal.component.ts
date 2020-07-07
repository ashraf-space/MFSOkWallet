import { Component, OnInit, ViewChild } from '@angular/core';
import { OutboxService } from 'src/app/services/client/outbox.service';
import { GridSettingService } from 'src/app/services/grid-setting.service';
import { AuthenticationService } from 'src/app/shared/_services';
import { MfsSettingService } from 'src/app/services/mfs-setting.service';
import { MfsUtilityService } from 'src/app/services/mfs-utility.service';
import { GenericGridComponent } from 'src/app/shared/directives/generic-grid/generic-grid.component';
import { MenuItem } from 'primeng/api';
import { DistributorService } from 'src/app/services/distribution';
import { first } from 'rxjs/operators';
import { MessageService } from 'primeng/api';


@Component({
  selector: 'app-customer-care-portal',
  templateUrl: './customer-care-portal.component.html',
  styleUrls: ['./customer-care-portal.component.css']
})
export class CustomerCarePortalComponent implements OnInit {
    caseList: MenuItem[];
    activeCase: MenuItem;

    activeCaseDetails: any = {};
    isHomeActive: boolean = true;

    showAgentLocation: boolean = false;
    showResendMessage: boolean = false;
    addCaseModal: boolean = false;
    showHotKey: boolean = false;
    showTransDetail: boolean = false;
    showErrorLog: boolean = false;
    showCustomerRequestLog: boolean = false;

    searchModel: any = {};
    caseDetailsList: any = [];

    loading: boolean = false;
    verification: any = {};
    switchCaseLoading: boolean = false;

    constructor(private distributorService: DistributorService, private messageService: MessageService) {
        this.caseList = [
            { label: 'Customer Care Portal', icon: 'far fa-handshake' }
        ];

        this.activeCase = this.caseList[0];
    }

    ngOnInit() {
    }

    closeCase(event, index) {
        this.switchCaseLoading = true;
        this.caseList = this.caseList.filter((item, i) => i !== index);
        event.preventDefault();
        this.caseDetailsList.splice([index - 1], 1);
        setTimeout(() => {
            this.isHomeActive = true;
            this.switchCaseLoading = false;
        }, 500);
    }

    addCase(): any {
        this.verification = {};
        this.caseDetailsList.push(this.searchModel.result);
        this.caseList.push({ label: this.searchModel.result.name, icon: 'far fa-user-circle' });
        this.searchModel = {};
        this.addCaseModal = false;
    }

    openTab(event, index) {
        if (!this.switchCaseLoading) {
            this.switchCaseLoading = true;
            if (index > 0) {
                this.isHomeActive = false;
                this.activeCaseDetails = this.caseDetailsList[index - 1];
                setTimeout(() => {
                    this.switchCaseLoading = false;
                }, 500);
            }
            else {
                this.isHomeActive = true;
                setTimeout(() => {
                    this.switchCaseLoading = false;
                }, 500);
            }
        }
        
    }

    goBack() {
        window.history.back();
    }

    openModal(event) {
        switch (event) {
            case 'agent-location':
                this.showAgentLocation = true;
                break;
            case 'resend-message':
                this.showResendMessage = true;
                break;
            case 'profile':
                this.addCaseModal = true;                
                break;
            case 'hot-key':
                this.showHotKey = true;
                break;
            case 'trans-detail':
                this.showTransDetail = true;
                break;
            case 'error':
                this.showErrorLog = true;
                break;
            case 'request':
                this.showCustomerRequestLog = true;
                break;
            default:
                break;
        }
    }    

    onSearch() {
        this.loading = true;
        this.distributorService.GetDistributorByMphone(this.searchModel.mphone).pipe(first())
            .subscribe(
            data => {
                this.loading = false;
                if (data) {
                    this.searchModel.result = data;
                    this.searchModel.result.dateOfBirth = this.searchModel.result.dateOfBirth != null ? this.searchModel.result.dateOfBirth.split('T')[0] : this.searchModel.result.dateOfBirth; 
                }
                else {
                    this.searchModel.result = {};
                    this.messageService.add({ severity: 'error', summary: 'Warning! ', detail: this.searchModel.mphone + ' could not be found' });
                }
                },
                error => {
                    console.log(error);
                });
    }

    verify(obj) {
        switch (obj) {
            case 'photo-id':
                if (this.verification.photoId == this.searchModel.result.photoId) {
                    this.verification.verified = true;
                }
                else {
                    this.verification.verified = false;
                }
                break;
            case 'dob':
                if (this.verification.dateOfBirth == this.searchModel.result.dateOfBirth) {
                    this.verification.verified = true;
                } else {
                    this.verification.verified = false;
                }
                break;
            case 'con-mob':
                if (this.verification.conMob == this.searchModel.result.secondConMob) {
                    this.verification.verified = true;
                } else {
                    this.verification.verified = false;
                }
                break;
            default:
        }
    }
}
