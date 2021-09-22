import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { DistributorService } from 'src/app/services/distribution';
import { Router, ActivatedRoute } from '@angular/router';
import { MessageService, MenuItem, Message } from 'primeng/api';
import { first } from 'rxjs/operators';
import { error } from 'selenium-webdriver';
import { AuthenticationService } from 'src/app/shared/_services';
import { MfsUtilityService } from 'src/app/services/mfs-utility.service';
import { KycService } from '../../../../services/distribution/kyc.service';
import { NgbDatepickerConfig, NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { MerchantService } from '../../../../services/distribution/merchant.service';

@Component({
    selector: 'app-distributor-addoredit',
    templateUrl: './distributor-addoredit.component.html',
    styleUrls: ['./distributor-addoredit.component.css'],
    encapsulation: ViewEncapsulation.None
})
export class DistributorAddoreditComponent implements OnInit {
    isEditPermitted: boolean = false;
    isRegistrationPermitted: boolean = false;
    regInfoModel: any = {};
    activeIndex: number = 0;
    items: MenuItem[];
    genderTypes: any;
    religeonList: any;
    regionList: any;
    areaList: any;
    territoryList: any;
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
    distCode: string;
    currentUserModel: any = {};
    entityId: string;
    isEditMode: boolean = false;
    isLoading: boolean = false;
    sessionPermission: any = {};
    formValidation: any;

    msgs: Message[] = [];
    regDate: any = {};
    dateOfBirth: any = {};
    error: boolean = false;
    showDuplicateMsg: boolean = false;
    checkedAsPresent: boolean = false;
    merchantBankBranchList: any;
    bankDistrictList: any;
    bankBranchByDistBankCodeList: any;
    isSecuredViewPermitted: boolean = false;
    disabledEdit: boolean = true;
    positveNumber: RegExp;
    constructor(private distributionService: DistributorService,
        private router: Router,
        private route: ActivatedRoute,
        private messageService: MessageService,
        private authService: AuthenticationService,
        private mfsUtilityService: MfsUtilityService,
        private ngbDatepickerConfig: NgbDatepickerConfig,
        private kycService: KycService,
        private merchantService: MerchantService) {
        ngbDatepickerConfig.minDate = { year: 1919, month: 1, day: 1 };
        var currentDate = new Date();
        ngbDatepickerConfig.maxDate = { year: currentDate.getFullYear(), month: currentDate.getMonth() + 1, day: currentDate.getDate() };
        this.authService.currentUser.subscribe(x => {
            this.currentUserModel = x;
        });
        this.formValidation = {};
        this.positveNumber = this.mfsUtilityService.getPositiveWholeNumberRegExp();
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
                label: 'Bank info',
                command: (event: any) => {
                    this.activeIndex = 3;
                    //this.messageService.add({ severity: 'info', summary: 'Last Step', detail: event.item.label });
                }
            }
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
            { label: 'Buddhism', value: 'Buddhism' }
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

        this.getRegionListForDDL();
        this.getDivisionListForDDL();
        this.getBankBranchListForDDL();
        this.getMerchantBankBranchList();
        this.getPhotoIDTypeListForDDL();
        this.regInfoModel.nationality = 'Bangladeshi';
        this.entityId = this.route.snapshot.paramMap.get('id');
        if (this.entityId) {
            this.isEditMode = true;
            this.getDistributorByMphone();

            this.isRegistrationPermitted = this.authService.checkRegisterPermissionAccess(this.route.snapshot.routeConfig.path);

            //this.isEditPermitted = this.authService.checkEditPermissionAccess(this.route.snapshot.routeConfig.path);
        }
        if (!this.entityId) {
            var currentDate = new Date();
            this.regDate = { year: currentDate.getFullYear(), month: currentDate.getMonth() + 1, day: currentDate.getDate() };
        }
        this.regInfoModel.partOfFirst = 100;
        this.checkForSecureView();
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
    sameAsPresent() {
        if (this.checkedAsPresent) {
            this.regInfoModel.preAddr = this.regInfoModel.perAddr;
        }
        else {
            this.regInfoModel.preAddr = '';
        }

    }
    getDistributorByMphone(): any {
        this.isLoading = true;
        this.distributionService.GetDistributorByMphone(this.entityId)
            .pipe(first())
            .subscribe(
                data => {
                    if (data.distCode) {
                        this.regInfoModel = data;
                        this.selectedRegion = this.regInfoModel.distCode.substring(0, 2);
                        this.fillAreaDDL();
                        this.selectedArea = this.regInfoModel.distCode.substring(0, 4);
                        this.fillTerritoryDDL();
                        this.selectedTerritory = this.regInfoModel.distCode.substring(0, 6);
                        this.selectedDivision = this.regInfoModel.locationCode.substring(0, 2);
                        this.fillDistrictDDLByDivision();
                        this.selectedDistrict = this.regInfoModel.locationCode.substring(0, 4);
                        this.fillThanaDDLByDistrict();
                        this.getDistrictByBank();
                        this.getBankBranchListByBankCodeAndDistCode();
                        this.regDate = this.mfsUtilityService.renderDateObject(data.regDate);
                        if (data.dateOfBirth != null) {
                            this.dateOfBirth = this.mfsUtilityService.renderDateObject(data.dateOfBirth);
                        }
                        this.isLoading = false;
                    }
                    else {
                        this.isLoading = false;
                    }

                },
                error => {
                    this.isLoading = false;
                    console.log(error);
                }
            )
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
    async getRegionListForDDL() {
        this.distributionService.getRegionList()
            .pipe(first())
            .subscribe(
                data => {
                    this.regionList = data;
                    this.regionList.unshift({ label: 'Please Select', value: null });
                },
                error => {
                    console.log(error);
                }
            );
    }

    //onStepBack() {
    //    this.showDuplicateMsg = false;
    //    if (this.activeIndex > 0) {
    //        this.activeIndex--;
    //    }
    //}

    //onStepAhead(event) {
    //    this.showDuplicateMsg = false;
    //    if (this.activeIndex < 3) {
    //        //this.activeIndex++;
    //        this.validation(event);
    //    }
    //    else {

    //        this.saveDistributor(event);
    //    }
    //}
    onStepAhead(event) {
        if (this.activeIndex < 4) {
            this.validation(event);
        }
        else {
            //this.SaveAgent(event);
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
                    if (!this.regInfoModel.mphone ||
                        !this.regDate.year ||
                        !this.selectedRegion || this.selectedRegion == '0' ||
                        !this.selectedArea || this.selectedArea == '0' ||
                        !this.selectedTerritory || this.selectedTerritory == '0') {
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
                    if (!this.regInfoModel.companyName ||
                        !this.regInfoModel.name ||
                        !this.regInfoModel.photoIdTypeCode ||
                        !this.regInfoModel.photoId ||
                        !this.regInfoModel.conMob ||
                        !this.regInfoModel.offAddr ||
                        !this.regInfoModel.tradeLicenseNo ||
                        !this.selectedDivision ||
                        !this.selectedDistrict ||
                        !this.regInfoModel.locationCode ||
                        !this.regInfoModel.perAddr ||
                        !this.regInfoModel.mphone ||
                        !this.regDate.year ||
                        !this.dateOfBirth.year ||
                        !this.selectedRegion || this.selectedRegion == '0' ||
                        !this.selectedArea || this.selectedArea == '0' ||
                        !this.selectedTerritory || this.selectedTerritory == '0') {
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
                    if (!this.regInfoModel.companyName ||
                        !this.regInfoModel.name ||
                        !this.regInfoModel.photoIdTypeCode ||
                        !this.regInfoModel.photoId ||
                        !this.regInfoModel.conMob ||
                        !this.regInfoModel.offAddr ||
                        !this.regInfoModel.tradeLicenseNo ||
                        !this.selectedDivision ||
                        !this.selectedDistrict ||
                        !this.regInfoModel.locationCode ||
                        !this.regInfoModel.perAddr ||
                        !this.regInfoModel.mphone ||
                        !this.regDate.year ||
                        !this.dateOfBirth.year ||
                        !this.selectedRegion || this.selectedRegion == '0' ||
                        !this.selectedArea || this.selectedArea == '0' ||
                        !this.selectedTerritory || this.selectedTerritory == '0') {
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
                    if (!this.regInfoModel.branchCode || this.regInfoModel.branchCode == "0" ||
                        !this.regInfoModel.companyName ||
                        !this.regInfoModel.name ||
                        !this.regInfoModel.photoIdTypeCode ||
                        !this.regInfoModel.photoId ||
                        !this.regInfoModel.conMob ||
                        !this.regInfoModel.offAddr ||
                        !this.selectedDivision ||
                        !this.selectedDistrict ||
                        !this.regInfoModel.locationCode ||
                        !this.regInfoModel.perAddr ||
                        !this.regInfoModel.mphone ||
                        !this.regDate.year ||
                        !this.dateOfBirth.year ||
                        !this.selectedRegion || this.selectedRegion == '0' ||
                        !this.selectedArea || this.selectedArea == '0' ||
                        !this.selectedTerritory || this.selectedTerritory == '0') {
                        this.msgs = [];
                        this.msgs.push({ severity: 'error', summary: 'Warning! ', detail: 'Cannot be left blank' });
                        this.error = true;
                        break;
                    } else {
                        this.saveDistributor(event);
                        this.error = false;
                        break;
                    }
                }

        }
    }

    saveDistributor(event): any {
        this.regInfoModel.regDate = this.mfsUtilityService.renderDate(this.regDate);
        this.regInfoModel.dateOfBirth = this.mfsUtilityService.renderDate(this.dateOfBirth);

        //if (!this.isEditMode) {
        //    this.regInfoModel.entryBy = this.currentUserModel.user.username;
        //}
        if (event === 'save') {
            this.regInfoModel.entryBy = this.currentUserModel.user.username;
        }
        if (this.isEditMode && !this.isRegistrationPermitted) {
            this.regInfoModel.updateBy = this.currentUserModel.user.username;
        }
        if (this.isEditMode && this.isRegistrationPermitted) {
            this.regInfoModel.authoBy = this.currentUserModel.user.username;
        }
        if (this.regInfoModel.distCode != "" || this.regInfoModel.branchName != "") {
            this.distributionService.save(this.regInfoModel, this.isEditMode, event).pipe(first())
                .subscribe(
                    data => {
                        if (data === 200) {
                            window.history.back();
                            if (this.isEditMode && !this.isRegistrationPermitted)
                                this.messageService.add({ severity: 'success', summary: 'Update successfully', sticky: true, detail: 'Distributor updated: ' + this.regInfoModel.mphone });
                            else if (this.isEditMode && this.isRegistrationPermitted)
                                this.messageService.add({ severity: 'success', summary: 'Register successfully', sticky: true, detail: 'Distributor registerd: ' + this.regInfoModel.mphone });
                            else {
                                this.messageService.add({ severity: 'success', summary: 'Save successfully', sticky: true, detail: 'Distributor added: ' + this.regInfoModel.mphone });
                            }
                        }
                        else {
                            this.messageService.add({ severity: 'error', summary: 'Erros in: ' + this.regInfoModel.mphone, sticky: true, detail: 'Bad Response from BackEnd', closable: true });
                        }
                    },
                    error => {
                        console.log(error);
                    });

        }
    }

    cancel() {
        window.history.back();
    }

    //load area against region
    fillAreaDDL() {
        this.distributionService.getAreaListByRegion(this.selectedRegion)
            .pipe(first())
            .subscribe(
                data => {
                    this.areaList = data;
                },
                error => {
                    console.log(error);
                }
            );
    }

    //load territory against area
    fillTerritoryDDL() {
        this.distributionService.getTerritoryListByArea(this.selectedArea)
            .pipe(first())
            .subscribe(
                data => {
                    this.territoryList = data;
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

    async generateDistributorCode() {
        this.distributionService.generateDistributorCode(this.selectedTerritory)
            .pipe(first())
            .subscribe(
                data => {
                    this.regInfoModel.distCode = data.DIST_CODE;


                },
                error => {
                    console.log(error);
                }
            );
    }

    checkMphoneAlreadyExist(): any {
        if (this.regInfoModel.mphone.toString().substring(0, 2) == "01" && this.regInfoModel.mphone.toString().substring(0, 3) != "012") {
            this.distributionService.GetDistributorByMphone(this.regInfoModel.mphone)
                .pipe(first())
                .subscribe(
                    data => {
                        if (data != null) {
                            this.msgs = [];
                            this.msgs.push({ severity: 'error', summary: 'Distributor A/C No : ' + this.regInfoModel.mphone, detail: 'Already Exists!' });
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

    updateDate(value) {
        this.regInfoModel.regDate = new Date(value);
    }

    checkPhotoIdLength() {
        this.formValidation.photoId = this.mfsUtilityService.checkPhotoIdLength(this.regInfoModel.photoId, this.regInfoModel.photoIdTypeCode);
        if (!this.formValidation.photoId) {
            //this.checkNidValid(this.regInfoModel.photoId);
        }
    };
    checkNidValid(value: any): any {
        this.kycService.checkNidValid(value, 'D')
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
    
    getMerchantBankBranchList(): any {
        this.merchantService.getMerchantBankBranchList()
            .pipe(first())
            .subscribe(
                data => {
                    this.merchantBankBranchList = data;
                },
                error => {
                    console.log(error);
                }
            );
    }
    getDistrictByBank(): any {
        this.merchantService.getDistrictByBank(this.regInfoModel.eftBankCode)
            .pipe(first())
            .subscribe(
                data => {
                    this.bankDistrictList = data;
                },
                error => {
                    console.log(error);
                }
            );
    }
    getBankBranchListByBankCodeAndDistCode(): any {
        this.merchantService.getBankBranchListByBankCodeAndDistCode(this.regInfoModel.eftBankCode, this.regInfoModel.eftDistCode)
            .pipe(first())
            .subscribe(
                data => {
                    this.bankBranchByDistBankCodeList = data;
                },
                error => {
                    console.log(error);
                }
            );
    }
    checkForSecureView() {
        if (this.entityId) {
            this.isSecuredViewPermitted = this.authService.checkIsSecuredViewPermitted(this.route.snapshot.routeConfig.path);
            if (this.isSecuredViewPermitted) {
                this.disabledEdit = false;
            }
            else {
                this.disabledEdit = true;
            }
        }
        else {
            this.disabledEdit = false;
        }

    }
    getRoutingNo(): any {
        this.merchantService.getRoutingNo(this.regInfoModel.eftBankCode, this.regInfoModel.eftDistCode, this.regInfoModel.eftBranchCode)
            .pipe(first())
            .subscribe(
                data => {
                    this.regInfoModel.eftRoutingNo = data.routing_no;
                },
                error => {
                    console.log(error);
                }
            );
    }
}
