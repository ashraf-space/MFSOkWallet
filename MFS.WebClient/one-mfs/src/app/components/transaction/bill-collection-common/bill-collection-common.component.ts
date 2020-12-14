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
    isLoading: boolean = false;
    mySubscription: any;
    monthYearList: any;
    isShowMonth: boolean = false;
    isShowCardHolder: boolean = false;
    subMenuList: any;
    isShowSubMenuDDL: boolean = false;

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
        
        this.featureId = +this.route.snapshot.paramMap.get('id');
        this.billCollectionCommonModel = {};
        this.billCollectionCommonService.GetFeaturePayDetails(this.featureId)
            .pipe(first())
            .subscribe(
                data => {
                    this.featurePayModel = data;
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
        if (!this.billCollectionCommonModel.billId || this.billCollectionCommonModel.billId == '' || this.billCollectionCommonModel.billId == '0') {
            this.msgs = [];
            this.msgs.push({ severity: 'error', summary: 'Warning! ', detail: 'Cannot be left blank' });
            this.error = true;
        }
        else if (!this.billCollectionCommonModel.beneficiaryNumber || this.billCollectionCommonModel.beneficiaryNumber == '' || this.billCollectionCommonModel.beneficiaryNumber == '0') {
            this.msgs = [];
            this.msgs.push({ severity: 'error', summary: 'Warning! ', detail: 'Cannot be left blank' });
            this.error = true;
        }

        else if (this.featurePayModel.MONTHTITLE != null) {
            if (!this.billCollectionCommonModel.month || this.billCollectionCommonModel.month == '' || this.billCollectionCommonModel.month == '0') {
                this.msgs = [];
                this.msgs.push({ severity: 'error', summary: 'Warning! ', detail: 'Cannot be left blank' });
                this.error = true;
            }
        }        

        else if (this.featurePayModel.MOREBILLTITLE != null) {
            if (!this.billCollectionCommonModel.cardHolderName || this.billCollectionCommonModel.cardHolderName == '' || this.billCollectionCommonModel.cardHolderName == '0') {
                this.msgs = [];
                this.msgs.push({ severity: 'error', summary: 'Warning! ', detail: 'Cannot be left blank' });
                this.error = true;
            }
        }
       

        else if (this.featurePayModel.SUBMENUTITLE != null) {
            if (!this.billCollectionCommonModel.subMenuId || this.billCollectionCommonModel.subMenuId == '' || this.billCollectionCommonModel.subMenuId == '0') {
                this.msgs = [];
                this.msgs.push({ severity: 'error', summary: 'Warning! ', detail: 'Cannot be left blank' });
                this.error = true;
            }
        }
        else {
            this.isLoading = true;
            this.billCollectionCommonService.CheckBillInfo(this.billCollectionCommonModel).pipe(first())
                .subscribe(
                    data => {

                        //if (this.isRegistrationPermitted) {
                        //    if (data == "1") {
                        //        this.messageService.add({ severity: 'success', summary: 'Approved successfully', detail: 'Branch cash in approved' });
                        //    }
                        //    else {
                        //        this.messageService.add({ severity: 'error', summary: 'Not Approved', detail: data });
                        //    }
                        //}
                        setTimeout(() => {
                            this.isLoading = false;
                            location.reload();
                        }, 5000);
                        
                    },
                    error => {
                        console.log(error);
                    });

            
        }
    }



}
