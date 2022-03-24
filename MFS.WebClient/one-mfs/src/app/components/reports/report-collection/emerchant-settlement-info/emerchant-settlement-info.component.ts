import { Component, OnInit } from '@angular/core';
import { MessageService } from 'primeng/api';
import { MfsUtilityService } from 'src/app/services/mfs-utility.service';
import { KycReportService } from 'src/app/services/report/kyc-report.service';

@Component({
    selector: 'app-emerchant-settlement-info',
    templateUrl: './emerchant-settlement-info.component.html',
    styleUrls: ['./emerchant-settlement-info.component.css']
})
export class EmerchantSettlementInfoComponent implements OnInit {
    model: any;
    isLoading: boolean = false;
    isDateRangeShow: boolean = false;
    error: boolean = false;
    constructor(private messageService: MessageService, private kycReportService: KycReportService, private mfsUtilityService: MfsUtilityService) {
        this.model = {};
    }


    ngOnInit() {
    }


    getReportParam() {
        if (this.validate()) {
            var obj: any = {};
            obj.fromDate = this.mfsUtilityService.renderDate(this.model.fromDate, true);
            obj.toDate = this.mfsUtilityService.renderDate(this.model.toDate, true);

            return obj;
        }
        else {
            var obj: any = {};
            obj.isNotValidated = true;
        }
    }

    validate(): any {
        if (!this.model.fromDate || !this.model.toDate) {
            return false;
        }
        else {
            return true;
        }
    }

}
