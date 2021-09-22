import { Component, OnInit } from '@angular/core';
import { GridSettingService } from 'src/app/services/grid-setting.service';
import { MfsSettingService } from 'src/app/services/mfs-setting.service';
import { disbursementService } from 'src/app/services/transaction';
import { first } from 'rxjs/operators';
import { MessageService, Message } from 'primeng/api';
import { AuthenticationService } from 'src/app/shared/_services';
import { ActivatedRoute } from '@angular/router';

@Component({
    selector: 'app-company-disbursement-limit',
    templateUrl: './company-disbursement-limit.component.html',
    styleUrls: ['./company-disbursement-limit.component.css']
})
export class CompanyDisbursementLimitComponent implements OnInit {

    tblDisburseAmtDtlMakeModel: any = {};
    error: boolean = false;
    msgs: Message[] = [];
    isLoading: boolean = false;
    disburseCompanyList: any;
    disburseTypeList: any;
    disburseNameCodeList: any;
    currentUserModel: any = {};
    isRegistrationPermitted: boolean = false;
    isEditMode: any;
    transAmtLimit: any;
    constructor(private messageService: MessageService, private disbursementService: disbursementService
        , private gridSettingService: GridSettingService, private mfsSettingService: MfsSettingService
        , private authService: AuthenticationService, private route: ActivatedRoute,) {

        this.authService.currentUser.subscribe(x => {
            this.currentUserModel = x;
        });

    }


    ngOnInit() {
        this.getDisburseCompanyList();
        this.getDisburseTypeList();
        this.getDisburseNameCodeList();
        this.isRegistrationPermitted = this.authService.checkRegisterPermissionAccess(this.route.snapshot.routeConfig.path);
    }
    getDisburseNameCodeList(): any {
        this.disbursementService.getDisburseNameCodeList()
            .pipe(first())
            .subscribe(
                data => {
                    this.disburseNameCodeList = data;
                },
                error => {
                    console.log(error);
                }
            );
    }
    getDisburseCompanyList(): any {
        this.disbursementService.getDisburseCompanyList()
            .pipe(first())
            .subscribe(
                data => {
                    this.disburseCompanyList = data;
                },
                error => {
                    console.log(error);
                }
            );
    }
    getDisburseTypeList(): any {
        this.disbursementService.getDisburseTypeList()
            .pipe(first())
            .subscribe(
                data => {
                    this.disburseTypeList = data;
                },
                error => {
                    console.log(error);
                }
            );
    }
    companyLimitSave() {
        this.tblDisburseAmtDtlMakeModel.brCode = this.currentUserModel.user.branchCode;
        if (!this.isRegistrationPermitted) {
            this.tblDisburseAmtDtlMakeModel.makerId = this.currentUserModel.user.username;
        }
        if (this.isRegistrationPermitted) {
            this.tblDisburseAmtDtlMakeModel.checkerId = this.currentUserModel.user.username;
        }
        if (!this.tblDisburseAmtDtlMakeModel.companyId || this.tblDisburseAmtDtlMakeModel.companyId == '' ||
            !this.tblDisburseAmtDtlMakeModel.disburseTp || this.tblDisburseAmtDtlMakeModel.disburseTp == '' ||
            !this.tblDisburseAmtDtlMakeModel.amountCr || this.tblDisburseAmtDtlMakeModel.amountCr == '' ||
            !this.tblDisburseAmtDtlMakeModel.refNo || this.tblDisburseAmtDtlMakeModel.refNo == '' ||
            !this.tblDisburseAmtDtlMakeModel.accNo || this.tblDisburseAmtDtlMakeModel.accNo == '') {
            this.msgs = [];
            this.msgs.push({ severity: 'error', summary: 'Warning! ', detail: 'Cannot be left blank' });
            this.error = true;
        }
        else {
            this.isLoading = true;
            this.disbursementService.saveCompanyLimit(this.tblDisburseAmtDtlMakeModel).pipe(first())
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
                    });


        }
    }
    companyDelete(event) {

    }
    cancel() {
        window.history.back();
    }


    async checkAmountWithLimit() {
        if (this.tblDisburseAmtDtlMakeModel.amountCr == '') {
            this.tblDisburseAmtDtlMakeModel.amountCr = 0;
        }
        this.transAmtLimit = this.currentUserModel.user.tranAmtLimit;
        if (+this.tblDisburseAmtDtlMakeModel.amountCr > this.transAmtLimit) {
            this.messageService.add({ severity: 'warn', summary: 'Exceed Limit', detail: 'Your Limit Amount :' + this.transAmtLimit });
            this.tblDisburseAmtDtlMakeModel.amountCr = 0;
        }
        else {

        }





    }

}
