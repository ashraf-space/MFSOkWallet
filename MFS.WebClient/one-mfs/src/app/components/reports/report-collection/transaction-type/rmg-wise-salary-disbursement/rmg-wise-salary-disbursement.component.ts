import { Component, OnInit } from '@angular/core';
import { MessageService } from 'primeng/api';
import { TransactionReportService } from 'src/app/services/report/transaction-report.service';
import { MfsUtilityService } from 'src/app/services/mfs-utility.service';
import { first } from 'rxjs/operators';

@Component({
  selector: 'app-rmg-wise-salary-disbursement',
  templateUrl: './rmg-wise-salary-disbursement.component.html',
  styleUrls: ['./rmg-wise-salary-disbursement.component.css']
})
export class RmgWiseSalaryDisbursementComponent implements OnInit {

    model: any;
    isLoading: boolean = false;
    rmgDDL: any;
    optionDDL: any;
    error: boolean = false;
    constructor(private messageService: MessageService, private transactionReportService: TransactionReportService, private mfsUtilityService: MfsUtilityService) {
        this.model = {};
    }

    ngOnInit() {
        this.loadRmgList();

    }

    loadRmgList() {
        this.isLoading = true;
        this.transactionReportService.getRmgDDL()
            .pipe(first())
            .subscribe(
                data => {
                    this.isLoading = false;
                    this.rmgDDL = data;
                },
                error => {
                    console.log(error);
                }
            );
    }



    getReportParam() {
        if (this.validate()) {
            var obj: any = {};
            obj.rmgId = this.model.rmgId;

            obj.rmgName = this.rmgDDL.find(it => {
                return it.value.toLowerCase().includes(this.model.rmgId.toLowerCase());
            }).label;

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
            || !this.model.rmgId || this.model.rmgId == '0') {
            return false;
        }
        else {
            return true;
        }


    }


}
