import { Component, OnInit } from '@angular/core';
import { MfsUtilityService } from 'src/app/services/mfs-utility.service';
import { first } from 'rxjs/operators';
import { KycService } from 'src/app/services/distribution/kyc.service';
import { KycReportService } from 'src/app/services/report/kyc-report.service';
import { NgbDatepickerConfig, NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { AuthenticationService } from 'src/app/shared/_services';


@Component({
  selector: 'app-channel-bank-info',
  templateUrl: './channel-bank-info.component.html',
  styleUrls: ['./channel-bank-info.component.css']
})
export class ChannelBankInfoComponent implements OnInit {

    model: any;
    dateTypeList: any;
    utilityList: any;
    gatewayList: any;
    categoryList: any;
    currentUserModel: any = {};
    emsList: any;
    constructor(private mfsUtilityService: MfsUtilityService,
        private kycReportService: KycReportService,
        private authService: AuthenticationService,
        private ngbDatepickerConfig: NgbDatepickerConfig) {
        this.authService.currentUser.subscribe(x => {
            this.currentUserModel = x;
        });
        ngbDatepickerConfig.minDate = { year: 1919, month: 1, day: 1 };
        var currentDate = new Date();
        ngbDatepickerConfig.maxDate = { year: currentDate.getFullYear(), month: currentDate.getMonth() + 1, day: currentDate.getDate() };
        this.model = {};
    }

    ngOnInit() {
        this.categoryList = [
            { label: 'Merchant', value: 'M' },
            { label: 'EMS Mercahnt Module', value: 'EMSM' },
            { label: 'EMS Bill Payment Module', value: 'EMSC' },
            { label: 'MMS Mercahnt Module', value: 'MMSM' },
            { label: 'MMS Bill Payment Module', value: 'MMSC' }
        ]       
    }
    getReportParam() {
        if (this.validate()) {
            var obj: any = {};
            if (this.model.fromDate && this.model.toDate) {
                obj.fromDate = this.mfsUtilityService.renderDate(this.model.fromDate, true);
                obj.toDate = this.mfsUtilityService.renderDate(this.model.toDate, true);
            }
            if (!this.model.accNo) {
                obj.accNo = null;
            }
            else {
                obj.accNo = this.model.accNo
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
        if (!this.model.fromDate || !this.model.toDate || !this.model.catId) {
            return false;
        }
        else {
            return true;
        }
    }

}
