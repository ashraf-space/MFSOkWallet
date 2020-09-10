
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
@Component({
    selector: 'app-customer-addoredit',
    templateUrl: './customer-addoredit.component.html',
    styleUrls: ['./customer-addoredit.component.css'],
    encapsulation: ViewEncapsulation.None
})
export class CustomerAddoreditComponent implements OnInit {
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
    alphabetsWithSpace: any;
    emailRegx: any;
    showRejectModal: boolean = false;
    disableButton: boolean = false;
    checkedAsPresent: boolean = false;
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
        private kycService: KycService) {
        this.customeNumberInput = this.mfsUtilityService.getPositiveWholeNumberRegExp();
        this.alphabetsWithSpace = this.mfsUtilityService.getAlphabetsWithSpaceEegExp();
        this.emailRegx = this.mfsUtilityService.getEmailRegExp();
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
                label: 'Nominee info',
                command: (event: any) => {
                    this.activeIndex = 2;
                    //this.messageService.add({ severity: 'info', summary: 'Pay with CC', detail: event.item.label });
                }
            },
            {
                label: 'Introducer info',
                command: (event: any) => {
                    this.activeIndex = 3;
                    //this.messageService.add({ severity: 'info', summary: 'Pay with CC', detail: event.item.label });
                }
            }
            //},
            //{
            //    label: 'Bank info',
            //    command: (event: any) => {
            //        this.activeIndex = 4;
            //        //this.messageService.add({ severity: 'info', summary: 'Last Step', detail: event.item.label });
            //    }
            //}
        ];

        this.genderTypes = [
            { label: 'Male', value: 'M', icon: 'fas fa-male' },
            { label: 'Female', value: 'F', icon: 'fas fa-female' },
            { label: 'Others', value: 'O', icon: 'fas fa-transgender-alt' }
        ];

        this.religeonList = [
            { label: 'Islam', value: 'Islam' },
            { label: 'Hinduism', value: 'Hinduism' },
            { label: 'Chritianity', value: 'Chritianity' },
            { label: 'Buddhism', value: 'Buddhism' },
            { label: 'Other', value: 'Other' }
        ];

        this.relationList = [
            { label: 'Husband', value: 'Husband' },
            { label: 'Wife', value: 'Wife' },
            { label: 'Mother', value: 'Mother' },
            { label: 'Father', value: 'Father' },
            { label: 'Son', value: 'Son' },
            { label: 'Daughter', value: 'Daughter' },
            { label: 'Brother', value: 'Brother' },
            { label: 'Sister', value: 'Sister' },
            { label: 'Others', value: 'Others' }
        ];

        this.getDivisionListForDDL();
        this.getBankBranchListForDDL();
        this.getOccupationList();
        this.getPhotoIDTypeListForDDL();
        this.regInfoModel.nationality = 'Bangladeshi';
        this.entityId = this.route.snapshot.paramMap.get('id');
        if (this.entityId) {
            this.isEditMode = true;
            this.GetCustomerByMphone();
            this.isRegPermit = this.authService.checkRegisterPermissionAccess(this.route.snapshot.routeConfig.path);
            if (this.isRegPermit) {
                this.isNidVerified = false;
            }
            else {
                this.isNidVerified = true;
            }
        }
        if (!this.entityId) {
            var currentDate = new Date();
            this.isNidVerified = true;
            this.regDate = { year: currentDate.getFullYear(), month: currentDate.getMonth() + 1, day: currentDate.getDate() };
        }
        this.regInfoModel.partOfFirst = 100;
    }
    validateDatepicker(event) {
        if (this.dateOfBirth) {
            var validate = this.mfsUtilityService.validateDatePickerInput(this.dateOfBirth);

            if (!validate) {
                //this.messageService.add({ severity: 'error', summary: 'Invalid Date Formate', detail: 'Please Input Valid Date', closable: true });
                this.dateOfBirth = null;
            }
        }
    }
    checkMphoneAlreadyExist(): any {
        //var mobileNo: string = this.regInfoModel.mphone;
        //var operatorCode: string = mobileNo.substring(0, 2);
        //var toMatch: string = "01";        
        if (this.regInfoModel.mphone.toString().substring(0, 2) == "01" && this.regInfoModel.mphone.toString().substring(0, 3) != "012") {
            this.distributionService.GetDistributorByMphone(this.regInfoModel.mphone)
                .pipe(first())
                .subscribe(
                    data => {
                        if (data != null) {
                            this.msgs = [];
                            this.msgs.push({ severity: 'error', summary: 'Customer A/C No : ' + this.regInfoModel.mphone, detail: 'Already Exists!' });
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
        else {
            this.regInfoModel.mphone = '';
            this.messageService.add({ severity: 'error', summary: 'Invalid Mobile No', detail: 'Please Input Valid Mobiel No', closable: true });
        }
    }
    GetCustomerByMphone(): any {
        this.isLoading = true;
        this.customerService.getCustomerByMphone(this.entityId)
            .pipe(first())
            .subscribe(
                data => {
                    if (data) {
                        this.regInfoModel = data;
                        if (data.locationCode) {
                            this.selectedDivision = this.regInfoModel.locationCode.substring(0, 2);
                            this.fillDistrictDDLByDivision();
                            this.selectedDistrict = this.regInfoModel.locationCode.substring(0, 4);
                            this.fillThanaDDLByDistrict();
                        }

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
    GetDistributorCodeByPhotoId(value: string) {
        this.distributionService.GetDistributorCodeByPhotoId(value)
            .pipe(first())
            .subscribe(
                data => {
                    this.DistributorCode = data;
                },
                error => {
                    console.log(error);
                });
    }
    GenerateAgentCode() {
        this.agentService.GenerateAgentCode(this.selectedCluster)
            .pipe(first())
            .subscribe(
                data => {
                    this.regInfoModel.distCode = data.DIST_CODE;
                },
                error => {
                    console.log(error);
                });
    }


    GetclusterByTerritoryCode() {
        this.agentService.GetclusterByTerritoryCode(this.selectedTerritory)
            .pipe(first())
            .subscribe(
                data => {
                    this.clusterList = data;
                },
                error => {
                    console.log(error);
                }
            );
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

    fillClusterDDL() {
        this.agentService.fillClusterDDL()
            .pipe(first())
            .subscribe(
                data => {
                    this.clusterList = data;
                },
                error => {
                    console.log(error);
                });
    }
    getPhotoIDTypeListForDDL(): any {
        this.distributionService.getPhotoIDTypeListForDDL()
            .pipe(first())
            .subscribe(
                data => {
                    this.photoIDTypeList = data;
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

    onStepAhead(event) {
        if (this.activeIndex < 4) {
            this.validation(event);
            //this.activeIndex++;
        }
        else {
            //this.SaveCustomer(event);
        }
    }

    onStepBack() {
        if (this.activeIndex > 0) {
            this.activeIndex--;
        }
    }
    validation(event) {
        if (event == 'reject') {
            this.SaveCustomer(event);
            this.error = false;
        }
        else {
            switch (this.activeIndex) {
                case 0:
                    {
                        if (!this.regInfoModel.mphone || !this.regDate.year) {
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
                            !this.regInfoModel.photoIdTypeCode ||
                            !this.regInfoModel.photoId ||
                            !this.selectedDivision ||
                            !this.selectedDistrict ||
                            !this.regInfoModel.locationCode ||
                            !this.regInfoModel.occupation ||
                            !this.regInfoModel.perAddr ||
                            !this.regInfoModel.mphone ||
                            !this.regDate.year ||
                            !this.regInfoModel.fatherName ||
                            !this.regInfoModel.motherName ||
                            !this.dateOfBirth.year ||
                            !this.regInfoModel.gender) {
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
                        if (!this.regInfoModel.name ||
                            !this.regInfoModel.photoIdTypeCode ||
                            !this.regInfoModel.photoId ||
                            !this.selectedDivision ||
                            !this.selectedDistrict ||
                            !this.regInfoModel.locationCode ||
                            !this.regInfoModel.occupation ||
                            !this.regInfoModel.perAddr ||
                            !this.regInfoModel.mphone ||
                            !this.regDate.year ||
                            !this.dateOfBirth.year ||
                            !this.regInfoModel.fatherName ||
                            !this.regInfoModel.motherName ||
                            !this.regInfoModel.firstNomineeName ||
                            !this.regInfoModel.gender) {
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
                case 3:
                    {
                        //this.activeIndex++;
                        //this.error = false;
                        //break;
                        this.SaveCustomer(event);
                        this.error = false;
                        break;
                    }
                //case 4:
                //    {
                //        if (!this.regInfoModel.branchCode ||
                //            !this.regInfoModel.name ||
                //            !this.regInfoModel.photoIdTypeCode ||
                //            !this.regInfoModel.photoId ||
                //            !this.selectedDivision ||
                //            !this.selectedDistrict ||
                //            !this.regInfoModel.locationCode ||
                //            !this.regInfoModel.occupation ||
                //            !this.regInfoModel.perAddr ||
                //            !this.regInfoModel.mphone ||
                //            !this.regDate.year ||
                //            !this.regInfoModel.fatherName ||
                //            !this.regInfoModel.motherName ||
                //            !this.dateOfBirth.year) {
                //            this.msgs = [];
                //            this.msgs.push({ severity: 'error', summary: 'Warning! ', detail: 'Cannot be left blank' });
                //            if (this.regInfoModel.branchCode) {
                //                this.messageService.add({ severity: 'error', summary: 'Cannot be left blank', detail: 'Mandatory input Cannot be left blank', closable: true });
                //            }
                //            this.error = true;
                //            break;
                //        } else {
                //            this.SaveCustomer(event);
                //            this.error = false;
                //            break;
                //        }
                //    }

            }
        }

    }

    onRejectCustomer(event) {
        this.disableButton = true;
        if (event === 'reject') {
            this.customerService.save(this.regInfoModel, this.isEditMode, event).pipe(first())
                .subscribe(
                    data => {
                        if (data === 200) {
                            this.showRejectModal = false;
                            this.disableButton = false;
                            window.history.back();
                            if (event == 'reject') {
                                this.messageService.add({ severity: 'error', summary: 'Reject successfully', sticky: true, detail: 'Customer rejected: ' + this.regInfoModel.mphone });
                            }
                        }
                        else {
                            this.messageService.add({ severity: 'error', summary: 'Erros in: ' + this.regInfoModel.mphone, sticky: true, detail: 'Bad Response from BackEnd', closable: true });
                        }
                    },
                    error => {
                        console.log(error);
                        this.messageService.add({ severity: 'error', summary: 'Erros Occured', sticky: true, detail: error, closable: false });
                    });
        }
        this.showRejectModal = false;
        this.disableButton = false;
    }

    sameAsPresent() {
        if (this.checkedAsPresent) {
            this.regInfoModel.preAddr = this.regInfoModel.perAddr;
        }
        else {
            this.regInfoModel.preAddr = '';
        }

    }
    SaveCustomer(event): any {
        this.regInfoModel.regDate = this.mfsUtilityService.renderDate(this.regDate);
        this.regInfoModel.dateOfBirth = this.mfsUtilityService.renderDate(this.dateOfBirth);
        if (this.isNidVerified) {
            this.regInfoModel.photoidValidation = 'Y';
        }
        else {
            this.regInfoModel.photoidValidation = 'N';
        }

        if (event === 'edit') {
            this.regInfoModel.updateBy = this.currentUserModel.user.username;
            this.regInfoModel.branchCode = this.currentUserModel.user.branchCode;
        }
        if (event === 'register') {
            this.regInfoModel.authoBy = this.currentUserModel.user.username;
        }
        if (event === 'save') {
            this.regInfoModel.entryBy = this.currentUserModel.user.username;
            this.regInfoModel.branchCode = this.currentUserModel.user.branchCode;
        }
        if (event === 'reject') {
            this.showRejectModal = true;
        }
        if (this.regInfoModel.mphone != "" && event != 'reject') {
            this.customerService.save(this.regInfoModel, this.isEditMode, event).pipe(first())
                .subscribe(
                    data => {
                        if (data === 200) {
                            window.history.back();
                            if (event == 'reject') {
                                this.messageService.add({ severity: 'error', summary: 'Reject successfully', sticky: true, detail: 'Customer rejected: ' + this.regInfoModel.mphone });
                            }

                            else if (this.isEditMode && !this.isRegPermit) {
                                this.messageService.add({ severity: 'success', summary: 'Update successfully', sticky: true, detail: 'Customer Updated: ' + this.regInfoModel.mphone });
                            }
                            else if (this.isRegPermit && this.isRegPermit) {
                                this.messageService.add({ severity: 'success', summary: 'Register successfully', sticky: true, detail: 'Customer Registered: ' + this.regInfoModel.mphone });
                            }
                            else {
                                this.messageService.add({ severity: 'success', summary: 'Save successfully', sticky: true, detail: 'Customer added: ' + this.regInfoModel.mphone });
                            }
                        }
                        else {
                            this.messageService.add({ severity: 'error', summary: 'Erros in: ' + this.regInfoModel.mphone, sticky: true, detail: 'Bad Response from BackEnd', closable: true });
                        }
                    },
                    error => {
                        console.log(error);
                        this.messageService.add({ severity: 'error', summary: 'Erros Occured', sticky: true, detail: error, closable: false });
                    });
        }
    }


    cancel() {
        window.history.back();
    }

    checkPhotoIdLength() {
        this.formValidation.photoId = this.mfsUtilityService.checkPhotoIdLength(this.regInfoModel.photoId, this.regInfoModel.photoIdTypeCode);
        if (!this.formValidation.photoId) {
            this.checkNidValid(this.regInfoModel.photoId);
        }
    };

    checkNidValid(value: any): any {
        this.kycService.checkNidValid(value, 'C')
            .pipe(first())
            .subscribe(
                data => {
                    if (data === false) {
                        this.messageService.add({ severity: 'error', summary: 'Duplicate ID', detail: 'Photo Id Already Exists!', closable: true });
                        this.regInfoModel.photoId = '';
                    }
                },
                error => {
                    console.log(error);
                });
    }
}
