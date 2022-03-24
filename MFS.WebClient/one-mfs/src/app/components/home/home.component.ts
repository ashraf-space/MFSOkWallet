import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { Observable, interval, Subscription } from 'rxjs';
import { first } from 'rxjs/operators';
import { Router, ActivatedRoute } from '@angular/router';
import { User } from '../../shared/_models';
import { UserService, AuthenticationService } from '../../shared/_services';


@Component({ templateUrl: 'home.component.html', styleUrls: ['home.component.css'] })


export class HomeComponent implements OnInit, OnDestroy {
    @ViewChild('mfsPdfViewer') pdfViewer;
    @ViewChild('form') childForm;
    pdf: any;
    reportObject: any;

    currentUser: User;
    currentUserModel: any = {};
    currentUserSubscription: Subscription;
    users: User[] = [];
    pieData: any;
    barData: any;
    barData2: any;
    barTransData: any;
    barAgentData: any;
    barCustomerData: any;
    barDistributorData: any;
    barOnlineMerchantData: any;
    barOfflineMerchantData: any;

    dashboardModel: any;
    isLoading: boolean;
    clientCountList: any;

    isBranchTeller: boolean = false;
    userPStatus: any;
    utilityBillCollectionMenus: any;
    tuitionFeeCollectionMenus: any;
    creditCardBillCollectionMenus: any;
    OtherBillFeeCollectionMenus: any;
    cashInOutMenus: any;
    interval: any;
    

    constructor(
        private authenticationService: AuthenticationService,
        private userService: UserService,
        private route: ActivatedRoute,
        private router: Router

    ) {
        this.currentUserSubscription = this.authenticationService.currentUser.subscribe(user => {
            this.currentUser = user;
            this.currentUserModel = user;
            this.dashboardModel = {};
            this.clientCountList = [];
            this.clientCountList.push({ name: 'CLIENT', icon: 'far fa-hand-point-right' });
        });

        this.pdf = {};
        this.reportObject = {};
        //this.model = {};
        this.reportObject.fileType = 'PDF';
        //this.fileOptionList = reportUtilityService.getFileExtensionList();
    }

    ngOnInit() {

        this.refreshData();
        this.interval = setInterval(() => {
            this.refreshData();
        }, 300000);
       

    }

    refreshData() {
        this.userPStatus = this.currentUserModel.user.pstatus;
        this.isBranchTeller = this.currentUserModel.user.role_Name == 'Branch Teller' ? true : false;

        //this.getDataForDashboard();
        //this.pieData = {
        //    labels: ['Agent', 'Customer', 'Distributor', 'DSR', 'Merchant'],
        //    datasets: [
        //        {
        //            data: [],
        //            backgroundColor: [
        //                "#FF6384",
        //                "#36A2EB",
        //                "#FFCE56",
        //                "#01447a",
        //                "#940a0a"
        //            ],
        //            hoverBackgroundColor: [
        //                "#FF6384",
        //                "#36A2EB",
        //                "#FFCE56",
        //                "#01447a",
        //                "#940a0a"
        //            ]
        //        }]
        //};

        //this.barData = {
        //    labels: [],
        //    datasets: [
        //        {
        //            label: 'Transactions (lakh)',
        //            backgroundColor: '#42A5F5',
        //            borderColor: '#1E88E5',
        //            data: []
        //        }
        //    ]
        //};

        //this.barData2 = {
        //    labels: [],
        //    datasets: [
        //        {
        //            label: 'Transactions (lakh)',
        //            backgroundColor: '#42A5F5',
        //            borderColor: '#78A0CA',
        //            data: [],
        //            fill: false
        //        }
        //    ]
        //};

        //this.barTransData = {
        //    labels: [],
        //    datasets: [
        //        {
        //            label: 'Transaction Trend(Volume in BDT in Million)',
        //            backgroundColor: '#42A5F5',
        //            borderColor: '#1E88E5',
        //            data: [],
        //            fill: false
        //        }
        //    ]
        //};

        //this.barAgentData = {
        //    labels: [],
        //    datasets: [
        //        {
        //            label: 'OK Wallet Agent',
        //            backgroundColor: '#42A5F5',
        //            borderColor: '#1E88E5',
        //            data: [],
        //            fill: false
        //        }
        //    ]
        //};
        //this.barCustomerData = {
        //    labels: [],
        //    datasets: [
        //        {
        //            label: 'OK Wallet Customer',
        //            backgroundColor: '#42A5F5',
        //            borderColor: '#1E88E5',
        //            data: [],
        //            fill: false
        //        }
        //    ]
        //};
        //this.barDistributorData = {
        //    labels: [],
        //    datasets: [
        //        {
        //            label: 'OK Wallet Distributor',
        //            backgroundColor: '#78A0CA',
        //            borderColor: '#1E88E5',
        //            data: [],
        //            fill: false
        //        }
        //    ]
        //};
        //this.barOnlineMerchantData = {
        //    labels: [],
        //    datasets: [
        //        {
        //            label: 'OK Wallet Merchant(Online)',
        //            backgroundColor: '#78A0CA',
        //            borderColor: '#1E88E5',
        //            data: [],
        //            fill: false
        //        }
        //    ]
        //};
        //this.barOfflineMerchantData = {
        //    labels: [],
        //    datasets: [
        //        {
        //            label: 'OK Wallet Merchant(Offline)',
        //            backgroundColor: '#78A0CA',
        //            borderColor: '#1E88E5',
        //            data: [],
        //            fill: false
        //        }
        //    ]
        //};


        this.authenticationService.GetBillCollectionMenus(this.currentUserModel.user.id)
            .pipe(first())
            .subscribe(
                data => {
                    if (data) {
                        this.utilityBillCollectionMenus = data.filter(x => x.CATEGORYID == 31);
                        this.tuitionFeeCollectionMenus = data.filter(x => x.CATEGORYID == 32);
                        this.creditCardBillCollectionMenus = data.filter(x => x.CATEGORYID == 33);
                        this.OtherBillFeeCollectionMenus = data.filter(x => x.CATEGORYID == 34);
                        this.cashInOutMenus = data.filter(x => x.CATEGORYID == 81);
                    }


                },
                error => {
                    console.log(error);
                }
            );      

    }



    ngOnDestroy() {
        this.currentUserSubscription.unsubscribe();
    }
    

   

    insertIntoClientCountList(e) {
        var obj: any = {};

        obj.name = e;
        obj.count = this.dashboardModel.totalClientCount[e];
        obj.thisMonth = this.dashboardModel.clientCountByMonth[e];
        obj.thisYear = this.dashboardModel.clientCountByYear[e];
        obj.pending = this.dashboardModel.pendingClientCount[e];

        switch (e) {
            case 'DISTRIBUTOR':
                obj.icon = 'fas fa-building';
                break;
            case 'AGENT':
                obj.icon = 'fas fa-street-view';
                break;
            case 'MERCHANT':
                obj.icon = 'fas fa-store';
                break;
            case 'DSR':
                obj.icon = 'fas fa-truck';
                break;
            case 'CUSTOMER':
                obj.icon = 'fas fa-user';
                break;
            case 'TOTAL':
                obj.icon = 'fas fa-users';
                obj.thisYear = this.dashboardModel.totalClientCount.CLIENTTHISYEAR;
                obj.thisMonth = this.dashboardModel.totalClientCount.CLIENTTHISMONTH;
                break;
            default:
                break;
        }

        return obj;
    }

    getDataForDashboard() {
        if (!this.isBranchTeller) {
            this.isLoading = true;
            this.authenticationService.getDataForDashboard().pipe(first())
                .subscribe(
                    data => {
                        this.dashboardModel = data;
                        //console.log(this.dashboardModel);
                        this.isLoading = false;
                        this.dashboardModel.totalTransaction = 0;
                        this.pieData.datasets[0].data.push(this.dashboardModel.totalClientCount.AGENT);
                        this.pieData.datasets[0].data.push(this.dashboardModel.totalClientCount.CUSTOMER);
                        this.pieData.datasets[0].data.push(this.dashboardModel.totalClientCount.DISTRIBUTOR);
                        this.pieData.datasets[0].data.push(this.dashboardModel.totalClientCount.DSR);
                        this.pieData.datasets[0].data.push(this.dashboardModel.totalClientCount.MERCHANT);

                        this.dashboardModel.transactionByMonth.forEach(obj => {
                            this.barData.labels.push(obj.MONTH.trim());
                            this.barData.datasets[0].data.push(obj.TRANSACTION);
                        });

                        this.dashboardModel.transactionByYear.forEach(obj => {
                            if (obj.YEAR != null) {
                                this.barData2.labels.push(obj.YEAR.trim());
                                this.barData2.datasets[0].data.push(obj.TRANSACTION);
                                this.dashboardModel.totalTransaction = this.dashboardModel.totalTransaction + obj.TRANSACTION;
                            }
                        });
                        
                        this.dashboardModel.listTransactionTrend.forEach(obj => {
                            this.barTransData.labels.push(obj.CAPTION.trim());
                            this.barTransData.datasets[0].data.push(obj.TRANSACTIONAMT);
                        });

                        this.dashboardModel.dynamicClientCount.forEach(obj => {
                            this.barAgentData.labels.push(obj.CAPTION.trim());
                            this.barAgentData.datasets[0].data.push(obj.AGENT);

                            this.barCustomerData.labels.push(obj.CAPTION.trim());
                            this.barCustomerData.datasets[0].data.push(obj.CUSTOMER);

                            this.barDistributorData.labels.push(obj.CAPTION.trim());
                            this.barDistributorData.datasets[0].data.push(obj.DISTRIBUTOR);

                            this.barOnlineMerchantData.labels.push(obj.CAPTION.trim());
                            this.barOnlineMerchantData.datasets[0].data.push(obj.MERCHANTONLINE);

                            this.barOfflineMerchantData.labels.push(obj.CAPTION.trim());
                            this.barOfflineMerchantData.datasets[0].data.push(obj.MERCHANTOFFLINE);

                        });
                        //this.dashboardModel.dynamicClientCount.forEach(obj => {
                        //    this.barCustomerData.labels.push(obj.CAPTION.trim());
                        //    this.barCustomerData.datasets[0].data.push(obj.AGENT);
                        //});
                        //this.dashboardModel.dynamicClientCount.forEach(obj => {
                        //    this.barDistributorData.labels.push(obj.CAPTION.trim());
                        //    this.barDistributorData.datasets[0].data.push(obj.DISTRIBUTOR);
                        //});

                        Object.keys(this.dashboardModel.totalClientCount).forEach(e => {
                            if (e != 'CLIENTTHISYEAR' && e != 'CLIENTTHISMONTH')
                                this.clientCountList.push(this.insertIntoClientCountList(e));
                        });

                        this.dashboardModel.totalTransaction = this.dashboardModel.totalTransaction / 100;
                    },
                    error => {
                        console.log(error);
                    });
        }

    }

    goToBranchCashIn() {
        this.router.navigate(['/transfer/branch-cash-in']);
        //this.router.navigateByUrl('../feature-category/worklist');
    }



    goToBranchCashOut() {
        this.router.navigate(['/transfer/branch-cash-out']);
    }

    goToReport() {
        this.router.navigate(['/reports/branch-cashin-cashout']);
    }

    //filterItemsOfCategory(categoryId) {
    //    return this.billCollectionMenus.filter(x => x.CATEGORYID == categoryId);
    //}

    goToBillCollectionMenu(url) {
        this.router.navigate(['/' + url]);
    }

    transactionAnalysis() {
        this.isLoading = true;
        this.authenticationService.getTransactionAnalysis(this.dashboardModel.totalClientCount).pipe(first())
            .subscribe(
                data => {
                    if (data) {
                        this.pdf.source = data;
                        //this.pdf.ext = this.reportObject.fileType;
                        this.pdf.ext = "PDF";
                        //this.pdf.fileName = this.model.ReportName;
                        this.pdf.fileName = "Transaction Analysis";
                        this.isLoading = false;
                        this.pdfViewer.refreshReport();
                    }
                    else {
                        this.isLoading = false;
                    }
                    
                },
                error => {
                    console.log(error);
                });
    }
}