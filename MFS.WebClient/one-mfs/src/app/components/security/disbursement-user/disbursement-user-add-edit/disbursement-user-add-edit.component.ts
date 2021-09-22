import { Component, OnInit } from '@angular/core';
import { DisbursementUserService } from 'src/app/services/security/disbursement-user.service';
import { DistributorService } from 'src/app/services/distribution';
import { Router, ActivatedRoute } from '@angular/router';
import { MessageService } from 'primeng/api';
import { AuthenticationService } from 'src/app/shared/_services';
import { MfsUtilityService } from 'src/app/services/mfs-utility.service';
import { first } from 'rxjs/operators';
import { disbursementService } from 'src/app/services/transaction';

@Component({
  selector: 'app-disbursement-user-add-edit',
  templateUrl: './disbursement-user-add-edit.component.html',
  styleUrls: ['./disbursement-user-add-edit.component.css']
})
export class DisbursementUserAddEditComponent implements OnInit {

    currentUserModel: any = {};
    isRegistrationPermitted: any;
    disbursementUserModel: any = {};
    userdata: any = {};
    formValidation: any;
    positveNumber: RegExp;
    merchantList: any;
    entityId: any;
    isEditMode: boolean = false;
    smsStatusList: any;
    statusList: any;
    error: boolean = false;
    selectedSmsStatus: any;
    isLoading: boolean = false;
    merchantTypeList: any;
    disburseCompanyList: any;
    roleNameList: any;

    constructor(private disbursementUserService: DisbursementUserService, private distributorService: DistributorService, private router: Router,
        private route: ActivatedRoute, private messageService: MessageService, private authService: AuthenticationService,
        private mfsUtilityService: MfsUtilityService, private disbursementService: disbursementService) {
        this.authService.currentUser.subscribe(x => {
            this.currentUserModel = x;
        });
        this.formValidation = {};
        this.positveNumber = this.mfsUtilityService.getPositiveWholeNumberRegExp();
    }

    ngOnInit() {
        this.statusList = [
            { label: 'Active', value: 'A' },
            { label: 'Inactive', value: 'I' }
        ]

        this.roleNameList = [
            { label: 'Maker', value: 'Maker' },
            { label: 'Checker', value: 'Checker' },
            { label: 'Both', value:'Both'}
        ]

       
        this.getDisburseCompanyList();
        //this.getMerchantList();
        this.entityId = +this.route.snapshot.paramMap.get('id');
        if (this.entityId) {
            this.isEditMode = true;
            this.getDisbursementUserById();
            this.isRegistrationPermitted = this.authService.checkRegisterPermissionAccess(this.route.snapshot.routeConfig.path);
        }
        else {
            this.disbursementUserModel = {};
        }
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

    getDisbursementUserById() {
        this.isLoading = true;
        this.disbursementUserService.getDisbursementUserById(this.entityId)
            .pipe(first())
            .subscribe(
                data => {
                    if (data) {
                        this.disbursementUserModel = data;
                        this.disbursementUserModel.password = data.plainPassword;
                        this.disbursementUserModel.confirmpassword = data.plainPassword;
                        this.isLoading = false;
                    }

                },
                error => {
                    console.log(error);
                }
            )
    }
    checkMphoneAlreadyExist(): any {
        if (this.disbursementUserModel.mobileNo.toString().substring(0, 2) == "01" && this.disbursementUserModel.mobileNo.toString().substring(0, 3) != "012") {
            this.distributorService.GetDistributorByMphone(this.disbursementUserModel.mobileNo)
                .pipe(first())
                .subscribe(
                    data => {
                        if (data != null) {
                            //if (this.disbursementUserService.isMerchantAllow(data.catId)) {
                            //    this.checkDisbursementUserAlreadyExist();
                            //}
                            //else {
                            //    this.disbursementUserModel.mobileNo = '';
                            //    this.messageService.add({ severity: 'error', summary: 'Invalid User', detail: 'Invalid User For Portal', closable: true });
                            //}

                            
                        }
                        else {

                        }
                    },
                    error => {
                        console.log(error);
                    }
                );
        }
        else {
            this.disbursementUserModel.mobileNo = '';
            this.messageService.add({ severity: 'error', summary: 'Invalid Mobile No', detail: 'Please Input Valid Mobiel No', closable: true });
        }
    }
    //getMerchantList() {
    //    this.isLoading = true;
    //    this.disbursementUserService.getMerchantList()
    //        .pipe(first())
    //        .subscribe(
    //            data => {
    //                this.merchantList = data;
    //                this.merchantList.unshift({ label: 'Please Select', value: null });
    //                this.isLoading = false;
    //            },
    //            error => {
    //                console.log(error);
    //            }
    //        );
    //}
    getMerChantUserByMphone() {
        this.isLoading = true;
        this.disbursementUserService.getDisbursementUserById(this.entityId)
            .pipe(first())
            .subscribe(
                data => {
                    if (data) {
                        this.disbursementUserModel = data;
                        this.isLoading = false;
                    }

                },
                error => {
                    console.log(error);
                }
            )
    }
    onDisbursementUserSave(event) {
        if ((this.disbursementUserModel.password && this.disbursementUserModel.confirmpassword && this.disbursementUserModel.username) && (this.disbursementUserModel.password === this.disbursementUserModel.confirmpassword)) {
            this.disbursementUserModel.insertBy = this.currentUserModel.user.username;
            if (this.entityId) {
                this.disbursementUserModel.updateBy = this.currentUserModel.user.username;
            }
            this.disbursementUserModel.plainPassword = this.disbursementUserModel.password;
            this.disbursementUserService.save(this.disbursementUserModel).pipe(first())
                .subscribe(
                    data => {
                        if (data === 200) {
                            window.history.back();
                            if (this.isEditMode)
                                this.messageService.add({ severity: 'success', summary: 'Update successfully', detail: 'User updated' });
                            else
                                this.messageService.add({ severity: 'success', summary: 'Save successfully', detail: 'User added' });
                        }
                        else {
                            this.messageService.add({ severity: 'error', summary: 'Erros in: ' + this.disbursementUserModel.mphone, sticky: true, detail: 'Bad Response from BackEnd', closable: true });
                        }
                    },
                    error => {
                        console.log(error);
                    });
        }
        else {
            this.messageService.add({ severity: 'error', summary: 'Input Password', detail: 'Please Input Information Correctly' });
        }

    }

    checkDisbursementUserAlreadyExist() {
        this.isLoading = true;
        this.disbursementUserService.CheckDisbursementUserAlreadyExist(this.disbursementUserModel.username)
            .pipe(first())
            .subscribe(
                data => {
                    if (data) {
                        this.isLoading = false;
                        this.disbursementUserModel.username = '';
                        this.messageService.add({ severity: 'error', summary: 'Exist', detail: 'User is already exist' });
                    }
                    else {
                        this.isLoading = false;
                    }

                },
                error => {
                    this.isLoading = false;
                    console.log(error);
                }
            )
    }

}
