import { Component, OnInit, ViewEncapsulation,Output,EventEmitter } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { error } from 'selenium-webdriver';
import { first } from 'rxjs/operators';
import { MessageService, MenuItem } from 'primeng/api';
import { AuthenticationService } from 'src/app/shared/_services';
import { CustomerService } from '../../../../services/distribution/customer.service';
import { MfsUtilityService } from '../../../../services/mfs-utility.service';
import { Message } from 'primeng/components/common/api';
import { KycService } from '../../../../services/distribution/kyc.service';
import { Reginfo } from '../../../../shared/_models/reginfo';
@Component({
    selector: 'app-kyc-cbs',
    templateUrl: './kyc-cbs.component.html',
    styleUrls: ['./kyc-cbs.component.css'],
    encapsulation: ViewEncapsulation.None
})
export class KycCbsComponent implements OnInit {
    public reginfo: Reginfo;

    @Output() onCbsDataUpdate = new EventEmitter<any>();

    constructor(private router: Router,
        private route: ActivatedRoute,
        private messageService: MessageService,
        private authService: AuthenticationService,          
        private customerService: CustomerService,
        private mfsUtilityService: MfsUtilityService,
        private kycService: KycService) {
        this.reginfo = new Reginfo();     
        this.authService.currentUser.subscribe(x => {
            this.reginfo.currentUserModel = x;
        });        
    }
    ngOnInit() {
        var a = 10;
    }
    getCbsAccInfo() {
        this.customerService.GetCbsAccInfo(this.reginfo.regInfoModel.mphone, this.reginfo.regInfoModel.bankAcNo)
            .pipe(first())
            .subscribe(
                data => {
                    //this.reginfo.regInfoModel.distCode = data;
                    this.onCbsDataUpdate.emit(data);
                },
                error => {
                    console.log(error);
                });
    }
}
