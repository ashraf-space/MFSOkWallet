import { Component, OnInit, OnDestroy } from '@angular/core';
import { MessageService } from 'primeng/api';
import { AuthenticationService } from 'src/app/shared/_services';
import { ActivatedRoute, Router, NavigationEnd } from '@angular/router';
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

    constructor(private messageService: MessageService, private billCollectionCommonService: BillCollectionCommonService
        , private authService: AuthenticationService
        , private route: ActivatedRoute, private router: Router) {      

        this.authService.currentUser.subscribe(x => {
            this.currentUserModel = x;
        });


        //this.router.routeReuseStrategy.shouldReuseRoute = function () {
        //    return false;
        //};

        //this.mySubscription = this.router.events.subscribe((event) => {
        //    if (event instanceof NavigationEnd) {
        //        // Trick the Router into believing it's last link wasn't previously loaded
        //        this.router.navigated = false;
        //    }
        //});

    }



    ngOnInit() {
       
        var eventLog = JSON.parse(sessionStorage.getItem('currentEvent'));
        this.featureId = eventLog.item.featureId;

        //this.router.navigateByUrl('/', { skipLocationChange: true }).then(() => {
        //    this.router.navigate(['BillCollectionCommonComponent']);
        //});


        //this.featurePayModel = this.billCollectionCommonService.GetFeaturePayDetails(this.featureId);
        this.billCollectionCommonService.GetFeaturePayDetails(this.featureId)
            .pipe(first())
            .subscribe(
                data => {
                    this.featurePayModel = data;

                },
                error => {
                    console.log(error);
                }
            );
    }

    //ngOnDestroy() {
    //    if (this.mySubscription) {
    //        this.mySubscription.unsubscribe();
    //    }
    //}


}
