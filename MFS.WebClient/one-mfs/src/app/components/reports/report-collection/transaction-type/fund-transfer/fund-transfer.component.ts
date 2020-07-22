import { Component, OnInit } from '@angular/core';
import { TransactionReportService } from 'src/app/services/report/transaction-report.service';
import { MfsUtilityService } from 'src/app/services/mfs-utility.service';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-fund-transfer',
  templateUrl: './fund-transfer.component.html',
  styleUrls: ['./fund-transfer.component.css']
})
export class FundTransferComponent implements OnInit {
    model: any;
    isLoading: boolean = false;
    tansactionTypeDDL: any;
    optionDDL: any;
    isDateRangeShow: boolean = false;
    constructor(private messageService: MessageService,private transactionReportService: TransactionReportService, private mfsUtilityService: MfsUtilityService) {
        this.model = {};
    }

    ngOnInit() {
        this.tansactionTypeDDL = [
            { label: 'AC TO AC', value: 'AC TO AC' },
            { label: 'AC TO GL', value: 'AC TO GL' },
            { label: 'GL TO GL', value: 'GL TO GL' },
            { label: 'GL TO AC', value: 'GL TO AC' },
            { label: 'C TO M', value: 'PAY' }
        ];
        this.optionDDL = [
            { label: 'Cumulative', value: 'Cumulative' },
            { label: 'Period', value: 'Period' }
        ];
    }

    showDateRangeDiv() {
        if (this.model.option) {
            if (this.model.option == 'Period') {
                this.isDateRangeShow = true;               
            }            
            else {
                this.isDateRangeShow = false;     
            }
        }
        else {
            this.messageService.add({ severity: 'warn', summary: 'Option Empty', detail: 'Select Option First!' });
            this.model.option = '';
        }
    }

    getReportParam() {
        if (this.validate()) {
            var obj: any = {};
            obj.tansactionType = this.model.tansactionType;
            obj.option = this.model.option;
            if (this.model.option == "Period") {
                obj.fromDate = this.mfsUtilityService.renderDate(this.model.fromDate, true);
                obj.toDate = this.mfsUtilityService.renderDate(this.model.toDate, true);
            }
            else {
                obj.fromDate = null;
                obj.toDate = null;
            }
            return obj;
        }
        else {
            var obj: any = {};
            obj.isNotValidated = true;
        }
    }

    validate(): any {
        if (this.model.option == "Period") {
            if (!this.model.fromDate || !this.model.toDate                
                || !this.model.option || this.model.option == '0') {
                return false;
            }
            else {
                return true;
            }
        }
        else {
            if (!this.model.option || this.model.option == '0') {
                return false;
            }
            else {
                return true;
            }

        }


    }

}
