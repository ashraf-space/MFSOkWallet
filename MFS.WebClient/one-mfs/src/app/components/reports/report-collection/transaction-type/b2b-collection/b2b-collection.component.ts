import { Component, OnInit } from '@angular/core';
import { first } from 'rxjs/operators';
import { MfsUtilityService } from 'src/app/services/mfs-utility.service';
import { TransactionReportService } from 'src/app/services/report/transaction-report.service';

@Component({
  selector: 'app-b2b-collection',
  templateUrl: './b2b-collection.component.html',
  styleUrls: ['./b2b-collection.component.css']
})
export class B2bCollectionComponent implements OnInit {
    model: any;
    isLoading: boolean = false;
    error: boolean = false;
    okServicesList: any;

    tansactionTypeDDL: any;
    //isDisableService: boolean = true;
    constructor(private transactionReportService: TransactionReportService, private mfsUtilityService: MfsUtilityService) {
        this.model = {};
    }

    ngOnInit() {
       
        this.okServicesList = [
            //{ label: 'All', value: 'All' },
            { label: 'B2B Master Distributor to B2B Distributor', value: 'AMBD ABD' },
            { label: 'B2B Distributor to B2B Master Distributor', value: 'ABD AMBD' },
            { label: 'B2B Distributor to B2B DSR', value: 'ABD ABR' },
            { label: 'B2B DSR to B2B Distributor', value: 'ABR ABD' },
            { label: 'B2B DSR to B2B Parent Merchant', value: 'ABR ABM' },
            { label: 'B2B DSR to B2B Chain Merchant', value: 'ABR ABMC' },
            { label: 'B2B Chain Merchant to B2B DSR', value: 'ABMC ABR' },
            { label: 'B2B Chain Merchant to B2B Parent Merchant', value: 'ABMC ABM' }
        ];
        this.tansactionTypeDDL = [
            { label: 'Summary', value: 'Summary' },
            { label: 'Details', value: 'Details' }
        ];

     
    }



    getReportParam() {
        if (this.validate()) {
            var obj: any = {};
            obj.tansactionType = this.model.tansactionType;
            obj.okServices = this.model.okServices;
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
        //if (this.model.tansactionType == "Transaction Summary") {
        //    if (!this.model.fromDate || !this.model.toDate
        //        || !this.model.tansactionType || this.model.tansactionType == '0'
        //        || !this.model.dateType || this.model.dateType == '0'
        //        || !this.model.gateway || this.model.gateway == '0') {
        //        return false;
        //    }
        //    else {
        //        return true;
        //    }
        //}
        //else {
            if (!this.model.fromDate || !this.model.toDate
                || !this.model.tansactionType || this.model.tansactionType == '0') {
                return false;
            }
            else {
                return true;
            }

        //}


    }

}
