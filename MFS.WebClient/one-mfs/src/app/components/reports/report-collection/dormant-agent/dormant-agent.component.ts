import { Component, OnInit } from '@angular/core';
import { MfsUtilityService } from 'src/app/services/mfs-utility.service';
import { KycReportService } from 'src/app/services/report/kyc-report.service';

@Component({
    selector: 'app-dormant-agent',
    templateUrl: './dormant-agent.component.html',
    styleUrls: ['./dormant-agent.component.css']
})
export class DormantAgentComponent implements OnInit {
    typeList: any;
    model: any;
    isLoading: boolean = false;
    error: boolean = false;
    constructor(private kycReportService: KycReportService, private mfsUtilityService: MfsUtilityService) {
        this.model = {};
    }

    ngOnInit() {
        this.typeList = [
            { label: 'Agent', value: 'A' },
            { label: 'Distributor', value: 'D' }
        ];
    }

    getReportParam() {
        if (this.validate()) {
            var obj: any = {};
            obj.type = this.model.type;
            obj.fromDate = this.mfsUtilityService.renderDate(this.model.fromDate, true);
            obj.toDate = this.mfsUtilityService.renderDate(this.model.toDate, true);
          
            

            return obj;
        }
        else {
            var obj: any = {};
            obj.isNotValidated = true;
            return obj;
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
