import { Component, OnInit } from '@angular/core';
import { MerchantConfigService } from 'src/app/services/environment';
import { first } from 'rxjs/operators';
import { ActivatedRoute } from '@angular/router';
import { MessageService, Message } from 'primeng/api';
import { AuthenticationService } from 'src/app/shared/_services';

@Component({
    selector: 'app-merchant-config-addoredit',
    templateUrl: './merchant-config-addoredit.component.html',
    styleUrls: ['./merchant-config-addoredit.component.css']
})
export class MerchantConfigAddoreditComponent implements OnInit {
    merchantConfigModel: any = {};
    merchantConfigAcList: any = {};
    currentUserModel: any = {};
    statusList: any = {};
    isEditMode: any = true;
    msgs: Message[] = [];
    error: boolean = false;
    setAmount: any;
    type: string = "";

    constructor(private merchantConfigService: MerchantConfigService, private route: ActivatedRoute,
        private messageService: MessageService, private authService: AuthenticationService) {

        this.authService.currentUser.subscribe(x => {
            this.currentUserModel = x;
        });
    }

    ngOnInit() {
        this.getMerchantConfigListForDDL();

        this.statusList = [
            { label: 'Active', value: 'A' },
            { label: 'InActive', value: 'I' }
        ];
    }


    async getMerchantConfigListForDDL() {
        this.merchantConfigService.getMerchantConfigListForDDL()
            .pipe(first())
            .subscribe(
                data => {
                    this.merchantConfigAcList = data;
                },
                error => {
                    console.log(error);
                }
            );
    }

    GetMerchantConfigDetails() {
        this.merchantConfigService.GetMerchantConfigDetails(this.merchantConfigModel.mphone)
            .pipe(first())
            .subscribe(
                data => {
                    if (data != null) {
                        this.merchantConfigModel.mcode = data["MCODE"];
                        this.merchantConfigModel.status = data["STATUS"];
                        this.merchantConfigModel.customerServiceChargeFxt = data["CUSTOMER_SERVICE_CHARGE_FXT"];
                        this.merchantConfigModel.customerServiceChargePer = data["CUSTOMER_SERVICE_CHARGE_PER"] * 100;
                        this.merchantConfigModel.minTransAmt = data["MIN_TRANS_AMT"];
                        this.merchantConfigModel.semiconDbCharge = data["SEMICON_DB_CHARGE"];
                        this.merchantConfigModel.merchantCashoutCharge = data["MERCHANT_CASHOUT_CHARGE"] * 100;
                        this.merchantConfigModel.MerchantSmsNotification = data["MERCHANT_SMS_NOTIFICATION"];
                    }
                    else {
                        this.merchantConfigModel.mcode = null;
                        this.merchantConfigModel.status = null;
                        this.merchantConfigModel.customerServiceChargeFxt = 0;
                        this.merchantConfigModel.customerServiceChargePer = 0;
                        this.merchantConfigModel.minTransAmt = 0;
                        this.merchantConfigModel.semiconDbCharge = 0;
                        this.merchantConfigModel.merchantCashoutCharge = 0;
                        this.merchantConfigModel.MerchantSmsNotification = null;
                    }

                },
                error => {
                    console.log(error);
                }
            );
    }


    saveMerchantConfig(): any {

        if (!this.merchantConfigModel.mphone || this.merchantConfigModel.mphone == '') {
            this.msgs = [];
            this.msgs.push({ severity: 'error', summary: 'Warning! ', detail: 'Cannot be left blank' });
            this.error = true;
        }
        else {
            this.merchantConfigModel.customerServiceChargePer = this.merchantConfigModel.customerServiceChargePer / 100;
            this.merchantConfigModel.merchantCashoutCharge = this.merchantConfigModel.merchantCashoutCharge / 100;

            this.merchantConfigModel.UpdateBy = this.currentUserModel.user.username;

            if (this.merchantConfigModel.mphone != "" || !this.merchantConfigModel.mphone) {
                this.merchantConfigService.save(this.merchantConfigModel).pipe(first())
                    .subscribe(
                        data => {
                            window.history.back();

                            this.messageService.add({ severity: 'success', summary: 'Update successfully', detail: 'Merchant configuration updated' });


                        },
                        error => {
                            console.log(error);
                        });

            }
        }



    }

    cancel() {
        window.history.back();
    }

    async CheckAmount(setAmount, type) {
        if (setAmount > 100) {
            this.messageService.add({ severity: 'warn', summary: 'greater than 100', detail: 'Charge can not be greater than 100.' });
            if (type == "CSC") {
                this.merchantConfigModel.customerServiceChargePer = 0;
            }
            else {
                this.merchantConfigModel.merchantCashoutCharge = 0;
            }
        }

    }
}
