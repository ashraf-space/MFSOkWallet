import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { first } from 'rxjs/operators';
import { Router, ActivatedRoute } from '@angular/router';
import { User } from '../../shared/_models';
import { UserService, AuthenticationService } from '../../shared/_services';

@Component({ templateUrl: 'home.component.html', styleUrls: ['home.component.css'] })


export class HomeComponent implements OnInit, OnDestroy {
    currentUser: User;
    currentUserSubscription: Subscription;
    users: User[] = [];
    pieData: any;
    barData: any;
    barData2: any;

    dashboardModel: any;
    isLoading: boolean;
    clientCountList: any;

    constructor(
        private authenticationService: AuthenticationService,
        private userService: UserService,
        private route: ActivatedRoute,
        private router: Router
    ) {
        this.currentUserSubscription = this.authenticationService.currentUser.subscribe(user => {
            this.currentUser = user;   
            this.dashboardModel = {};
            this.clientCountList = [];
            this.clientCountList.push({ name: 'CLIENT', icon: 'far fa-hand-point-right' });
        });
    }

    ngOnInit() {
        this.getDataForDashboard();
        this.pieData = {
            labels: ['Agent','Customer','Distributor','DSR','Merchant'],
            datasets: [
                {
                    data: [],
                    backgroundColor: [
                        "#FF6384",
                        "#36A2EB",
                        "#FFCE56",
                        "#01447a",
                        "#940a0a"
                    ],
                    hoverBackgroundColor: [
                        "#FF6384",
                        "#36A2EB",
                        "#FFCE56",
                        "#01447a",
                        "#940a0a"
                    ]
                }]
        };

        this.barData = {
            labels: [],
            datasets: [
                {
                    label: 'Transactions (lakh)',
                    backgroundColor: '#42A5F5',
                    borderColor: '#1E88E5',
                    data: []
                }
            ]
        };

        this.barData2 = {
            labels: [],
            datasets: [
                {
                    label: 'Transactions (lakh)',
                    backgroundColor: '#42A5F5',
                    borderColor: '#78A0CA',
                    data: [],
                    fill: false
                }
            ]
        };
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

                    Object.keys(this.dashboardModel.totalClientCount).forEach(e => {
                        if (e != 'CLIENTTHISYEAR' && e !='CLIENTTHISMONTH')
                            this.clientCountList.push(this.insertIntoClientCountList(e));
                    });

                    this.dashboardModel.totalTransaction = this.dashboardModel.totalTransaction / 100;
                },
                error => {
                    console.log(error);
                });
    }
}