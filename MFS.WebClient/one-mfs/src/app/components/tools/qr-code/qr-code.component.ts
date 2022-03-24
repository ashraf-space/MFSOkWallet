import { Component, OnInit } from '@angular/core';
import { MfsUtilityService } from 'src/app/services/mfs-utility.service';
import { first } from 'rxjs/operators';
import { KycService } from 'src/app/services/distribution/kyc.service';
import { KycReportService } from 'src/app/services/report/kyc-report.service';
import { NgbDatepickerConfig, NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { AuthenticationService } from 'src/app/shared/_services';

@Component({
    selector: 'app-qr-code',
    templateUrl: './qr-code.component.html',
    styleUrls: ['./qr-code.component.css']
})
export class QrCodeComponent implements OnInit {
    model: any;
    dateTypeList: any;
    utilityList: any;
    gatewayList: any;
    catTypeList: any;
    qrTypeList: any;
    isDateDisabled: boolean = false;
    currentUserModel: any = {};
    constructor(private mfsUtilityService: MfsUtilityService,
        private kycReportService: KycReportService,
        private authService: AuthenticationService) {
        this.authService.currentUser.subscribe(x => {
            this.currentUserModel = x;
        });
        this.model = {};
    }

    ngOnInit() {
        this.catTypeList = [
            { label: 'Agent', value: 'A' },
            { label: 'Customer', value: 'C' },
            { label: 'Merchant', value: 'M' }           
        ];
        this.qrTypeList = [
            { label: 'Ok Qr (Previous)', value: 'okqr' },
            { label: 'Bangla Qr', value: 'bnqr' }          
        ];
    }
    getReportParam() {
        if (this.validate()) {
            var obj: any = {};

            if (!this.model.mphone) {
                obj.mphone = null;
            }
            else {
                obj.mphone = this.model.mphone
            }   
            if (!this.model.qrType) {
                obj.qrType = null;
            }
            else {
                obj.qrType = this.model.qrType
            }   
            if (!this.model.catId) {
                obj.catId = null;
            }
            else {
                obj.catId = this.model.catId
            }
               
            return obj;
        }
        else {
            var obj: any = {};
            obj.isNotValidated = true;
            return obj;
        }
    }

    validate(): any {
        if (!this.model.catId || !this.model.mphone || !this.model.qrType) {
            return false;
        }
        else {
            return true;
        }
    }

}
