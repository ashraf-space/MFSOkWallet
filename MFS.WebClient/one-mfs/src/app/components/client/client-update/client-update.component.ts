import { Component, OnInit, ViewChild, Input, Output } from '@angular/core';
import { KycService } from '../../../services/distribution/kyc.service';
import { first } from 'rxjs/operators';
import { AuthenticationService } from 'src/app/shared/_services';
import { MessageService, MenuItem } from 'primeng/api';
import { MfsUtilityService } from '../../../services/mfs-utility.service';

@Component({
    selector: 'mfs-client-update',
    templateUrl: './client-update.component.html',
    styleUrls: ['./client-update.component.css']
})
export class ClientUpdateComponent implements OnInit {

    @Input() model: any;
    occupationList: any;
    currentUserModel: any = {};
    positveNumber: RegExp;
    constructor(private kycService: KycService,
        private authService: AuthenticationService,
        private mfsUtilityService: MfsUtilityService,
        private messageService: MessageService) {
        this.authService.currentUser.subscribe(x => {
            this.currentUserModel = x;
        });
        this.positveNumber = this.mfsUtilityService.getPositiveWholeNumberRegExp();
    }

    ngOnInit() {
        //if (this.model.catId === 'C') {
        //    this.getOccupationList();
        //}    
        this.getOccupationList();
    }

    async getOccupationList() {
        this.kycService.GetOccupationList()
            .pipe(first())
            .subscribe(
                data => {
                    this.occupationList = data;
                },
                error => {
                    console.log(error);
                }
            );
    }
    onSave() {
        this.model.updateBy = this.currentUserModel.user.username;
        this.kycService.updetKyc(this.model).pipe(first())
            .subscribe(
                data => {
                    if (data) {
                        window.history.back();
                        this.messageService.add({ severity: 'success', summary: 'Update successfully', detail: 'Client Updated' });
                    }
                },
                error => {
                    console.log(error);
                    this.messageService.add({ severity: 'error', summary: 'Erros Occured', detail: error, closable: false });
                });
    }
}
