import { Component, OnInit } from '@angular/core';
import { MerchantService, DistributorService } from 'src/app/services/distribution';
import { Router, ActivatedRoute } from '@angular/router';
import { MessageService, MenuItem, Message } from 'primeng/api';
import { AuthenticationService } from 'src/app/shared/_services';
import { first } from 'rxjs/operators';
import { MfsUtilityService } from 'src/app/services/mfs-utility.service';
@Component({
    selector: 'app-merchant-user',
    templateUrl: './merchant-user.component.html',
    styleUrls: ['./merchant-user.component.css']
})
export class MerchantUserComponent implements OnInit {
    currentUserModel: any = {};
    isRegistrationPermitted: any;
    merchantUserModel: any = {};
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
    merchantSubTypeList: any;
    constructor(private merchantService: MerchantService, private distributorService: DistributorService, private router: Router,
        private route: ActivatedRoute, private messageService: MessageService, private authService: AuthenticationService,
        private mfsUtilityService: MfsUtilityService) {
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
        this.merchantTypeList = [
            { label: 'Individual', value: 'I' },
            { label: 'Parent Merchant', value: 'PM' },
            { label: 'Child Merchantt', value: 'CM' },
            { label: 'Master Wallet', value: 'MW' },
            { label: 'Distributor', value: 'D' },
            { label: 'Bank', value: 'BNK' },
            { label: 'Donation', value: 'DON' },
            { label: 'Ekpay', value: 'EKPAY' },
            { label: 'SSL COMMERZ', value: 'SSL' }
        ]
        this.merchantSubTypeList = [
            { label: 'Jamuna Bank', value: 'JBL' },
            { label: 'Mutual Trust Bank', value: 'MTB' },
            { label: 'Brac Bank Limited', value: 'BBL' } 
        ]
        this.getMerchantList();
        this.entityId = +this.route.snapshot.paramMap.get('id');
        if (this.entityId) {
            this.isEditMode = true;
            this.getMerChantUserById();
            this.isRegistrationPermitted = this.authService.checkRegisterPermissionAccess(this.route.snapshot.routeConfig.path);
        }
        else {
            this.merchantUserModel = {};
        }
    }
    getMerChantUserById() {
        this.isLoading = true;
        this.merchantService.getMerChantUserById(this.entityId)
            .pipe(first())
            .subscribe(
                data => {
                    if (data) {
                        this.merchantUserModel = data;
                        this.merchantUserModel.password = data.plainPassword;
                        this.merchantUserModel.confirmpassword = data.plainPassword;
                        this.isLoading = false;
                    }

                },
                error => {
                    console.log(error);
                }
            )
    }
    checkMphoneAlreadyExist(): any {        
        if (this.merchantUserModel.mobileNo.toString().substring(0, 2) == "01" && this.merchantUserModel.mobileNo.toString().substring(0, 3) != "012") {
            this.distributorService.GetDistributorByMphone(this.merchantUserModel.mobileNo)
                .pipe(first())
                .subscribe(
                    data => {
                        if (data != null) {
                            if (this.merchantService.isMerchantAllow(data.catId)) {
                                this.checkMerchantUserAlreadyExist();
                            }
                            else {
                                this.merchantUserModel.mobileNo = '';
                                this.messageService.add({ severity: 'error', summary: 'Invalid User', detail: 'Invalid User For Portal', closable: true });
                            }
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
            this.merchantUserModel.mobileNo = '';
            this.messageService.add({ severity: 'error', summary: 'Invalid Mobile No', detail: 'Please Input Valid Mobiel No', closable: true });
        }
    }
    getMerchantList() {
        this.isLoading = true;
        this.merchantService.getMerchantList()
            .pipe(first())
            .subscribe(
                data => {
                    this.merchantList = data;
                    this.merchantList.unshift({ label: 'Please Select', value: null });
                    this.isLoading = false;
                },
                error => {
                    console.log(error);
                }
            );
    }
    getMerChantUserByMphone() {
        this.isLoading = true;
        this.merchantService.getMerChantUserByMphone(this.entityId)
            .pipe(first())
            .subscribe(
                data => {
                    if (data) {
                        this.merchantUserModel = data;
                        this.isLoading = false;
                    }

                },
                error => {
                    console.log(error);
                }
            )
    }
    onMerchantUserSave(event) {
        if ((this.merchantUserModel.password && this.merchantUserModel.confirmpassword && this.merchantUserModel.username && this.merchantUserModel.mtype) && (this.merchantUserModel.password === this.merchantUserModel.confirmpassword)) {
            this.merchantUserModel.insertBy = this.currentUserModel.user.username;
            if (this.entityId) {
                this.merchantUserModel.updateBy = this.currentUserModel.user.username;
            }
            this.merchantUserModel.plainPassword = this.merchantUserModel.password;
            this.merchantService.onMerchantUserSave(this.merchantUserModel, this.isEditMode, event).pipe(first())
                .subscribe(
                    data => {
                        if (data === 200) {
                            window.history.back();
                            if (this.isEditMode)
                                this.messageService.add({ severity: 'success', summary: 'Update successfully', detail: 'Merchant updated' });
                            else
                                this.messageService.add({ severity: 'success', summary: 'Save successfully', detail: 'Merchant added' });
                        }
                        else {
                            this.messageService.add({ severity: 'error', summary: 'Erros in: ' + this.merchantUserModel.mphone, sticky: true, detail: 'Bad Response from BackEnd', closable: true });
                        }
                    },
                    error => {
                        console.log(error);
                    });
        }
        else {
            this.messageService.add({ severity: 'error', summary: 'Input Properly', detail: 'Please Input Information Correctly' });
        }

    }

    checkMerchantUserAlreadyExist() {
        this.isLoading = true;
        this.merchantService.checkMerchantUserAlreadyExist(this.merchantUserModel.username)
            .pipe(first())
            .subscribe(
                data => {
                    if (data) {
                        this.isLoading = false;
                        this.merchantUserModel.username = '';
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
