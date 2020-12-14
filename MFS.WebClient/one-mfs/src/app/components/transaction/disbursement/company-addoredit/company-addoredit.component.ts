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
    TargetCatTypeList: any;
    entityId: string;
    isRegistrationPermitted: boolean = false;
    branchCode: any;

    constructor(private disbursementService: disbursementService, private messageService: MessageService, private route: ActivatedRoute, private authService: AuthenticationService) {
        this.authService.currentUser.subscribe(x => {
            this.currentUserModel = x;
        });
    }

    ngOnInit() {

        this.TargetCatTypeList = [
            { label: 'Distributor', value: 'D' },
            { label: 'Agent', value: 'A' },
            { label: 'Customer', value: 'C' },
            { label: 'Merchant', value: 'M' },
            { label: 'Any', value: '' }
        ];

        this.entityId = this.route.snapshot.paramMap.get('id');
        if (this.entityId) {
            this.isEditMode = true;
            this.getCompanyInfoByCompanyId();

            this.isRegistrationPermitted = this.authService.checkRegisterPermissionAccess(this.route.snapshot.routeConfig.path);
        }
    }

    getCompanyInfoByCompanyId(): any {
        this.disbursementService.getCompanyInfoByCompanyId(this.entityId)
            .pipe(first())
            .subscribe(
                data => {
                    this.tblDisburseCompanyInfoModel = data;

                },
                error => {
                    console.log(error);
                }
            )
    }


    companySave() {

        if (!this.isEditMode) {
            if (!this.tblDisburseCompanyInfoModel.companyName
                || !this.tblDisburseCompanyInfoModel.address
                //|| !this.tblDisburseCompanyInfoModel.targetCatId || this.tblDisburseCompanyInfoModel.targetCatId == ''
                || !this.tblDisburseCompanyInfoModel.phone) {
                this.msgs = [];
                this.msgs.push({ severity: 'error', summary: 'Warning! ', detail: 'Cannot be left blank' });
                this.error = true;
            }
            else {
                this.isLoading = true;
                this.tblDisburseCompanyInfoModel.entry_user = this.currentUserModel.user.username;
                this.disbursementService.save(this.tblDisburseCompanyInfoModel).pipe(first())
                    .subscribe(
                        data => {
                            this.messageService.add({ severity: 'success', summary: 'Save successfully', detail: 'Disburse company info added' });

                            setTimeout(() => {
                                this.isLoading = false;
                                location.reload();
                            }, 5000);
                        },
                        error => {
                            console.log(error);
                            this.isLoading = false;
                        });
            }
        }
        else {
            this.isLoading = true;
            this.branchCode = this.currentUserModel.user.branchCode;
            this.tblDisburseCompanyInfoModel.entry_user = this.currentUserModel.user.username;
            this.disbursementService.ApproveRefundDisburseAmount(this.tblDisburseCompanyInfoModel, this.branchCode).pipe(first())
                .subscribe(
                    data => {

                        if (this.isRegistrationPermitted) {
                            if (data == "1") {
                                this.messageService.add({ severity: 'success', summary: 'Refunded successfully', detail: 'Disbursement amount refunded' });
                            }
                            else {
                                this.messageService.add({ severity: 'error', summary: 'Not Approved', detail: data });
                            }
                        }
                        setTimeout(() => {
                            this.isLoading = false;
                            location.reload();
                        }, 5000);
                        //window.history.back();
                    },
                    error => {
                        console.log(error);
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
            && (!this.tblDisburseCompanyInfoModel.incAcc || this.tblDisburseCompanyInfoModel.incAcc.trim().length === 0)
            && (!this.tblDisburseCompanyInfoModel.eftAcc || this.tblDisburseCompanyInfoModel.eftAcc.trim().length === 0)
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
                            this.tblDisburseCompanyInfoModel.incAcc = null;
                            this.tblDisburseCompanyInfoModel.eftAcc = null;
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

    checkAndEnableApprove() {

        this.isLoading = true;
        if (this.tblDisburseCompanyInfoModel.refund_amt <= this.tblDisburseCompanyInfoModel.bala_nce) {
            this.isActionDisabled = false;
            this.isLoading = false;
        }
        else {
            this.messageService.add({ severity: 'warn', summary: 'Exceed Limit', detail: 'Refund Amount can not be greater than balance' });
            this.tblDisburseCompanyInfoModel.refund_amt = null;
            this.isLoading = false;
            this.isActionDisabled = true;
        }



    }
}
