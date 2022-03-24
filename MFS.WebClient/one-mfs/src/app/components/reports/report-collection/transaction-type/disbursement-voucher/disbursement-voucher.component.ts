import { Component, OnInit } from '@angular/core';
import { MessageService } from 'primeng/api';
import { first } from 'rxjs/operators';
import { MfsUtilityService } from 'src/app/services/mfs-utility.service';
import { TransactionReportService } from 'src/app/services/report/transaction-report.service';
import { AuthenticationService } from 'src/app/shared/_services';
import { disbursementService } from 'src/app/services/transaction';

@Component({
  selector: 'app-disbursement-voucher',
  templateUrl: './disbursement-voucher.component.html',
  styleUrls: ['./disbursement-voucher.component.css']
})
export class DisbursementVoucherComponent implements OnInit {
    model: any;
    isLoading: boolean = false;
    optionDDL: any;
    disburseTypeDDL: any;
    error: boolean = false;
    currentUserModel: any = {};
    roleName: any;
    constructor(private messageService: MessageService, private transactionReportService: TransactionReportService,
        private mfsUtilityService: MfsUtilityService, private authService: AuthenticationService, private disbursementService: disbursementService) {
        this.model = {};
        this.authService.currentUser.subscribe(x => {
            this.currentUserModel = x;
        });
    }

    ngOnInit() {
        this.optionDDL = [
            { label: 'Successful', value: 'Successful' },
            { label: 'Failed', value: 'Failed' }
            //{ label: 'Rejected', value: 'Rejected' }
        ];

        this.loadDisburseTypeList();

       

    }

    loadDisburseTypeList() {
        //this.isLoading = true;
        //this.roleName = this.currentUserModel.user.role_Name;
        //if (this.roleName != "Training") {
        //    this.disbursementService.getDisburseTypeList()
        //        .pipe(first())
        //        .subscribe(
        //            data => {
        //                this.isLoading = false;
        //                this.disburseTypeDDL = data;
        //            },
        //            error => {
        //                console.log(error);
        //            }
        //        );
        //}
        //else {
        //    this.disburseTypeDDL = [
        //        { label: 'Training Honorarium', value: 'TRH' }
        //    ];
        //}

        this.disburseTypeDDL = [
            { label: 'Training Honorarium', value: 'TRH' }
        ];
        
    }



    getReportParam() {
        if (this.validate()) {
            var obj: any = {};
            obj.optionId = this.model.optionId;         
            obj.disTypeId = this.model.disTypeId;         
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
            || !this.model.optionId || this.model.optionId == '0'
            || !this.model.disTypeId || this.model.disTypeId == '0') {
            return false;
        }
        else {
            return true;
        }


    }

}
