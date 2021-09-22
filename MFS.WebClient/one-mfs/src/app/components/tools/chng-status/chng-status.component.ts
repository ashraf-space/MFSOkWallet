import { Component, OnInit } from '@angular/core';
import { DistributorService } from 'src/app/services/distribution';
import { MessageService } from 'primeng/api';
import { first } from 'rxjs/operators';
import { KycService } from '../../../services/distribution/kyc.service';
import { AuthenticationService, AuditTrailService } from 'src/app/shared/_services';

@Component({
  selector: 'app-chng-status',
  templateUrl: './chng-status.component.html',
  styleUrls: ['./chng-status.component.css']
})
export class ChngStatusComponent implements OnInit {
    model: any;
    loading: boolean = false;
    auditTrailModel: any = {};
    currentUserModel: any;
    disableButton = false;
    remarks: any;
    statusList: any;
    selectStatus: any;
    dataModel: any = {};
    constructor(private distributorService: DistributorService,
        private messageService: MessageService, private authService: AuthenticationService,
        private kycService: KycService, private auditTrailService: AuditTrailService) {
        this.authService.currentUser.subscribe(x => {
            this.currentUserModel = x;
        });
        this.model = {};
    }


    ngOnInit() {
        this.statusList = [
            { label: 'Close', value: 'C' },
            { label: 'Inward Block', value: 'I' },
            { label: 'Outward Block', value: 'O' },
            { label: 'Active', value: 'A' }
        ];
  }
    onSearch() {
        this.loading = true;
        this.distributorService.GetDistributorByMphone(this.model.mphone).pipe(first())
            .subscribe(
                data => {
                    this.loading = false;
                    if (data) {
                        this.model = data;
                        this.getSubCatNameById();
                    }
                    else {
                        this.model = {};
                        this.messageService.add({ severity: 'error', summary: 'Warning! ', detail: 'Account could not be found' });
                    }
                },
                error => {
                    this.loading = false;
                    console.log(error);
                });
    }
    getSubCatNameById() {
        this.loading = true;
        this.kycService.getSubCatNameById(this.model.mphone).pipe(first())
            .subscribe(
                data => {
                    this.loading = false;
                    if (data) {
                        this.model.subCategory = data;
                    }
                },
                error => {
                    this.loading = false;
                    console.log(error);
                });
        this.loading = false;
    }
    changeStatus() {
        this.disableButton = true;
        this.model.updateBy = this.currentUserModel.user.username;
        this.model.status = this.selectStatus;
        this.kycService.changeStatus(this.model, this.remarks).pipe(first())
            .subscribe(
                data => {
                    if (data) {
                        this.dataModel = data;
                        if (this.dataModel.status === 200) {
                            this.onSearch();
                            this.messageService.add({ severity: 'success', summary: 'Success', sticky: true, detail: 'Response: ' + this.dataModel.messege });
                        }
                        else {
                            this.onSearch();
                            this.messageService.add({ severity: 'error', summary: 'Error', sticky: true, detail: 'Response: ' + this.dataModel.messege });
                        }
                        
                    }                                  
                    this.disableButton = false;               
                },
                error => {
                    console.log(error);
                });
    }
}
