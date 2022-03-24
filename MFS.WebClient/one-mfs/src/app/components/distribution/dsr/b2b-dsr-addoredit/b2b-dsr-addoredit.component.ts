import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { first } from 'rxjs/operators';
import { DistributorService, DsrService } from 'src/app/services/distribution';
import { Router, ActivatedRoute } from '@angular/router';
import { MessageService, MenuItem, Message } from 'primeng/api';
import { AuthenticationService } from 'src/app/shared/_services';
import { MfsUtilityService } from 'src/app/services/mfs-utility.service';
import { KycService } from '../../../../services/distribution/kyc.service';
import { NgbDatepickerConfig, NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-b2b-dsr-addoredit',
  templateUrl: './b2b-dsr-addoredit.component.html',
  styleUrls: ['./b2b-dsr-addoredit.component.css']
})
export class B2bDsrAddoreditComponent implements OnInit {

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
    DistributorCode: string = "";
    thanaList: any;
    photoIDTypeList: any;
    bankBranchList: any;
    selectedTerritory: string = "0";
    distCode: string;
    currentUserModel: any = {};
    entityId: string;
    isEditMode: boolean = false;
    isRegistrationPermitted: boolean = false;
    regDate: any = {};
    dateOfBirth: any = {};
    msgs: Message[] = [];
    error: boolean = false;
    showDuplicateMsg: boolean = false;
    formValidation: any;
    isLoading: boolean = false;
    checkedAsPresent: boolean = false;
    distributorList: any;
    constructor(private distributorService: DistributorService,
        private dsrService: DsrService,
        private router: Router,
        private route: ActivatedRoute,
        private messageService: MessageService,
        private authService: AuthenticationService,
        private mfsUtilityService: MfsUtilityService,
        private ngbDatepickerConfig: NgbDatepickerConfig,
        private kycService: KycService) {
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
        this.regInfoModel.nationality = 'Bangladeshi';
        this.getRegionListForDDL();
        this.getDivisionListForDDL();
        this.getBankBranchListForDDL();
        this.getB2bDistributorForDDL();
        this.getPhotoIDTypeListForDDL();

        this.entityId = this.route.snapshot.paramMap.get('id');
        if (this.entityId) {
            this.isEditMode = true;
            this.getDsrByMphone();

            this.isRegistrationPermitted = this.authService.checkRegisterPermissionAccess(this.route.snapshot.routeConfig.path);
        }
        if (!this.entityId) {
            var currentDate = new Date();
            this.regDate = { year: currentDate.getFullYear(), month: currentDate.getMonth() + 1, day: currentDate.getDate() };
        }
        this.regInfoModel.partOfFirst = 100;
    }
    sameAsPresent() {
        if (this.checkedAsPresent) {
            this.regInfoModel.preAddr = this.regInfoModel.perAddr;
        }
        else {
            this.regInfoModel.preAddr = '';
        }

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
    getDsrByMphone(): any {
        this.isLoading = true;
        this.distributorService.GetDistributorByMphone(this.entityId)
            .pipe(first())
            .subscribe(
                data => {
                    if (data.distCode) {
                        this.regInfoModel = data;
                        this.getDistCodeByPmhone(this.regInfoModel.pmphone);
                        this.selectedRegion = this.regInfoModel.distCode.substring(0, 2);
                        this.fillAreaDDL();
                        this.selectedArea = this.regInfoModel.distCode.substring(0, 4);
                        this.fillTerritoryDDL();
                        this.selectedTerritory = this.regInfoModel.distCode.substring(0, 6);
                        this.selectedDivision = this.regInfoModel.locationCode.substring(0, 2);
                        this.fillDistrictDDLByDivision();
                        this.selectedDistrict = this.regInfoModel.locationCode.substring(0, 4);
                        this.fillThanaDDLByDistrict();
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
                }
            )
    }
    getDistCodeByPmhone(value: string): any {
        this.distributorService.getDistCodeByPmhone(value)
            .pipe(first())
            .subscribe(
                data => {
                    this.DistributorCode = data;
                },
                error => {
                    console.log(error);
                });
    }
    getDistributorForDDL() {
        this.distributorService.getB2bDistributorListWithDistCodeForDDL()
            .pipe(first())
            .subscribe(
                data => {
                    this.distributorList = data;
                },
                error => {
                    console.log(error);
                }
            );
    }
    getB2bDistributorForDDL() {
        this.distributorService.GetB2bDistributorForB2bDsrListWithDistCodeForDDL()
            .pipe(first())
            .subscribe(
                data => {
                    this.distributorList = data;
                },
                error => {
                    console.log(error);
                }
            );
    }
    GetDistributorCodeByPhotoId(value: string) {
        this.distributorService.GetDistributorCodeByPhotoId(value)
            .pipe(first())
            .subscribe(
                data => {
                    this.DistributorCode = data;
                },
                error => {
                    console.log(error);
                });
    }
    getPhotoIDTypeListForDDL(): any {
        this.distributorService.getPhotoIDTypeListForDDL()
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
        this.distributorService.getBankBranchListForDDL()
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
        this.distributorService.getRegionList()
            .pipe(first())
            .subscribe(
                data => {
                    this.regionList = data;
                },
                error => {
                    console.log(error);
                }
            );
    }

    onStepBack() {
        this.showDuplicateMsg = false;
        if (this.activeIndex > 0) {
            this.activeIndex--;
        }
    }

    onStepAhead(event) {
        this.showDuplicateMsg = false;
        if (this.activeIndex < 4) {
            //this.activeIndex++;
            this.validation(event);
        }
        else {
            this.saveDSR(event);
        }
    }
    validation(event) {

        switch (this.activeIndex) {
            case 0:
                {
                    if (!this.regInfoModel.mphone ||
                        !this.regDate.year ||
                        !this.DistributorCode ||
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
                        !this.selectedDivision ||
                        !this.selectedDistrict ||
                        !this.regInfoModel.locationCode ||
                        !this.regInfoModel.perAddr ||
                        !this.regInfoModel.mphone ||
                        !this.regDate.year ||
                        !this.dateOfBirth.year ||
                        !this.DistributorCode ||
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
                        !this.selectedDivision ||
                        !this.selectedDistrict ||
                        !this.regInfoModel.locationCode ||
                        !this.regInfoModel.perAddr ||
                        !this.regInfoModel.mphone ||
                        !this.regDate.year ||
                        !this.dateOfBirth.year ||
                        !this.DistributorCode ||
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
                    if (!this.regInfoModel.branchCode ||
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
                        !this.DistributorCode ||
                        !this.selectedRegion || this.selectedRegion == '0' ||
                        !this.selectedArea || this.selectedArea == '0' ||
                        !this.selectedTerritory || this.selectedTerritory == '0') {
                        this.msgs = [];
                        this.msgs.push({ severity: 'error', summary: 'Warning! ', detail: 'Cannot be left blank' });
                        if (this.regInfoModel.branchCode) {
                            this.messageService.add({ severity: 'error', summary: 'Cannot be left blank', detail: 'Mandatory input Cannot be left blank', closable: true });
                        }
                        this.error = true;
                        break;
                    } else {
                        this.saveDSR(event);
                        this.error = false;
                        break;
                    }
                }

        }
    }

    saveDSR(event): any {
        this.regInfoModel.regDate = this.mfsUtilityService.renderDate(this.regDate);
        this.regInfoModel.dateOfBirth = this.mfsUtilityService.renderDate(this.dateOfBirth);
        //if (!this.isEditMode) {
        //    this.regInfoModel.entryBy = this.currentUserModel.user.username;
        //}

        if (this.isEditMode && !this.isRegistrationPermitted) {
            this.regInfoModel.updateBy = this.currentUserModel.user.username;
        }
        if (this.isEditMode && this.isRegistrationPermitted) {
            this.regInfoModel.authoBy = this.currentUserModel.user.username;
        }
        if (event === 'save') {
            this.regInfoModel.entryBy = this.currentUserModel.user.username;
        }
        if (this.regInfoModel.distCode != "" || this.regInfoModel.branchName != "") {
            this.dsrService.saveB2bDsr(this.regInfoModel, this.isEditMode, event).pipe(first())
                .subscribe(
                    data => {
                        if (data === 200) {
                            window.history.back();
                            if (this.isEditMode) {
                                this.messageService.add({ severity: 'success', summary: 'Update successfully', sticky: true, detail: 'DSR updated: ' + this.regInfoModel.mphone });
                            }
                            else if
                                (this.isRegistrationPermitted && this.isEditMode) {
                                this.messageService.add({ severity: 'success', summary: 'Register successfully', sticky: true, detail: 'Agent Registered: ' + this.regInfoModel.mphone });
                            }
                            else {
                                this.messageService.add({ severity: 'success', summary: 'Save successfully', sticky: true, detail: 'DSR added: ' + this.regInfoModel.mphone });
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
        this.distributorService.getAreaListByRegion(this.selectedRegion)
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
        this.distributorService.getTerritoryListByArea(this.selectedArea)
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
        this.distributorService.getDivisionList()
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
        this.distributorService.getDistrictListByDivision(this.selectedDivision)
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
        this.distributorService.getDistrictListByDivision(this.selectedDistrict)
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
        this.distributorService.generateDistributorCode(this.selectedTerritory)
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
    async generateB2bDistributorCode() {
        this.distributorService.generateB2bDistributorCode(this.selectedTerritory)
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
    getB2bDistributorDataByDistributorCode() {
        this.dsrService.GetB2bDistributorDataByDistributorCode(this.DistributorCode, 'ABR')
            .pipe(first())
            .subscribe(
                data => {
                    if (data) {
                        if (data.catId === 'ABD') {
                            this.selectedRegion = data.distCode.substring(0, 2);
                            this.fillAreaDDL();
                            this.selectedArea = data.distCode.substring(0, 4);
                            this.fillTerritoryDDL();
                            this.selectedTerritory = data.distCode.substring(0, 6);
                            this.regInfoModel.distCode = data.distCode;
                            this.regInfoModel.companyName = data.companyName;
                            this.regInfoModel.offAddr = data.offAddr;
                            this.regInfoModel.nationality = data.nationality;
                            this.regInfoModel.photoIdTypeCode = data.photoIdTypeCode;
                            this.regInfoModel.photoId = data.photoId;
                            this.regInfoModel.pmphone = data.mphone;
                            this.regInfoModel.ppmphone = data.pmphone;
                        }
                        else {
                            this.DistributorCode = '';
                            this.messageService.add({
                                severity: 'error', summary: 'Not Found',
                                detail: 'Please input distributor dist code', closable: true
                            });
                        }
                        //this.regInfoModel = data;

                    }
                    else {
                        this.messageService.add({
                            severity: 'error', summary: 'Not Found',
                            detail: 'Distributor Not found', closable: true
                        });
                        this.selectedRegion = '';
                        this.selectedArea = '';
                        this.selectedTerritory = '';
                    }
                },
                error => {
                    console.log(error);
                }
            )
    }
    //load data against Distributor Code
    getDistributorDataByDistributorCode(): any {
        this.dsrService.GetDistributorDataByDistributorCode(this.DistributorCode)
            .pipe(first())
            .subscribe(
                data => {
                    if (data) {
                        if (data.catId === 'BD') {
                            this.selectedRegion = data.distCode.substring(0, 2);
                            this.fillAreaDDL();
                            this.selectedArea = data.distCode.substring(0, 4);
                            this.fillTerritoryDDL();
                            this.selectedTerritory = data.distCode.substring(0, 6);
                            this.regInfoModel.distCode = data.distCode;
                            this.regInfoModel.companyName = data.companyName;
                            this.regInfoModel.offAddr = data.offAddr;
                            this.regInfoModel.nationality = data.nationality;
                            this.regInfoModel.photoIdTypeCode = data.photoIdTypeCode;
                            this.regInfoModel.photoId = data.photoId;
                            this.regInfoModel.pmphone = data.mphone;
                        }
                        else {
                            this.DistributorCode = '';
                            this.messageService.add({
                                severity: 'error', summary: 'Not Found',
                                detail: 'Please input distributor dist code', closable: true
                            });
                        }
                        //this.regInfoModel = data;

                    }
                    else {
                        this.messageService.add({
                            severity: 'error', summary: 'Not Found',
                            detail: 'Distributor Not found', closable: true
                        });
                        this.selectedRegion = '';
                        this.selectedArea = '';
                        this.selectedTerritory = '';
                    }
                },
                error => {
                    console.log(error);
                }
            )
    }

    checkMphoneAlreadyExist(): any {
        if (this.regInfoModel.mphone.toString().substring(0, 2) == "01" && this.regInfoModel.mphone.toString().substring(0, 3) != "012") {
            this.distributorService.GetDistributorByMphone(this.regInfoModel.mphone)
                .pipe(first())
                .subscribe(
                    data => {
                        if (data != null) {
                            this.msgs = [];
                            this.msgs.push({ severity: 'error', summary: 'DSR A/C No : ' + this.regInfoModel.mphone, detail: 'Already Exists!' });
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
    checkPhotoIdLength() {
        this.formValidation.photoId = this.mfsUtilityService.checkPhotoIdLength(this.regInfoModel.photoId, this.regInfoModel.photoIdTypeCode);
        if (!this.formValidation.photoId) {
            this.checkNidValid(this.regInfoModel.photoId);
        }
    };
    checkNidValid(value: any): any {
        this.kycService.checkNidValid(value, 'R')
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
