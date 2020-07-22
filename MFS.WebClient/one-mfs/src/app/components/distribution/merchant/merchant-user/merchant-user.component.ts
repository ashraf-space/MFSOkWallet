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
        this.getMerchantList();
        this.entityId = this.route.snapshot.paramMap.get('id');
        if (this.entityId) {
            this.isEditMode = true;
            this.getMerChantUserByMphone();
            this.isRegistrationPermitted = this.authService.checkRegisterPermissionAccess(this.route.snapshot.routeConfig.path);
        }
        else {
            this.merchantUserModel = {};           
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
        if ((this.merchantUserModel.password && this.merchantUserModel.confirmpassword) && (this.merchantUserModel.password === this.merchantUserModel.confirmpassword)) {
            this.merchantUserModel.insertBy = this.currentUserModel.user.username;
            if (this.entityId) {
                this.merchantUserModel.updateBy = this.currentUserModel.user.username;
            }
            this.merchantUserModel.plainPassword = this.merchantUserModel.password;
            this.merchantService.onMerchantUserSave(this.merchantUserModel, this.isEditMode, event).pipe(first())
                .subscribe(
                    data => {
                        if (data) {
                            window.history.back();
                            if (this.isEditMode)
                                this.messageService.add({ severity: 'success', summary: 'Update successfully', detail: 'Merchant updated' });
                            else
                                this.messageService.add({ severity: 'success', summary: 'Save successfully', detail: 'Merchant added' });
                        }
                    },
                    error => {
                        console.log(error);
                    });
        }
        else {
            this.messageService.add({ severity: 'error', summary: 'Input Password', detail: 'Please Input Password Correctly' });
        }      

    }

    checkMerchantUserAlreadyExist() {
        this.isLoading = true;
        this.merchantService.getMerChantUserByMphone(this.merchantUserModel.mphone)
            .pipe(first())
            .subscribe(
                data => {
                    if (data) {
                        this.isLoading = false;
                        this.merchantUserModel.mphone = '';
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
