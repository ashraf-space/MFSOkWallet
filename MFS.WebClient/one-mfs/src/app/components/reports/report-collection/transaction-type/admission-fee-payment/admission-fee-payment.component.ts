import { Component, OnInit } from '@angular/core';
import { MfsUtilityService } from 'src/app/services/mfs-utility.service';
import { TransactionReportService } from 'src/app/services/report/transaction-report.service';

@Component({
  selector: 'app-admission-fee-payment',
  templateUrl: './admission-fee-payment.component.html',
  styleUrls: ['./admission-fee-payment.component.css']
})
export class AdmissionFeePaymentComponent implements OnInit {
    model: any;
    isLoading: boolean = false;
    error: boolean = false;
    constructor(private transactionReportService: TransactionReportService, private mfsUtilityService: MfsUtilityService) {
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
