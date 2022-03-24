import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { Observable, interval, Subscription } from 'rxjs';
import { first } from 'rxjs/operators';
import { Router, ActivatedRoute } from '@angular/router';
import { User } from '../../../../shared/_models';
import { UserService, AuthenticationService } from '../../../../shared/_services';
import { MessageService, Message } from 'primeng/api';

@Component({
    selector: 'app-utility-dash',
    templateUrl: './utility-dash.component.html',
    styleUrls: ['./utility-dash.component.css']
})
export class UtilityDashComponent implements OnInit {

    currentUser: User;
    currentUserModel: any = {};
    currentUserSubscription: Subscription;
    users: User[] = [];
    barUtilityDataToday: any;
    barUtilityDataMonth: any;
    barUtilityDataAll: any;
    dashboardModel: any;
    isLoading: boolean;


    constructor(
        private authenticationService: AuthenticationService,
        private messageService: MessageService) {
        this.dashboardModel = {};
    }

    ngOnInit() {
        
        this.barUtilityDataToday = {
            labels: [],
            datasets: [
                {
                    label: 'Utility Transaction Today',
                    backgroundColor: '#42A5F5',
                    borderColor: '#1E88E5',
                    fill:false,
                    data: []
                }
            ]
        };
        this.barUtilityDataMonth = {
            labels: [],
            datasets: [
                {
                    label: 'Utility Transaction Month',
                    backgroundColor: '#42A5F5',
                    borderColor: '#1E88E5',
                    fill: false,
                    data: []
                }
            ]
        };
        this.barUtilityDataAll = {
            labels: [],
            datasets: [
                {
                    label: 'Utility Transaction Lifetime',
                    backgroundColor: '#42A5F5',
                    borderColor: '#1E88E5',
                    fill: false,
                    data: []
                }
            ]
        };
        this.GetDataForUtilityDashboard();
    }
    selectData(event) {
        this.messageService.add({ severity: 'info', summary: 'Data Selected', 'detail': this.dashboardModel.barChartForTotal[event.element._index].amount });
    }
    refreshDashboard() {

        this.ngOnInit();
        this.GetDataForUtilityDashboard();
    }
    GetDataForUtilityDashboard() {
        this.isLoading = true;
        this.authenticationService.GetDataForUtilityDashboard().pipe(first())
            .subscribe(
                data => {
                    if (data) {                      
                        this.isLoading = false;
                        this.dashboardModel = data;                       
                        this.dashboardModel.barChartForCurrent.forEach(obj => {
                            this.barUtilityDataToday.labels.push(obj.utility.trim());
                            this.barUtilityDataToday.datasets[0].data.push(obj.amount);
                        });
                        this.dashboardModel.barChartForCurrentMonth.forEach(obj => {
                            this.barUtilityDataMonth.labels.push(obj.utility.trim());
                            this.barUtilityDataMonth.datasets[0].data.push(obj.amount);
                        });
                        this.dashboardModel.barChartForTotal.forEach(obj => {
                            this.barUtilityDataAll.labels.push(obj.utility.trim());
                            this.barUtilityDataAll.datasets[0].data.push(obj.amount);
                        });

                       
                    }
                    

                },
                error => {
                    this.isLoading = false;
                    console.log(error);
                });

    }

}
