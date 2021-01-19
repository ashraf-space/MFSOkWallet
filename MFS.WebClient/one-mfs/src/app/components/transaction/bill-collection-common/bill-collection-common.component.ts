import { Component, OnInit, OnDestroy } from '@angular/core';
import { MessageService } from 'primeng/api';
import { AuthenticationService } from 'src/app/shared/_services';
import { ActivatedRoute, Router, NavigationEnd, NavigationStart, Event } from '@angular/router';
import { BillCollectionCommonService } from 'src/app/services/transaction/bill-collection-common.service';
import { first } from 'rxjs/operators';


@Component({
    selector: 'app-bill-collection-common',
    templateUrl: './bill-collection-common.component.html',
    styleUrls: ['./bill-collection-common.component.css']
})
export class BillCollectionCommonComponent implements OnInit {
    featureId: number = 0;
    currentUserModel: any = {};
    billCollectionCommonModel: any = {};
    featurePayModel: any = {};
    error: boolean = false;
    msgs: any[];
    isActionDisabled: boolean = true;
    isCheckDisabled: boolean = true;
    isNextDisabled: boolean = true;
    isLoading: boolean = false;
    mySubscription: any;
    monthYearList: any;
    isShowMonth: boolean = false;
    isShowCardHolder: boolean = false;
    subMenuList: any;
    isShowSubMenuDDL: boolean = false;
    isAmountDisabled: boolean = true;
    message: string = null;
    //bill2: string = null;
    isShowMessage: boolean = false;

    isDisabledSubMenuDDL: boolean = false;
    isDisabledMonth: boolean = false;
    isDisabledBillId: boolean = false;
    isDisabledCard: boolean = false;
    isDisabledBeneficiary: boolean = false;
    initiateModal: boolean = false;
    fee: any;
    total: number = 0;
    branchCode: any;
    userName: any;
    glue: any;
    isConfirmDisabled: boolean = false;
    isBgColorYellow: boolean = true;


    constructor(private messageService: MessageService, private billCollectionCommonService: BillCollectionCommonService
        , private authService: AuthenticationService
        , private route: ActivatedRoute, private router: Router) {

        this.authService.currentUser.subscribe(x => {
            this.currentUserModel = x;
        });


        //this.router.routeReuseStrategy.shouldReuseRoute = function () {
        //    return false;
        //};

        this.router.events.subscribe((event: Event) => {
            if (event instanceof NavigationStart) {
                this.ngOnInit();
            }
            if (event instanceof NavigationEnd) {
                // Trick the Router into believing it's last link wasn't previously loaded
                //this.router.navigated = false;
                this.ngOnInit();
            }
        });

    }



    ngOnInit() {
        //var eventLog = JSON.parse(sessionStorage.getItem('currentEvent'));
        //this.featureId = eventLog.item.featureId;
        this.message = null;
        this.error = false;
        this.isShowMessage = false;
        this.isBgColorYellow = true;


        this.isCheckDisabled = false;
        this.isNextDisabled = true;
        this.isDisabledSubMenuDDL = false;
        this.isDisabledMonth = false;
        this.isDisabledBillId = false;
        this.isDisabledCard = false;
        this.isDisabledBeneficiary = false;
        this.isAmountDisabled = true;

        this.isConfirmDisabled = false;


        this.featureId = +this.route.snapshot.paramMap.get('id');



        this.billCollectionCommonModel = {};
        this.billCollectionCommonService.GetFeaturePayDetails(this.featureId)
            .pipe(first())
            .subscribe(
                data => {
                    this.featurePayModel = data;
                    this.billCollectionCommonModel.ParentPenuId = data.PARENTPENUID;
                    this.billCollectionCommonModel.Title = data.TITLE;
                    this.billCollectionCommonModel.MethodName = data.METHODNAME;
                    this.billCollectionCommonModel.OnlineCall = data.ONLINECALL;

                    if (this.featurePayModel.MONTHTITLE != null) {
                        this.isShowMonth = true;
                        this.LoadMonthYearList();
                    }
                    else {
                        this.isShowMonth = false;
                        this.monthYearList = null;
                    }

                    if (this.featurePayModel.MOREBILLTITLE != null) {
                        this.isShowCardHolder = true;
                    }
                    else {
                        this.isShowCardHolder = false;
                    }

                    if (this.featurePayModel.SUBMENUTITLE != null) {
                        this.isShowSubMenuDDL = true;
                        this.LoadSubMenuDDL(this.featureId);
                    }
                    else {
                        this.isShowSubMenuDDL = false;
                        this.subMenuList = null;
                    }

                    if (data.ONLINECALL == "Y") {
                        this.isCheckDisabled = false;
                        this.isActionDisabled = true;
                        this.isNextDisabled = true;
                        this.isAmountDisabled = true;
                    }
                    else {
                        this.isCheckDisabled = true;
                        this.isNextDisabled = false;
                        this.isAmountDisabled = false;
                        //this.isActionDisabled = false;
                    }

                },
                error => {
                    console.log(error);
                }
            );
    }


    LoadMonthYearList(): any {
        this.billCollectionCommonService.GetMonthYearList()
            .pipe(first())
            .subscribe(
                data => {
                    this.monthYearList = data;
                },
                error => {
                    console.log(error);
                }
            );
    }

    LoadSubMenuDDL(featureId: number): any {
        this.billCollectionCommonService.GetSubMenuDDL(featureId)
            .pipe(first())
            .subscribe(
                data => {
                    this.subMenuList = data;
                },
                error => {
                    console.log(error);
                }
            );
    }

    onCheck(): any {
        this.error = false;  
        this.isBgColorYellow = true;

        if (!this.billCollectionCommonModel.billId || this.billCollectionCommonModel.billId == '' || this.billCollectionCommonModel.billId == '0') {
            this.msgs = [];
            this.msgs.push({ severity: 'error', summary: 'Warning! ', detail: 'Cannot be left blank' });
            this.error = true;
        }
        if (!this.billCollectionCommonModel.beneficiaryNumber || this.billCollectionCommonModel.beneficiaryNumber == '' || this.billCollectionCommonModel.beneficiaryNumber == '0') {
            this.msgs = [];
            this.msgs.push({ severity: 'error', summary: 'Warning! ', detail: 'Cannot be left blank' });
            this.error = true;
        }

        if (this.billCollectionCommonModel.beneficiaryNumber.substring(0,2) != '01') {
            this.isShowMessage = true;
            this.isBgColorYellow = false;
            this.message = "Beneficiary number must start with 01";
            this.error = true;
        }

        if (this.featurePayModel.MONTHTITLE != null) {
            if (!this.billCollectionCommonModel.month || this.billCollectionCommonModel.month == '' || this.billCollectionCommonModel.month == '0') {
                this.msgs = [];
                this.msgs.push({ severity: 'error', summary: 'Warning! ', detail: 'Cannot be left blank' });
                this.error = true;
            }
        }

        if (this.featurePayModel.MOREBILLTITLE != null) {
            if (!this.billCollectionCommonModel.cardHolderName || this.billCollectionCommonModel.cardHolderName == '' || this.billCollectionCommonModel.cardHolderName == '0') {
                this.msgs = [];
                this.msgs.push({ severity: 'error', summary: 'Warning! ', detail: 'Cannot be left blank' });
                this.error = true;
            }
        }

        if (this.featurePayModel.SUBMENUTITLE != null) {
            if (!this.billCollectionCommonModel.subMenuId || this.billCollectionCommonModel.subMenuId == '' || this.billCollectionCommonModel.subMenuId == '0') {
                this.msgs = [];
                this.msgs.push({ severity: 'error', summary: 'Warning! ', detail: 'Cannot be left blank' });
                this.error = true;
            }
        }
        if (!this.error) {
            this.isLoading = true;

            this.billCollectionCommonService.CheckBillInfo(this.billCollectionCommonModel).pipe(first())
                .subscribe(
                    data => {

                        this.message = data.msg;
                        this.glue = data.glue;
                        //this.bill2 = data.bill2;
                        this.isShowMessage = true;

                        this.isCheckDisabled = true;
                        //this.isNextDisabled = false;
                        this.isDisabledSubMenuDDL = true;
                        this.isDisabledMonth = true;
                        this.isDisabledBillId = true;
                        this.isDisabledCard = true;
                        this.isDisabledBeneficiary = true;

                        this.isLoading = false;

                        if (data.status == "true") {
                            this.isActionDisabled = false;
                            this.isNextDisabled = false;
                            this.billCollectionCommonModel.amount = data.amount;

                            if (this.featurePayModel.MOREBILLTITLE != null) {
                                if (!this.billCollectionCommonModel.cardHolderName || this.billCollectionCommonModel.cardHolderName == '' || this.billCollectionCommonModel.cardHolderName == '0') {
                                    this.billCollectionCommonModel.bill2 = data.bill2;
                                }
                                else {
                                    this.billCollectionCommonModel.bill2 = data.bill2 + this.billCollectionCommonModel.cardHolderName;
                                }
                            }
                            else {
                                this.billCollectionCommonModel.bill2 = data.bill2;
                            }



                            if (data.amount == "0") {
                                this.isAmountDisabled = false;
                                this.billCollectionCommonModel.amount = '';
                            }
                            else {
                                this.isAmountDisabled = true;
                            }
                        }
                        else {
                            this.isActionDisabled = true;
                            //this.billCollectionCommonModel.amount = 0;
                            this.billCollectionCommonModel.amount = '';
                        }

                        //setTimeout(() => {
                        //    this.isLoading = false;
                        //    location.reload();
                        //}, 5000);


                    },
                    error => {
                        console.log(error);
                    });


        }
    }

    onNext(): any {
        if (!this.billCollectionCommonModel.amount || this.billCollectionCommonModel.amount == '' || this.billCollectionCommonModel.amount == '0') {
            this.isShowMessage = true;
            this.isBgColorYellow = false;
            this.message = "Amount must be greater than 0";
            //this.error = true;
        }
        if (+this.billCollectionCommonModel.amount > 0) {
            this.isLoading = true;
            this.billCollectionCommonService.GetFeeInfo(this.billCollectionCommonModel).pipe(first())
                .subscribe(
                    data => {
                        this.isLoading = false;
                        if (data.status == "true") {
                            this.fee = data.fee;
                            this.initiateModal = true;
                            var x: number = +this.billCollectionCommonModel.amount;
                            var y: number = +this.fee;
                            this.total = x + y;
                        }
                        else {
                            this.initiateModal = false;
                            this.isShowMessage = true;
                            this.isBgColorYellow = false;
                            this.message = data.msg;
                        }


                    },
                    error => {
                        console.log(error);
                    });


        }
    }


    onConfirmClick() {
        this.isLoading = true;
        this.isConfirmDisabled = true;
        this.branchCode = this.currentUserModel.user.branchCode;
        this.userName = this.currentUserModel.user.username;

        if (this.glue) {
            if (this.featurePayModel.MOREBILLTITLE != null) {
                this.billCollectionCommonModel.bill2 = this.billCollectionCommonModel.bill2 + this.glue + this.branchCode + this.glue + this.userName + this.glue;
            }
            else {
                this.billCollectionCommonModel.bill2 = this.billCollectionCommonModel.bill2 + this.branchCode + this.glue + this.userName + this.glue;
            }

        }
        else {
            if (this.featurePayModel.MOREBILLTITLE != null) {
                this.billCollectionCommonModel.bill2 = this.billCollectionCommonModel.bill2 + ',' + this.branchCode + ',' + this.userName + ',';
            }
            else {
                if (this.billCollectionCommonModel.bill2) {
                    this.billCollectionCommonModel.bill2 = this.billCollectionCommonModel.bill2 + this.branchCode + ',' + this.userName + ',';
                }
                else {
                    this.billCollectionCommonModel.bill2 = this.branchCode + ',' + this.userName + ',';
                }
                
            }

        }

        this.billCollectionCommonModel.EntryUser = this.userName;
        this.billCollectionCommonService.confirmBill(this.billCollectionCommonModel).pipe(first())
            .subscribe(
                data => {
                    this.initiateModal = false;
                    if (data.status == "true") {
                        this.messageService.add({ severity: 'success', sticky: true, summary: 'Payment successful', detail: data.msg });
                        this.isNextDisabled = true;
                    }
                    else {
                        this.messageService.add({ severity: 'error', sticky: true, summary: 'Failed', detail: data.msg });
                    }
                    setTimeout(() => {
                        this.isLoading = false;
                        location.reload();
                    }, 5000);
                },
                error => {
                    console.log(error);
                });
    }

    refresh() {
        this.ngOnInit();
    }



}
