import { Component, OnInit, ViewEncapsulation, Output, EventEmitter } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { error } from 'selenium-webdriver';
import { first } from 'rxjs/operators';
import { MessageService, MenuItem } from 'primeng/api';
import { AuthenticationService, AuditTrailService } from 'src/app/shared/_services';
import { CustomerService } from '../../../../services/distribution/customer.service';
import { MfsUtilityService } from '../../../../services/mfs-utility.service';
import { Message } from 'primeng/components/common/api';
import { KycService } from '../../../../services/distribution/kyc.service';
import { Reginfo } from '../../../../shared/_models/reginfo';
@Component({
    selector: 'app-kyc-cbs',
    templateUrl: './kyc-cbs.component.html',
    styleUrls: ['./kyc-cbs.component.css'],
    encapsulation: ViewEncapsulation.None
})
export class KycCbsComponent implements OnInit {
    public reginfo: Reginfo;
    msgs: Message[] = [];
    @Output() onCbsDataUpdate = new EventEmitter<any>();
    auditTrailModel: any = {};
    currentUserModel: any;
    constructor(private router: Router,
        private route: ActivatedRoute,
        private messageService: MessageService,
        private authService: AuthenticationService,
        private customerService: CustomerService,
        private mfsUtilityService: MfsUtilityService,
        private auditTrailService: AuditTrailService,
        private kycService: KycService) {
        this.reginfo = new Reginfo();
        this.authService.currentUser.subscribe(x => {
            this.currentUserModel = x;
        });
    }
    ngOnInit() {
        var a = 10;
    }
    getCbsAccInfo() {
        this.reginfo.isLoading = true;
        this.insertDataToAuditTrail();
        this.customerService.GetCbsAccInfo(this.reginfo.regInfoModel.mphone, this.reginfo.regInfoModel.bankAcNo)
            .pipe(first())
            .subscribe(
                data => {
                    if (data) {
                        if (data.status === 200) {
                            this.onCbsDataUpdate.emit(data);
                        }
                        else if (data.status === 406) {
                            this.msgs = [];
                            this.msgs.push({ severity: 'warn', summary: 'Warn Message', detail: data.erros });
                        }
                        else if (data.status === 417) {
                            this.msgs = [];
                            this.msgs.push({ severity: 'error', summary: 'Error Message', detail: data.erros });
                        }
                    }
                    else {
                        this.msgs.push({ severity: 'warn', summary: 'Warn Message', detail: 'No Data Found' });
                    }
                    this.reginfo.isLoading = false;
                },
                error => {
                    console.log(error);
                });
    }
    refresh() {
        this.reginfo.regInfoModel.mphone = '';
        this.reginfo.regInfoModel.bankAcNo = '';
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
        this.auditTrailModel.inputFeildAndValue = [
            { whichFeildName: 'Mphone', whatValue: this.reginfo.regInfoModel.mphone },
            { whichFeildName: 'bankAcNo', whatValue: this.reginfo.regInfoModel.bankAcNo}
        ];
        this.auditTrailService.insertIntoAuditTrail(this.auditTrailModel).pipe(first())
            .subscribe(
                data => {
                    if (data) {
                    }
                },
                error => {

                });
    }
}
