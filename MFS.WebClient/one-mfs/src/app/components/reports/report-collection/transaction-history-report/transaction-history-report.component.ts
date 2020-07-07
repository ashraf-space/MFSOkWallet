import { Component, OnInit } from '@angular/core';
import { MfsUtilityService } from 'src/app/services/mfs-utility.service';

@Component({
  selector: 'transaction-history-report',
  templateUrl: './transaction-history-report.component.html',
  styleUrls: ['./transaction-history-report.component.css']
})
export class TransactionHistoryReportComponent implements OnInit {

    model: any;
    constructor(private mfsUtilityService: MfsUtilityService) {
        this.model = {};
    }

    ngOnInit() {
    }

    getReportParam() {
        var obj: any = {};
        obj.fromDate = this.mfsUtilityService.renderDate(this.model.fromDate, true);
        obj.toDate = this.mfsUtilityService.renderDate(this.model.toDate, true);
        obj.mphone = this.model.mphone;
        return obj;
    }

}
