import { Component, OnInit } from '@angular/core';
import { MerchantService, DistributorService } from 'src/app/services/distribution';
import { Router, ActivatedRoute } from '@angular/router';
import { MessageService, MenuItem, Message } from 'primeng/api';
import { AuthenticationService } from 'src/app/shared/_services';
import { first } from 'rxjs/operators';
import { MfsUtilityService } from 'src/app/services/mfs-utility.service';
@Component({
    selector: 'app-merchant-config',
    templateUrl: './merchant-config.component.html',
    styleUrls: ['./merchant-config.component.css']
})
export class MerchantConfigComponent implements OnInit {
    currentUserModel: any = {};
    formValidation: any;
    positveNumber: RegExp;
    merchantList: any;
    merchantConfigModel: any = {};
    entityId: any;
    isEditMode: any;
    isRegistrationPermitted: any;
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
        this.getAllMerchant();
        this.smsStatusList = [
            { label: 'Yes', value: 'A' },
            { label: 'No', value: 'I' }
        ]

        this.statusList = [
            { label: 'Active', value: 'A' },
            { label: 'Inactive', value: 'I' }
        ]
        this.entityId = this.route.snapshot.paramMap.get('id');
        if (this.entityId) {
            this.isEditMode = true;
            this.getMerChantConfigByMphone();
            this.isRegistrationPermitted = this.authService.checkRegisterPermissionAccess(this.route.snapshot.routeConfig.path);
        }
    }

    getAllMerchant() {
        this.merchantService.getAllMerchant()
            .pipe(first())
            .subscribe(
                data => {
                    this.merchantList = data;
                },
                error => {
                    console.log(error);
                }
            );
    }

    getMerChantConfigByMphone() {
        this.isLoading = true;
        this.merchantService.getMerChantConfigByMphone(this.entityId)
            .pipe(first())
            .subscribe(
                data => {
                    if (data) {
                        this.merchantConfigModel = data;
                        this.selectedSmsStatus = data.merchantSmsNotification;
                        this.isLoading = false;
                    }

                },
                error => {
                    console.log(error);
                }
            )
    }
    onMerchantConfigUpdate(event) {
        //this.merchantConfigModel.merchantSmsNotification = this.selectedSmsStatus;
        this.merchantConfigModel.updateBy = this.currentUserModel.user.username;

        this.merchantService.onMerchantConfigUpdate(this.merchantConfigModel, event).pipe(first())
            .subscribe(
                data => {
                    if (data) {
                        window.history.back();
                        this.messageService.add({ severity: 'success', summary: 'Update successfully', detail: 'Merchant updated' });
                    }
                },
                error => {
                    console.log(error);
                });



    }
}
