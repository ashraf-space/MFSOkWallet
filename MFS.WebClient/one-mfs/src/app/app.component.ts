import { Component, OnInit, ViewEncapsulation, HostListener } from '@angular/core';
import { Router } from '@angular/router';
import { trigger, style, animate, transition } from '@angular/animations';

import { AuthenticationService, AuditTrailService } from './shared/_services';
import { User } from './shared/_models';

import { MenuItem } from 'primeng/api';
import { MessageService } from 'primeng/api';
import { ApplicationUserService } from './services/security';
import { first } from 'rxjs/operators';
import { MfsUtilityService } from './services/mfs-utility.service';
import { GridSettingService } from './services/grid-setting.service';
import { UserIdleService } from 'angular-user-idle';
import { Subject } from 'rxjs';
@Component({
    selector: 'app', animations: [
        trigger(
            'menu-annimation', [
                transition(':enter', [
                    style({ transform: 'translateX(-120%)', opacity: 0.5 }),
                    animate('400ms ease-out', style({ transform: 'translateX(0)', opacity: 1 }))
                ]),
                transition(':leave', [
                    style({ transform: 'translateX(0)', opacity: 1 }),
                    animate('100ms ease-out', style({ transform: 'translateX(-60%)', opacity: 0.6 }))
                ])
            ]
        )
    ],
    templateUrl: 'app.component.html',
    encapsulation: ViewEncapsulation.None,
    providers: [MessageService]
})

export class AppComponent implements OnInit {

    currentUser: any;
    leftMenuItems: MenuItem[] = [];
    settingItems: MenuItem[];
    showMenu: boolean;
    menuObj: any = {};

    promptChangePasswordModal: boolean;
    changePasswordModel: any;
    invalidCredentials: boolean;
    globalSearchModal: boolean;

    firstTimeChange: boolean;
    display: boolean;

    searchOptions: any = [];
    criteriaList: any = [];
    searchModel: any;
    searchGridConfig: any;
    isGridLoading: boolean = false;
    href: string;
    url: string;
    menuName: any;
    categoryName: any;
    userName: any;
    auditTrailModel: any = {};
    userActivity;
    userInactive: Subject<any> = new Subject();
    constructor(
        private router: Router,
        private authenticationService: AuthenticationService,
        private applicationUserService: ApplicationUserService,
        private messageService: MessageService,
        private mfsUtilityService: MfsUtilityService,
        private gridSettingService: GridSettingService,
        private auditTrailService: AuditTrailService,
        private userIdle: UserIdleService
    ) {
        
        this.authenticationService.currentUser.subscribe(x => {
            this.currentUser = x;
            this.generateLeftMenu();
            if (this.currentUser && this.currentUser.user.pstatus == 'N') {
                this.firstTimeChange = true;
                this.promptChangePassword();
            }
        });
        this.searchModel = {};
    }

    ngOnInit() {
        this.changePasswordModel = {};
        this.searchGridConfig = {};
        this.setTimeout();
        this.userInactive.subscribe((res) => {            
            //console.log('Reset');
        });
        this.userIdle.startWatching();
        this.showMenu = true;
        this.userIdle.onTimerStart().subscribe();

        //Start watch when time is up.
        this.userIdle.onTimeout().subscribe((res) => {
            if (res) {
                this.logout();
            }
        });
        this.settingItems = [
            {
                label: 'Settings',
                icon: 'far fa-life-ring',
                items: [
                    {
                        label: ' Password', icon: 'fas fa-key', command: (event) => {
                            this.firstTimeChange = false;
                            this.promptChangePassword();
                        }
                    },
                    {
                        label: ' Search', icon: 'fas fa-search', command: (event) => {
                            this.firstTimeChange = false;
                            this.initiateGlobalSearch();
                        }
                    },
                    {
                        label: ' Sign Out ', icon: 'fas fa-sign-out-alt', command: (event) => {
                            this.logout();
                        }
                    },
                ]
            }
        ];
    }
    setTimeout() {
        this.userActivity = setTimeout(() => this.userInactive.next(undefined), 300000);
        this.userIdle.resetTimer();
    }

    @HostListener('window:mousemove') refreshUserState() {
        //console.log('refreshUserState');
        clearTimeout(this.userActivity);
        this.setTimeout();
    }
    logout() {
        this.leftMenuItems = [];
        this.authenticationService.logout();
        this.router.navigate(['/login']);
    }

    generateLeftMenu(): any {
        this.leftMenuItems = [
            {
                label: ' Dashboard',
                icon: 'pi pi-home',
                routerLink: ['/'],
                command: (event) => {
                    this.display = false;
                    //this.insertIntoAuditTrail(event);
               }
            }
        ];

        var menuCategory: any = {};
        if (this.currentUser && this.currentUser.featureList.length != 0) {

            this.currentUser.featureList.forEach(obj => {
                menuCategory = this.findMenuCategory(obj.CATEGORYNAME);
                if (menuCategory != null) {
                    this.menuObj = {
                        label: ' ' + obj.FEATURENAME, icon: obj.FEATUREICON, routerLink: [obj.FEATURELINK],
                        command: (event) => {
                            this.display = false;
                            this.insertIntoAuditTrail(event);
                        }
                    };
                    menuCategory.items.push(this.menuObj);
                    this.menuObj = {};
                }
                else {
                    this.menuObj = {
                        label: ' ' + obj.CATEGORYNAME, icon: obj.CATEGORYICON,
                        items: [{
                            label: ' ' + obj.FEATURENAME, icon: obj.FEATUREICON, routerLink: [obj.FEATURELINK],
                            command: (event) => {
                                this.display = false;
                                this.insertIntoAuditTrail(event);
                            }
                        }]
                    };
                    this.leftMenuItems.push(this.menuObj);
                    this.menuObj = {};
                }
            });
        }
    }

    insertIntoAuditTrail(event) {

        this.auditTrailModel.Who = this.currentUser.user.username;
        this.auditTrailModel.WhatAction = 'VISIT';
        this.auditTrailModel.WhatActionId = this.auditTrailService.getWhatActionId('VISIT')
        this.auditTrailModel.WhichMenu = event.item.label.trim();
        this.auditTrailModel.WhichParentMenu = this.currentUser.featureList.find(it => {
            return it.FEATURENAME.includes(this.auditTrailModel.WhichMenu);
        }).CATEGORYNAME;
        this.auditTrailModel.WhichParentMenuId = this.auditTrailService.getWhichParentMenuId(this.auditTrailModel.WhichParentMenu);
        sessionStorage.setItem('currentEvent', JSON.stringify(event));

        this.auditTrailService.insertIntoAuditTrail(this.auditTrailModel).pipe(first())
            .subscribe(
                data => {
                    if (data) {

                    }

                },
                error => {

                });

    }

    findMenuCategory(menuLabel) {
        var ret = this.leftMenuItems.find(function (obj) {
            return obj.label.trim() == menuLabel;
        });
        return ret;
    }

    onProfileVisit() {
        this.router.navigateByUrl('/application-user/add-edit/' + this.currentUser.user.id);
    }

    promptChangePassword(): any {
        this.promptChangePasswordModal = true;
    }

    confirmPasswordChange() {
        this.changePasswordModel.ApplicationUserId = this.currentUser.user.id;
        this.applicationUserService.changePassword(this.changePasswordModel).pipe(first())
            .subscribe(
                data => {
                    if (data == 'Old Password is Invalid') {
                        this.invalidCredentials = true;
                    }
                    else {
                        this.messageService.add({ severity: 'success', summary: 'Success', detail: this.currentUser.user.name + ' password changed Successfully' });
                        this.promptChangePasswordModal = false;
                        this.logout();
                    }
                },
                error => {
                    this.messageService.add({ severity: 'error', summary: 'Warning', detail: this.currentUser.user.name + ' password change unsuccessful' });
                });
    }

    initiateGlobalSearch() {
        this.searchModel = {};
        this.searchModel.optionList = this.mfsUtilityService.getSearchModelOptionParams();
        this.globalSearchModal = true;
    }

    fillCriteriaList() {
        this.searchModel.criteria = null;
        this.searchModel.filter = null;
        this.searchModel.criteriaList = this.mfsUtilityService.getSearchModelCriteriaParams(this.searchModel.option);
    }

    onSearch() {
        this.searchModel.initiateSearch = false;
        this.isGridLoading = true;
        this.initializeSearchGrid();
        this.searchModel.initiateSearch = true;
        this.isGridLoading = true;
    }

    async initializeSearchGrid() {
        this.searchGridConfig = {};
        this.searchGridConfig.dataSource = [];
        this.searchGridConfig.gridName = ' Search Result';
        this.searchGridConfig.gridIconClass = 'fas fa-search';
        this.searchGridConfig.showCaption = false;

        this.searchGridConfig.columnList = [];

        this.authenticationService.getGlobalSearchResult(this.searchModel).pipe()
            .subscribe(data => {
                this.searchGridConfig.dataSource = data;
                console.log(data.length);
                if (data.length > 0) {
                    this.generateGlobalSearchGridColumnList(this.searchGridConfig.dataSource);
                }
                else {
                    this.searchModel.initiateSearch = false;
                    this.messageService.add({ severity: 'error', summary: 'Warning', detail: ' No Data Found' });
                }

                this.isGridLoading = false;
                //this.searchModel = {};
            });
    }

    generateGlobalSearchGridColumnList(data: any): any {
        var columnObj: any = {};

        Object.keys(data[0]).forEach(e => {

            columnObj = {};
            columnObj.width = '25%';
            columnObj.filter = this.gridSettingService.getDefaultFilterable();

            if (e.toLowerCase() == 'details') {
                this.searchGridConfig.detailsStateUrl = 'customer/details/';
                columnObj.field = 'MPHONE';
                columnObj.header = e.toLowerCase();
                columnObj.isDetailsColumn = true;
            }
            else {
                columnObj.field = e;
                columnObj.header = e.toLowerCase().replace("_", " ");
            }

            if (e.toLowerCase().includes('date'))
                columnObj.template = this.gridSettingService.getDateTemplateForRowData();

            this.searchGridConfig.columnList.push(columnObj);
        });
    }

}