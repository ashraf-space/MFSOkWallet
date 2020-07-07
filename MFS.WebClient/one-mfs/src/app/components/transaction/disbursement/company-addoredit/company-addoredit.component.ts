import { Component, OnInit } from '@angular/core';
import { MessageService, Message } from 'primeng/api';
import { ActivatedRoute } from '@angular/router';
import { AuthenticationService } from 'src/app/shared/_services';
import { first } from 'rxjs/operators';
import { disbursementService } from 'src/app/services/transaction/disbursement.service';

@Component({
    selector: 'app-company-addoredit',
    templateUrl: './company-addoredit.component.html',
    styleUrls: ['./company-addoredit.component.css']
})
export class CompanyAddoreditComponent implements OnInit {
    currentUserModel: any = {};
    tblDisburseCompanyInfoModel: any = {};
    error: boolean = false;
    msgs: Message[] = [];
    isLoading: boolean = false;
    isEditMode: any;
    isShow: boolean = true;
    isActionDisabled: boolean = true;
    accountNo: string = "";
    constructor(private disbursementService: disbursementService, private messageService: MessageService, private route: ActivatedRoute, private authService: AuthenticationService) {
        this.authService.currentUser.subscribe(x => {
            this.currentUserModel = x;
        });
    }

    ngOnInit() {
    }
    companySave() {

        if (!this.tblDisburseCompanyInfoModel.companyName || !this.tblDisburseCompanyInfoModel.address || !this.tblDisburseCompanyInfoModel.phone) {
            this.msgs = [];
            this.msgs.push({ severity: 'error', summary: 'Warning! ', detail: 'Cannot be left blank' });
            this.error = true;
        }
        else {
            this.isLoading = true;
            this.disbursementService.save(this.tblDisburseCompanyInfoModel).pipe(first())
                .subscribe(
                    data => {
                        this.messageService.add({ severity: 'success', summary: 'Save successfully', detail: 'Disburse company info added' });
                       
                        setTimeout(() => {
                            this.isLoading = false;
                            location.reload();
                        }, 100);
                    },
                    error => {
                        console.log(error);
                        this.isLoading = false;
                    });
        }
    }
    companyDelete(event) {

    }
    cancel() {
        window.history.back();
    }

    enableSave(accountNo) {

        if ((!this.tblDisburseCompanyInfoModel.salAcc || this.tblDisburseCompanyInfoModel.salAcc.trim().length === 0)
            && (!this.tblDisburseCompanyInfoModel.remAcc || this.tblDisburseCompanyInfoModel.remAcc.trim().length === 0)
            && (!this.tblDisburseCompanyInfoModel.cabAcc || this.tblDisburseCompanyInfoModel.cabAcc.trim().length === 0)
            && (!this.tblDisburseCompanyInfoModel.catAcc || this.tblDisburseCompanyInfoModel.catAcc.trim().length === 0)
            && (!this.tblDisburseCompanyInfoModel.rwdAcc || this.tblDisburseCompanyInfoModel.rwdAcc.trim().length === 0)) {
            this.isActionDisabled = true;
            this.isShow = true;
        }
        else {
            //this.isActionDisabled = false;
            //this.isShow = false;

            this.disbursementService.GetAccountDetails(accountNo)
                .pipe(first())
                .subscribe(
                    data => {
                        if (data == null) {
                            //this.msgs = [];
                            //this.msgs.push({ severity: 'error', summary: 'A/C No : ' + this.accountNo, detail: 'is not valid!' });
                            this.messageService.add({ severity: 'error', summary: 'A/C No : ' + accountNo, detail: 'is not valid!' });
                            this.isActionDisabled = true;
                            this.isShow = true;
                            this.tblDisburseCompanyInfoModel.salAcc = null;
                            this.tblDisburseCompanyInfoModel.remAcc = null;
                            this.tblDisburseCompanyInfoModel.cabAcc = null;
                            this.tblDisburseCompanyInfoModel.catAcc = null;
                            this.tblDisburseCompanyInfoModel.rwdAcc = null;
                        }
                        else {
                            this.isActionDisabled = false;
                            this.isShow = false;
                        }
                    },
                    error => {
                        console.log(error);
                    }
                );

        }
    }
}
