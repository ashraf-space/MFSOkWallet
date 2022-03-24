import { Component, OnInit } from '@angular/core';
import { first } from 'rxjs/operators';
import { MfsUtilityService } from 'src/app/services/mfs-utility.service';
import { TransactionReportService } from 'src/app/services/report/transaction-report.service';

@Component({
    selector: 'app-referral-campaign',
    templateUrl: './referral-campaign.component.html',
    styleUrls: ['./referral-campaign.component.css']
})
export class ReferralCampaignComponent implements OnInit {
    model: any;
    isLoading: boolean = false;
    error: boolean = false;
    campaignTypeList: any;
    tansactionTypeDDL: any;
    //isDisableService: boolean = true;
    fromTime = { hour: 13, minute: 30 };
    meridian = true;

    toggleMeridian() {
        this.meridian = !this.meridian;
    }
    constructor(private transactionReportService: TransactionReportService, private mfsUtilityService: MfsUtilityService) {
        this.model = {};
    }

    ngOnInit() {
        this.tansactionTypeDDL = [
            { label: 'Transaction Summary', value: 'Transaction Summary' },
            { label: 'Transaction Details', value: 'Transaction Details' }
        ];

        this.getCampaignTypeList();
    }

    getCampaignTypeList() {
        this.isLoading = true;
        this.transactionReportService.getCampaignTypeDDL()
            .pipe(first())
            .subscribe(

                data => {
                    this.isLoading = false;
                    this.campaignTypeList = data;
                    //if (data) {
                    //    this.campaignTypeList.unshift({ label: 'All', value: 'All' });
                    //}
                },
                error => {
                    console.log(error);
                }
            );
    }



    getReportParam() {
        if (this.validate()) {
            var obj: any = {};
            obj.tansactionType = this.model.tansactionType;
            obj.campaignType = this.model.campaignType;
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
        if (!this.model.fromDate || !this.model.toDate
            || !this.model.tansactionType || this.model.tansactionType == '0') {
            return false;
        }
        else {
            return true;
        }
    }

}
