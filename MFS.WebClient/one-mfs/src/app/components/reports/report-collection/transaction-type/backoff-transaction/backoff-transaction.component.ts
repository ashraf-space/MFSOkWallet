import { Component, OnInit } from '@angular/core';
import { MessageService } from 'primeng/api';
import { MfsUtilityService } from 'src/app/services/mfs-utility.service';
import { TransactionReportService } from 'src/app/services/report/transaction-report.service';

@Component({
    selector: 'app-backoff-transaction',
    templateUrl: './backoff-transaction.component.html',
    styleUrls: ['./backoff-transaction.component.css']
})
export class BackoffTransactionComponent implements OnInit {
    model: any;
    isLoading: boolean = false;
    tansactionTypeDDL: any;
    optionDDL: any;
    isDateRangeShow: boolean = false;
    error: boolean = false;
    constructor(private messageService: MessageService, private transactionReportService: TransactionReportService, private mfsUtilityService: MfsUtilityService) {
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
