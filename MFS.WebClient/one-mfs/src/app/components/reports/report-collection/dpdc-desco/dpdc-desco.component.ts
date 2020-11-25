import { Component, OnInit } from '@angular/core';
import { MfsUtilityService } from 'src/app/services/mfs-utility.service';
import { first } from 'rxjs/operators';
import { KycService } from 'src/app/services/distribution/kyc.service';
import { KycReportService } from 'src/app/services/report/kyc-report.service';
import { NgbDatepickerConfig, NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-dpdc-desco',
  templateUrl: './dpdc-desco.component.html',
  styleUrls: ['./dpdc-desco.component.css']
})
export class DpdcDescoComponent implements OnInit {

   model: any;
    dateTypeList: any;
    utilityList: any;
    gatewayList: any;
    catTypeList: any;
    isDateDisabled: boolean = false;
    constructor(private mfsUtilityService: MfsUtilityService,
        private kycReportService: KycReportService,
        private ngbDatepickerConfig: NgbDatepickerConfig) {
        ngbDatepickerConfig.minDate = { year: 1919, month: 1, day: 1 };
        var currentDate = new Date();
        ngbDatepickerConfig.maxDate = { year: currentDate.getFullYear(), month: currentDate.getMonth() + 1, day: currentDate.getDate() };
        this.model = {};
    }


    ngOnInit() {
        this.utilityList = [
            { label: 'DPDC', value: 'dpdc' },
            { label: 'DESCO', value: 'desco' },
            { label: 'WASA', value: 'wasa' },
            { label: 'JALALABAD GAS', value: 'jgtd' }
        ];
        this.gatewayList = [
            { label: 'USSD', value: 'U' },
            { label: 'APP', value: 'A' },
            { label: 'ALL', value: 'All' }  
        ];
        this.catTypeList = [
            { label: 'Agent', value: 'A' },
            { label: 'Customer', value: 'C' },
            { label: 'ALL', value: 'All' }  
        ];
        this.dateTypeList = [
            { label: 'EOD Date', value: 'eod' },
            { label: 'Transaction Date', value: 'trans' }
        ]
    }
   
   

    getReportParam() {
        if (this.validate()) {
            var obj: any = {};
            if (this.model.fromDate && this.model.toDate) {
                obj.fromDate = this.mfsUtilityService.renderDate(this.model.fromDate, true);
                obj.toDate = this.mfsUtilityService.renderDate(this.model.toDate, true);
            }
            obj.utility = this.model.utility;
            obj.gateway = this.model.gateway;
            obj.catType = this.model.catType;
            obj.dateType = this.model.dateType;
            return obj;
        }
        else {
            var obj: any = {};
            obj.isNotValidated = true;
            return obj;
        }     
    }

    validate(): any {
        if (!this.model.utility || !this.model.gateway || !this.model.catType || !this.model.dateType ||
            !this.model.fromDate || !this.model.toDate) {
            return false;
        }
        else {
            return true;
        }
    }

}
