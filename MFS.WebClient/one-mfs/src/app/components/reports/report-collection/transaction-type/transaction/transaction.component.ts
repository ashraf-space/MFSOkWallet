import { Component, OnInit } from '@angular/core';
import { TransactionReportService } from 'src/app/services/report/transaction-report.service';
import { MfsUtilityService } from 'src/app/services/mfs-utility.service';
import { first } from 'rxjs/operators';

@Component({
    selector: 'app-transaction',
    templateUrl: './transaction.component.html',
    styleUrls: ['./transaction.component.css']
})
export class TransactionComponent implements OnInit {
    model: any;
    isLoading: boolean = false;
    error: boolean = false;
    okServicesList: any;
    dateTypeDDL: any;
    gatewayDDL: any;
    tansactionTypeDDL: any;
    //isDisableService: boolean = true;
    constructor(private transactionReportService: TransactionReportService, private mfsUtilityService: MfsUtilityService) {
        this.model = {};
    }

    ngOnInit() {
        this.dateTypeDDL = [
            { label: 'EOD Date', value: 'EOD Date' },
            { label: 'Transaction Date', value: 'Transaction Date' }
        ];
        this.gatewayDDL = [
            { label: 'All', value: 'L' },
            { label: 'APPS', value: 'A' },
            { label: 'USSD', value: 'U' },
            { label: 'COMMON', value: 'C' }
        ];
        this.tansactionTypeDDL = [
            { label: 'Transaction Summary', value: 'Transaction Summary' },
            { label: 'Transaction Details', value: 'Transaction Details' }
        ];

        this.getOkServicesList();
    }

    getOkServicesList() {
        this.isLoading = true;
        this.transactionReportService.getOkServicesDDL()
            .pipe(first())
            .subscribe(
                data => {
                    this.isLoading = false;
                    this.okServicesList = data;
                    if (data) {
                        this.okServicesList.unshift({ label: 'All', value: 'All' });
                    }
                },
                error => {
                    console.log(error);
                }
            );
    }

    //enableDisableService() {
    //    if (this.model.tansactionType == "Transaction Summary") {
    //        this.isDisableService = true;
    //        this.model.okServices = null;
    //    }
    //    else {
    //        this.isDisableService = false;
    //    }
    //}

    getReportParam() {
        if (this.validate()) {
            var obj: any = {};
            obj.tansactionType = this.model.tansactionType;
            obj.okServices = this.model.okServices;
            obj.dateType = this.model.dateType;
            obj.fromDate = this.mfsUtilityService.renderDate(this.model.fromDate, true);
            obj.toDate = this.mfsUtilityService.renderDate(this.model.toDate, true);
            obj.gateway = this.model.gateway;

            //if (this.model.sysCoaCode) {
            //    obj.coaDes = this.glCoaCodenalelevelDDL.find(it => {
            //        return it.value.toLowerCase().includes(this.model.sysCoaCode.toLowerCase());
            //    }).label;
            //}

            return obj;
        }
        else {
            var obj: any = {};
            obj.isNotValidated = true;
        }
    }

    validate(): any {
        if (this.model.tansactionType == "Transaction Summary") {
            if (!this.model.fromDate || !this.model.toDate
                || !this.model.tansactionType || this.model.tansactionType == '0'
                || !this.model.dateType || this.model.dateType == '0'
                || !this.model.gateway || this.model.gateway == '0') {
                return false;
            }
            else {
                return true;
            }
        }
        else {
            if (!this.model.fromDate || !this.model.toDate
                || !this.model.tansactionType || this.model.tansactionType == '0'
                || !this.model.okServices || this.model.okServices == '0'
                || !this.model.dateType || this.model.dateType == '0'
                || !this.model.gateway || this.model.gateway == '0') {
                return false;
            }
            else {
                return true;
            }

        }


    }

}