import { Component, OnInit } from '@angular/core';
import { first } from 'rxjs/operators';
import { TransactionReportService } from 'src/app/services/report/transaction-report.service';
import { MfsUtilityService } from 'src/app/services/mfs-utility.service';

@Component({
  selector: 'app-particular-wise-transaction',
  templateUrl: './particular-wise-transaction.component.html',
  styleUrls: ['./particular-wise-transaction.component.css']
})
export class ParticularWiseTransactionComponent implements OnInit {
    particularList: any;
    transactionDDL: any;
    model: any;
    isLoading: boolean = false;
    error: boolean = false;
    constructor(private transactionReportService: TransactionReportService, private mfsUtilityService: MfsUtilityService) {
        this.model = {};
    }

    ngOnInit() {        

        this.loadParticulars();
    }

    loadParticulars() {
        this.isLoading = true;
        this.transactionReportService.getParticularDDL()
            .pipe(first())
            .subscribe(
                data => {
                    this.isLoading = false;
                    this.particularList = data;
                },
                error => {
                    console.log(error);
                }
            );
    }

    loadTrasaction() {
        this.isLoading = true;
        this.transactionReportService.getTransactionDDLByParticular(this.model.particular)
            .pipe(first())
            .subscribe(
                data => {
                    this.isLoading = false;
                    this.transactionDDL = data;
                },
                error => {
                    console.log(error);
                }
            );
    }

    getReportParam() {
        if (this.validate()) {
            var obj: any = {};
            obj.fromDate = this.mfsUtilityService.renderDate(this.model.fromDate, true);
            obj.toDate = this.mfsUtilityService.renderDate(this.model.toDate, true);
            obj.particular = this.model.particular;
            obj.transaction = this.model.transaction ? this.model.transaction : '';

            
            return obj;
        }
        else {
            var obj: any = {};
            obj.isNotValidated = true;
        }
    }

    validate(): any {
        if (!this.model.fromDate || !this.model.toDate
            || !this.model.particular || this.model.particular == '0') {
            return false;
        }
        else {
            return true;
        }
    }

}
