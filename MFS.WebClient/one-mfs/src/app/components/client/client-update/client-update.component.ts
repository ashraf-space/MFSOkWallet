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
    regInfoModel: any = {};
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
    ngOnChanges() {
        if (this.model.mphone) {
            this.regInfoModel.mphone = this.model.mphone;
            this.regInfoModel.name = this.model.name;
            this.regInfoModel.spouseName = this.model.spouseName;
            this.regInfoModel.fatherName = this.model.fatherName;
            this.regInfoModel.motherName = this.model.motherName;
            this.regInfoModel.monthlyIncome = this.model.monthlyIncome;
            this.regInfoModel.occupation = this.model.occupation;
            this.regInfoModel.conMob = this.model.conMob;
            this.regInfoModel.conPhone = this.model.conPhone;
            this.regInfoModel.secondConName = this.model.secondConName;
            this.regInfoModel.secondConMob = this.model.secondConMob;
            this.regInfoModel.thirdConName = this.model.thirdConName;
            this.regInfoModel.thirdConMob = this.model.thirdConMob;
            this.regInfoModel.offAddr = this.model.offAddr;
            this.regInfoModel.preAddr = this.model.preAddr;
            this.regInfoModel.perAddr = this.model.perAddr;
        }      
    }
    ngOnInit() {
        
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
        this.regInfoModel.updateBy = this.currentUserModel.user.username;
        this.kycService.updetKyc(this.regInfoModel).pipe(first())
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
