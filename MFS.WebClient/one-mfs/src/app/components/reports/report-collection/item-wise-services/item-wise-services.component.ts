import { Component, OnInit } from '@angular/core';
import { MessageService } from 'primeng/api';
import { TransactionReportService } from 'src/app/services/report/transaction-report.service';
import { MfsUtilityService } from 'src/app/services/mfs-utility.service';
import { first } from 'rxjs/operators';

@Component({
    selector: 'app-item-wise-services',
    templateUrl: './item-wise-services.component.html',
    styleUrls: ['./item-wise-services.component.css']
})
export class ItemWiseServicesComponent implements OnInit {

    model: any;
    isLoading: boolean = false;
    telcoTypeDDL: any;
    optionDDL: any;
    error: boolean = false;
    constructor(private messageService: MessageService, private transactionReportService: TransactionReportService, private mfsUtilityService: MfsUtilityService) {
        this.model = {};
    }

    ngOnInit() {
        this.loadTelcoList();

    }

    loadTelcoList() {
        this.isLoading = true;
        this.transactionReportService.getTelcoDDL()
            .pipe(first())
            .subscribe(
                data => {
                    this.isLoading = false;
                    this.telcoTypeDDL = data;
                },
                error => {
                    console.log(error);
                }
            );
    }



    getReportParam() {
        if (this.validate()) {
            var obj: any = {};
            obj.telcoType = this.model.telcoType;

            obj.telcoName = this.telcoTypeDDL.find(it => {
                return it.value.toLowerCase().includes(this.model.telcoType.toLowerCase());
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
            || !this.model.telcoType || this.model.telcoType == '0') {
            return false;
        }
        else {
            return true;
        }


    }

}
