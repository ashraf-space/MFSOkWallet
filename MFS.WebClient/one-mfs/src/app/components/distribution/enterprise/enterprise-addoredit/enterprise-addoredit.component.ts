
import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { DistributorService, DsrService } from 'src/app/services/distribution';
import { Router, ActivatedRoute } from '@angular/router';
import { error } from 'selenium-webdriver';
import { first } from 'rxjs/operators';
import { MessageService, MenuItem } from 'primeng/api';
import { AuthenticationService } from 'src/app/shared/_services';
import { AgentService } from '../../../../services/distribution/agent.service';
import { CustomerService } from '../../../../services/distribution/customer.service';
import { MfsUtilityService } from '../../../../services/mfs-utility.service';
import { Message } from 'primeng/components/common/api';
import { NgbDatepickerConfig, NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { KycService } from '../../../../services/distribution/kyc.service';
import { EnterpriseServiceService } from '../../../../services/distribution/enterprise-service.service';
@Component({
    selector: 'app-enterprise-addoredit',
    templateUrl: './enterprise-addoredit.component.html',
    styleUrls: ['./enterprise-addoredit.component.css'],
    encapsulation: ViewEncapsulation.None
})
export class EnterpriseAddoreditComponent implements OnInit {

    regInfoModel: any = {};
    activeIndex: number = 0;
    items: MenuItem[];
    genderTypes: any;
    religeonList: any;
    relationList: any;
    selectedRegion: string = "0";
    selectedArea: string = "0";
    divisionList: any;
    selectedDivision: string = "0";
    districtList: any;
    selectedDistrict: string = "0";
    thanaList: any;
    photoIDTypeList: any;
    bankBranchList: any;
    selectedTerritory: string = "0";
    selectedCluster: string = "0";
    clusterList: any;
    distCode: string;
    currentUserModel: any = {};
    entityId: string;
    isEditMode: boolean = false;
    DistributorCode: string = "";
    regDate: any = {};
    dateOfBirth: any = {};
    formValidation: any;
    isFinalValdidate: boolean;

    isNidVerified: boolean;
    isLoading: boolean = false;
    isRegPermit = false;
    msgs: Message[] = [];
    error: boolean = false;
    showDuplicateMsg: boolean = false;
    customeNumberInput: any;
    date = new Date();
    occupationList: any;
    constructor(private distributionService: DistributorService,
        private router: Router,
        private route: ActivatedRoute,
        private messageService: MessageService,
        private authService: AuthenticationService,
        private agentService: AgentService,
        private dsrService: DsrService,
        private customerService: CustomerService,
        private mfsUtilityService: MfsUtilityService,
        private ngbDatepickerConfig: NgbDatepickerConfig,
        private kycService: KycService,
        private enterpriseServiceService: EnterpriseServiceService) {
        this.customeNumberInput = this.mfsUtilityService.getPositiveWholeNumberRegExp();
        ngbDatepickerConfig.minDate = { year: 1919, month: 1, day: 1 };
        var currentDate = new Date();
        ngbDatepickerConfig.maxDate = { year: currentDate.getFullYear(), month: currentDate.getMonth() + 1, day: currentDate.getDate() };
        this.authService.currentUser.subscribe(x => {
            this.currentUserModel = x;
        });
        this.formValidation = {};
    }

    ngOnInit() {
        this.items = [
            {
                label: 'Office use only',
                command: (event: any) => {
                    this.activeIndex = 0;
                    //this.messageService.add({ severity: 'success', summary: 'Service Message', detail: 'Via MessageService' });
                }
            },
            {
                label: 'Personal details',
                command: (event: any) => {
                    this.activeIndex = 1;
                }
            },
            {
                label: 'Bank info',
                command: (event: any) => {
                    this.activeIndex = 2;
                    //this.messageService.add({ severity: 'info', summary: 'Pay with CC', detail: event.item.label });
                }
            }
        ];

        this.getDivisionListForDDL();
        this.getBankBranchListForDDL();

        this.entityId = this.route.snapshot.paramMap.get('id');
        if (this.entityId) {
            this.isEditMode = true;
            this.isNidVerified = false;
            this.GetCustomerByMphone();
            this.isRegPermit = this.authService.checkRegisterPermissionAccess(this.route.snapshot.routeConfig.path);
        }
        if (!this.entityId) {
            var currentDate = new Date();
            this.isNidVerified = true;
            this.regDate = { year: currentDate.getFullYear(), month: currentDate.getMonth() + 1, day: currentDate.getDate() };
        }
        this.regInfoModel.partOfFirst = 100;
    }
    checkMphoneAlreadyExist(): any {              
        if (this.regInfoModel.mphone) {
            this.distributionService.GetDistributorByMphone(this.regInfoModel.mphone)
                .pipe(first())
                .subscribe(
                    data => {
                        if (data != null) {
                            this.msgs = [];
                            this.msgs.push({ severity: 'error', summary: 'Enterprise A/C No : ' + this.regInfoModel.mphone, detail: 'Already Exists!' });
                            this.regInfoModel.mphone = null;
                            this.showDuplicateMsg = true;
                        }
                        else {
                            this.showDuplicateMsg = false;
                        }
                    },
                    error => {
                        console.log(error);
                    }
                );
        }        
    }
    GetCustomerByMphone(): any {
        this.isLoading = true;
        this.customerService.getCustomerByMphone(this.entityId)
            .pipe(first())
            .subscribe(
                data => {
                    if (data.locationCode) {
                        this.regInfoModel = data;
                        this.selectedDivision = this.regInfoModel.locationCode.substring(0, 2);
                        this.fillDistrictDDLByDivision();
                        this.selectedDistrict = this.regInfoModel.locationCode.substring(0, 4);
                        this.fillThanaDDLByDistrict();
                        //this.GetDistributorCodeByPhotoId(data.photoId);
                        this.regDate = this.mfsUtilityService.renderDateObject(data.regDate);
                        if (data.dateOfBirth != null) {
                            this.dateOfBirth = this.mfsUtilityService.renderDateObject(data.dateOfBirth);
                        }
                    }

                    this.isLoading = false;
                },
                error => {
                    this.isLoading = false;
                    console.log(error);
                });
    }
    async fillThanaDDLByDistrict() {
        this.distributionService.getDistrictListByDivision(this.selectedDistrict)
            .pipe(first())
            .subscribe(
                data => {
                    this.thanaList = data;
                },
                error => {
                    console.log(error);
                }
            );
    }
    async fillDistrictDDLByDivision() {
        this.distributionService.getDistrictListByDivision(this.selectedDivision)
            .pipe(first())
            .subscribe(
                data => {
                    this.districtList = data;
                },
                error => {
                    console.log(error);
                }
            );
    }
    getBankBranchListForDDL(): any {
        this.distributionService.getBankBranchListForDDL()
            .pipe(first())
            .subscribe(
                data => {
                    this.bankBranchList = data;
                },
                error => {
                    console.log(error);
                }
            );
    }
    async getDivisionListForDDL() {
        this.distributionService.getDivisionList()
            .pipe(first())
            .subscribe(
                data => {
                    this.divisionList = data;
                },
                error => {
                    console.log(error);
                }
            );
    }
    onStepAhead(event) {
        if (this.activeIndex < 3) {
            this.validation(event);
        }
    }

    onStepBack() {
        if (this.activeIndex > 0) {
            this.activeIndex--;
        }
    }
    validation(event) {

        switch (this.activeIndex) {
            case 0:
                {
                    if (!this.regInfoModel.mphone) {
                        this.msgs = [];
                        this.msgs.push({ severity: 'error', summary: 'Warning! ', detail: 'Cannot be left blank' });
                        this.error = true;
                        break;
                    }
                    else {
                        this.activeIndex++;
                        this.error = false;
                        break;
                    }
                }
            case 1:
                {
                    if (!this.regInfoModel.name ||
                        !this.selectedDivision ||
                        !this.selectedDistrict ||
                        !this.regInfoModel.mphone) {
                        this.msgs = [];
                        this.msgs.push({ severity: 'error', summary: 'Warning! ', detail: 'Cannot be left blank' });
                        this.error = true;
                        break;
                    } else {
                        this.activeIndex++;
                        this.error = false;
                        break;
                    }
                }

            case 2:
                {
                    if (!this.regInfoModel.branchCode ||
                        !this.regInfoModel.name ||
                        !this.selectedDivision ||
                        !this.selectedDistrict ||
                        !this.regInfoModel.mphone
                    ) {
                        this.msgs = [];
                        this.msgs.push({ severity: 'error', summary: 'Warning! ', detail: 'Cannot be left blank' });
                        if (this.regInfoModel.branchCode) {
                            this.messageService.add({ severity: 'error', summary: 'Cannot be left blank', detail: 'Mandatory input Cannot be left blank', closable: true });
                        }
                        this.error = true;
                        break;
                    } else {
                        this.SaveEnterprise(event);
                        this.error = false;
                        break;
                    }
                }

        }
    }

    SaveEnterprise(event): any {
       
        if (this.isEditMode) {
            this.regInfoModel.updateBy = this.currentUserModel.user.username;
        }
        else {
            this.regInfoModel.entryBy = this.currentUserModel.user.username; 
        }
        //if (event != 'reject') {
        //    this.regInfoModel.entryBy = this.currentUserModel.user.username;
        //}
        if (this.regInfoModel.mphone != "" || this.regInfoModel.branchName != "") {
            this.enterpriseServiceService.save(this.regInfoModel, this.isEditMode, event).pipe(first())
                .subscribe(
                    data => {
                        window.history.back();
                        if (data === 200) {
                            if (event == 'reject') {
                                this.messageService.add({ severity: 'error', summary: 'Reject successfully', detail: 'Customer approval rejected' });
                            }
                            else if (this.isEditMode && !this.isRegPermit) {
                                this.messageService.add({ severity: 'success', summary: 'Update successfully', detail: 'Customer Updated' });
                            }
                            else if
                                (this.isRegPermit && this.isRegPermit) {
                                this.messageService.add({ severity: 'success', summary: 'Register successfully', detail: 'Customer Registered' });
                            }
                            else
                                this.messageService.add({ severity: 'success', summary: 'Save successfully', detail: 'Customer added' });

                        }
                        else {
                            this.messageService.add({ severity: 'error', summary: 'Erros Occured', detail: 'Server Error', closable: false });
                        }
                    },
                    error => {
                        console.log(error);
                        this.messageService.add({ severity: 'error', summary: 'Erros Occured', detail: error, closable: false });
                    });
        }
    }
}
