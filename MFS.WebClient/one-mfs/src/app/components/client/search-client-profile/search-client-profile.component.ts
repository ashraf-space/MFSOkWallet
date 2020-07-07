import { Component, OnInit } from '@angular/core';
import { DistributorService } from 'src/app/services/distribution';
import { MessageService } from 'primeng/api';
import { first } from 'rxjs/operators';
import { KycService } from '../../../services/distribution/kyc.service';
import { AuthenticationService, AuditTrailService } from 'src/app/shared/_services';

@Component({
    selector: 'app-search-client-profile',
    templateUrl: './search-client-profile.component.html',
    styleUrls: ['./search-client-profile.component.css']
})
export class SearchClientProfileComponent implements OnInit {

    searchModel: any;
    loading: boolean = false;
    auditTrailModel: any = {};
    currentUserModel: any;
    constructor(private distributorService: DistributorService,
        private messageService: MessageService, private authService: AuthenticationService,
        private kycService: KycService, private auditTrailService: AuditTrailService) {
        this.authService.currentUser.subscribe(x => {
            this.currentUserModel = x;
        });
        this.searchModel = {};
    }

    ngOnInit() {
    }

    onSearch() {
        this.loading = true;
        this.distributorService.GetDistributorByMphone(this.searchModel.mphone).pipe(first())
            .subscribe(
                data => {
                    this.loading = false;
                    if (data) {
                        this.searchModel.result = data;                       
                        this.getClientDistLocationInfo();
                        this.getPhotoIdTypeByCode();
                        this.getBranchNameByCode();
                        this.insertDataToAuditTrail();
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
    insertDataToAuditTrail() {
        this.auditTrailModel.Who = this.currentUserModel.user.username;
        this.auditTrailModel.WhatAction = 'SEARCH';
        this.auditTrailModel.WhatActionId = this.auditTrailService.getWhatActionId('SEARCH');                   
        var eventLog = JSON.parse(sessionStorage.getItem('currentEvent'));             
        this.auditTrailModel.WhichMenu = eventLog.item.label.trim();       
        this.auditTrailModel.WhichParentMenu = this.currentUserModel.featureList.find(it => {
            return it.FEATURENAME.includes(this.auditTrailModel.WhichMenu);
        }).CATEGORYNAME;
        this.auditTrailModel.WhichParentMenuId = this.auditTrailService.getWhichParentMenuId(this.auditTrailModel.WhichParentMenu);        
        this.auditTrailModel.inputFeildAndValue = [{ whichFeildName: 'Mphone', whatValue: this.searchModel.mphone}];
        this.auditTrailService.insertIntoAuditTrail(this.auditTrailModel).pipe(first())
            .subscribe(
                data => {
                    if (data) {
                    }
                },
                error => {

                });
    }
    getBranchNameByCode() {
        this.loading = true;
        this.kycService.getBranchNameByCode(this.searchModel.result.branchCode).pipe(first())
            .subscribe(
                data => {
                    this.loading = false;
                    if (data) {
                        this.searchModel.result.branchName = data.value;
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
        this.kycService.getPhotoIdTypeByCode(this.searchModel.result.photoIdTypeCode).pipe(first())
            .subscribe(
                data => {
                    this.loading = false;
                    if (data) {
                        this.searchModel.result.photoIdType = data.value;                       
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
        this.kycService.getClientDistLocationInfo(this.searchModel.result.distCode, this.searchModel.result.locationCode).pipe(first())
            .subscribe(
                data => {
                    this.loading = false;
                    if (data) {
                        this.searchModel.result.division = data.division;
                        this.searchModel.result.district = data.district;
                        this.searchModel.result.thana = data.thana;
                        this.searchModel.result.region = data.region;
                        this.searchModel.result.area = data.area;
                        this.searchModel.result.territory = data.territory;
                    }
                    
                },
                error => {
                    this.loading = false;
                    console.log(error);
                });
        this.loading = false;
    }

}
