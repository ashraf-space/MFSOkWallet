import { Component, OnInit } from '@angular/core';
import { MfsUtilityService } from 'src/app/services/mfs-utility.service';
import { KycReportService } from 'src/app/services/report/kyc-report.service';

@Component({
  selector: 'app-dist-wise-commission',
  templateUrl: './dist-wise-commission.component.html',
  styleUrls: ['./dist-wise-commission.component.css']
})
export class DistWiseCommissionComponent implements OnInit {
    commissionStatusList: any;
    reportNameList: any;
    model: any;
    isLoading: boolean = false;
    error: boolean = false;
    constructor(private kycReportService: KycReportService, private mfsUtilityService: MfsUtilityService) {
        this.model = {};
    }

    ngOnInit() {
        this.reportNameList = [
            { label: 'Distributor wise documentation & PIN change commission report', value: 'SLUBONEDISTRIBUTOR' },
            { label: 'Agent wise documentation & PIN change commission report', value: 'SLUBONEAGENT' },
            { label: 'Distributor wise commission for outgoing transaction', value: 'SLUBTWODISTRIBUTOR' },
            { label: 'Agent wise commission for outgoing transaction', value: 'SLUBTWOAGENT' }
        ];
        this.commissionStatusList = [
            { label: 'Eligible', value: 'P' },
            { label: 'Disbursed', value: 'S' },
            { label: 'Not Eligible', value: 'I' },
            { label: 'Failed', value: 'F' }
        ];
    }

    getReportParam() {
        if (this.validate()) {
            var obj: any = {};
            if (this.model.regFromDate && this.model.regToDate) {
                obj.regFromDate = this.mfsUtilityService.renderDate(this.model.regFromDate, true);
                obj.regToDate = this.mfsUtilityService.renderDate(this.model.regToDate, true);
            }
            if (this.model.authFromDate && this.model.authToDate) {
                obj.authFromDate = this.mfsUtilityService.renderDate(this.model.authFromDate, true);
                obj.authToDate = this.mfsUtilityService.renderDate(this.model.authToDate, true);
            }
            else {
                obj.authFromDate = null;
                obj.authToDate = null;
            }
            obj.reportName = this.model.reportName;                    
            if (this.model.commissionStatus) {
                obj.commissionStatus = this.model.commissionStatus;
            }
            else {
                obj.commissionStatus = null;
            }
            if (this.model.distributorNo) {
                obj.distributorNo = this.model.distributorNo;
            }
            else {
                obj.distributorNo = null;
            }
            if (this.model.transNo) {
                obj.transNo = this.model.transNo;
            }
            else {
                obj.transNo = null;
            }
            
            if (this.model.agentNo) {
                obj.agentNo = this.model.agentNo;
            }
            else {
                obj.agentNo = null;
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
        if (!this.model.regFromDate || !this.model.regToDate || !this.model.reportName) {
            return false;
        }
        else {
            return true;
        }
    }

}
