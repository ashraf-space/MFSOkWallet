
import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthenticationService } from 'src/app/shared/_services';
import { InterestTaxVatRateSetupService } from '../../../services/environment/interest-tax-vat-rate-setup.service';
import { first } from 'rxjs/operators';

@Component({
    selector: 'app-interest-tax-vat-rate-setup',
    templateUrl: './interest-tax-vat-rate-setup.component.html',
    styleUrls: ['./interest-tax-vat-rate-setup.component.css']
})
export class InterestTaxVatRateSetupComponent implements OnInit {

    rateModel: any = {};
    currentUserModel: any = {};
    calculationMethodList: any;
    statusList : any;
    constructor(private authenticationService: AuthenticationService,
        private interestTaxVatRateSetupService: InterestTaxVatRateSetupService,
        private router: Router,
        private route: ActivatedRoute) {
        this.authenticationService.currentUser.subscribe(x => {
            this.currentUserModel = x;
        });
    }

    ngOnInit() {
        this.calculationMethodList = [
            { label: 'Actual/Actual', value: '1' },
            { label: 'Actual/360', value: '2' }        
        ];

        this.statusList = [
            { label: 'Active', value: 'A' },
            { label: 'Inactive', value: 'I' }
        ];
        this.getGlobalInfos();
    }

    getGlobalInfos(): any {
        this.interestTaxVatRateSetupService.getGlobalInfos()
            .pipe(first())
            .subscribe(
                data => {
                    this.rateModel.interestPer = data[0].INTEREST_PER;
                    this.rateModel.serviceStatus = data[0].SERVICE_STATUS;
                    this.rateModel.taxPer = data[0].TAX_PER;
                    this.rateModel.vatPer = data[0].VAT_PER;
                    this.rateModel.tinTaxPer = data[0].TIN_TAX_PER;

                },
                error => {
                    console.log(error);
                });

    }
    onSave() {
        
    }

}
