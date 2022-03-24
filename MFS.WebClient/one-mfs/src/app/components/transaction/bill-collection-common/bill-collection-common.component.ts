import { Component, OnInit, OnDestroy } from '@angular/core';
import { MessageService } from 'primeng/api';
import { AuthenticationService } from 'src/app/shared/_services';
import { ActivatedRoute, Router, NavigationEnd, NavigationStart, Event } from '@angular/router';
import { BillCollectionCommonService } from 'src/app/services/transaction/bill-collection-common.service';
import { first } from 'rxjs/operators';
import { GridSettingService } from 'src/app/services/grid-setting.service';
import { ReceiptapiService } from 'src/app/shared/_services';
import { Injectable } from '@angular/core';
@Injectable()

@Component({
    selector: 'app-bill-collection-common',
    templateUrl: './bill-collection-common.component.html',
    styleUrls: ['./bill-collection-common.component.css']
})
export class BillCollectionCommonComponent implements OnInit {
    gridConfig: any;
    featureId: number = 0;
    currentUserModel: any = {};
    obj: any = {};
    billCollectionCommonModel: any = {};
    featurePayModel: any = {};
    branchPortalReceipt: any = {};
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
    isShowMMSGrid: boolean = false;
    exGridConfig: any;
    FeeCollectionModel: any = {};
    selectedFeeCollectionModel: any = {};

    sum: number = 0;
    ids: string = '';
    checkedBill2: string = '';
    isShowEnterAmount: boolean = true;

    receiptUrl: any;
    thermalReceiptUrl: any;


    constructor(private messageService: MessageService, private billCollectionCommonService: BillCollectionCommonService
        , private authService: AuthenticationService
        , private route: ActivatedRoute, private router: Router, private gridSettingService: GridSettingService
        , private receiptapiService: ReceiptapiService) {

        this.exGridConfig = {};
        this.gridConfig = {};
        this.authService.currentUser.subscribe(x => {
            this.currentUserModel = x;
        });


        this.receiptUrl = receiptapiService.receiptUrl;
        this.thermalReceiptUrl = receiptapiService.thermalReceiptUrl;


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

        this.loadExInitialiseGrid();
        //this.loadCommonInitialiseGrid();

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


                    this.loadCommonInitialiseGrid();
                    //for load grid by paidby,methodName
                    this.billCollectionCommonService.GetDataForCommonGrid(this.currentUserModel.user.username, this.billCollectionCommonModel.MethodName, '')
                        .pipe(first())
                        .subscribe(
                            data2 => {
                                this.gridConfig.dataSource = data2;
                            },
                            error => {
                                console.log(error);
                            }
                        );



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
                        if (data.CALLWITHAMT == "N") {
                            this.isAmountDisabled = true;
                        }
                        else {
                            this.isAmountDisabled = false;
                        }

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

    loadExInitialiseGrid(): any {
        this.exGridConfig.dataSource = [];

        this.exGridConfig.autoUpdateDataSource = true;
        this.exGridConfig.autoIndexing = true;
        this.exGridConfig.isBatchSwitchBoxEdit = true;

        //this.exGridConfig.gridName = "From Agent";
        //this.exGridConfig.gridIconClass = 'fas fa-thumbtack';

        //this.exGridConfig.hasEditState = true;
        this.exGridConfig.showUniversalFilter = false;


        this.exGridConfig.columnList = [
            //{ field: 'id', header: 'ID', width: '30%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'paymenT_HEAD', header: 'Payment head', width: '30%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'fee', header: 'Fee', width: '20%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'latE_FEE', header: 'Late fee', width: '15%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'total', header: 'Total', width: '20%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'makeStatus', header: 'Action', width: '10%', isSwitchBoxColumn: true, filter: this.gridSettingService.getFilterableNoneAndToggleSelectAll() }
        ];
    }

    loadCommonInitialiseGrid(): any {
        this.gridConfig.dataSource = [];

        this.gridConfig.autoUpdateDataSource = true;
        this.gridConfig.autoIndexing = true;
        this.gridConfig.isBatchSwitchBoxEdit = true;

        //this.exGridConfig.gridName = "From Agent";
        //this.exGridConfig.gridIconClass = 'fas fa-thumbtack';

        //this.exGridConfig.hasEditState = true;
        this.gridConfig.showUniversalFilter = false;

        if (this.featurePayModel.SUBMENUTITLE) {
            this.gridConfig.columnList = [
                { field: 'trans_No', header: 'Trans No', width: '10%' },
                { field: 'trans_Date', header: 'Trans Date', width: '15%' },
                { field: 'msg_Amt', header: 'Amount', width: '10%' },
                { field: 'billno', header: this.featurePayModel.BILLTITLE, width: '15%' },
                { field: 'ref_Phone', header: 'Beneficiary Mobile Number', width: '20%' },
                { field: 'subname', header: this.featurePayModel.SUBMENUTITLE, width: '17%' },
                { field: 'Id', header: 'General Receipt', width: '13%', isCustomAction: true, customActionIcon: 'fas fa-file-download' },
                { field: 'Id', header: 'Thermal Receipt', width: '13%', isCustomActionTwo: true, customActionIcon: 'fas fa-file-download' }
            ];
        }
        else {
            this.gridConfig.columnList = [
                { field: 'trans_No', header: 'Trans No', width: '15%' },
                { field: 'trans_Date', header: 'Trans Date', width: '15%' },
                { field: 'msg_Amt', header: 'Amount', width: '10%' },
                { field: 'billno', header: this.featurePayModel.BILLTITLE, width: '15%' },
                { field: 'ref_Phone', header: 'Beneficiary Mobile Number', width: '25%' },
                //{ field: 'subname', header: 'Sub Name', width: '15%' },
                { field: 'Id', header: 'General Receipt', width: '20%', isCustomAction: true, customActionIcon: 'fas fa-file-download' },
                { field: 'Id', header: 'Thermal Receipt', width: '20%', isCustomActionTwo: true, customActionIcon: 'fas fa-file-download' }
            ];
        }





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

        if (this.billCollectionCommonModel.beneficiaryNumber.substring(0, 2) != '01') {
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
        if (!this.isAmountDisabled) {
            if (!this.billCollectionCommonModel.amount || this.billCollectionCommonModel.amount == '' || this.billCollectionCommonModel.amount == '0') {
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

                            if (this.featurePayModel.MOREBILLTITLE != null && this.featurePayModel.CALLWITHMORE != 'Y') {
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

                            this.checkedBill2 = this.billCollectionCommonModel.bill2;



                            if (data.amount == "0") {
                                this.isAmountDisabled = false;
                                this.billCollectionCommonModel.amount = '';
                            }
                            else {
                                this.isAmountDisabled = true;
                            }

                            if (data.fees != null) {
                                this.isAmountDisabled = true;
                                this.isShowMMSGrid = true;
                                this.isShowEnterAmount = false;
                                this.exGridConfig.dataSource = data.fees;
                                this.FeeCollectionModel = data.fees;
                            } else {
                                this.isShowMMSGrid = false;
                                this.isShowEnterAmount = true;
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
        if (this.FeeCollectionModel.length > 0) {
            this.selectedFeeCollectionModel = this.FeeCollectionModel.filter(it => {
                return it.makeStatus == true;
            });
            if (this.selectedFeeCollectionModel.length > 0) {
                this.sum = 0;
                this.ids = "";
                this.billCollectionCommonModel.bill2 = this.checkedBill2;
                for (var i = 0; i < this.selectedFeeCollectionModel.length; i++) {
                    this.sum = this.sum + (+this.selectedFeeCollectionModel[i].total);
                    this.ids = this.ids + (this.ids != "" ? "#" : "") + this.selectedFeeCollectionModel[i].id;
                }
                this.billCollectionCommonModel.amount = this.sum;
                this.billCollectionCommonModel.bill2 = this.billCollectionCommonModel.bill2 + this.ids;

            }
        }



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
            if (this.featurePayModel.MOREBILLTITLE != null || this.ids != '') {
                this.billCollectionCommonModel.bill2 = this.billCollectionCommonModel.bill2 + this.glue + this.branchCode + this.glue + this.userName + this.glue;
            }
            else {
                this.billCollectionCommonModel.bill2 = this.billCollectionCommonModel.bill2 + this.branchCode + this.glue + this.userName + this.glue;
            }

            ///for MMS Payment 
            //if (this.ids != null) {
            //    this.billCollectionCommonModel.bill2 = this.billCollectionCommonModel.bill2 + this.glue + this.branchCode + this.glue + this.userName + this.glue;
            //}
            //else {
            //    this.billCollectionCommonModel.bill2 = this.billCollectionCommonModel.bill2 + this.branchCode + this.glue + this.userName + this.glue;
            //}

        }
        else {
            if (this.featurePayModel.MOREBILLTITLE != null || this.ids != '') {
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


            ///for MMS Payment 
            //if (this.ids != null) {
            //    this.billCollectionCommonModel.bill2 = this.billCollectionCommonModel.bill2 + ',' + this.branchCode + ',' + this.userName + ',';
            //}
            //else {
            //    if (this.billCollectionCommonModel.bill2) {
            //        this.billCollectionCommonModel.bill2 = this.billCollectionCommonModel.bill2 + this.branchCode + ',' + this.userName + ',';
            //    }
            //    else {
            //        this.billCollectionCommonModel.bill2 = this.branchCode + ',' + this.userName + ',';
            //    }

            //}

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

    onGenerate(event) {

        //window.open(this.receiptUrl + event.ref_Phone + "&Trans_ID=" + event.trans_No, "_blank");

        this.obj.mphone = event.ref_Phone;
        this.obj.Trans_ID = event.trans_No;

        const mapForm = document.createElement('form');
        mapForm.target = '_blank';
        mapForm.method = 'POST'; // or "post" if appropriate
        mapForm.action = this.receiptUrl;
        Object.keys(this.obj).forEach((param) => {
            const mapInput = document.createElement('input');
            mapInput.type = 'hidden';
            mapInput.name = param;
            mapInput.setAttribute('value', this.obj[param]);
            mapForm.appendChild(mapInput);
        });
        document.body.appendChild(mapForm);
        mapForm.submit();

    }

    onThermalReceipt(event) {

        //window.open(this.receiptUrl + event.ref_Phone + "&Trans_ID=" + event.trans_No, "_blank");

        this.obj.mphone = event.ref_Phone;
        this.obj.Trans_ID = event.trans_No;

        const mapForm = document.createElement('form');
        mapForm.target = '_blank';
        mapForm.method = 'POST'; // or "post" if appropriate
        mapForm.action = this.thermalReceiptUrl;
        Object.keys(this.obj).forEach((param) => {
            const mapInput = document.createElement('input');
            mapInput.type = 'hidden';
            mapInput.name = param;
            mapInput.setAttribute('value', this.obj[param]);
            mapForm.appendChild(mapInput);
        });
        document.body.appendChild(mapForm);
        mapForm.submit();

    }



}
