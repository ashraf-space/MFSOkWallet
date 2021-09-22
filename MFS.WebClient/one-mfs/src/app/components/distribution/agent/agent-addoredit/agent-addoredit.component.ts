import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { DistributorService, DsrService } from 'src/app/services/distribution';
import { Router, ActivatedRoute } from '@angular/router';
import { error } from 'selenium-webdriver';
import { first } from 'rxjs/operators';
import { MessageService, MenuItem } from 'primeng/api';
import { AuthenticationService } from 'src/app/shared/_services';
import { AgentService } from '../../../../services/distribution/agent.service';
import { MfsUtilityService } from '../../../../services/mfs-utility.service';
import { Message } from 'primeng/components/common/api';
import { NgbDatepickerConfig, NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { KycService } from '../../../../services/distribution/kyc.service';

@Component({
    selector: 'app-agent-addoredit',
    templateUrl: './agent-addoredit.component.html',
    styleUrls: ['./agent-addoredit.component.css'],
    encapsulation: ViewEncapsulation.None
})
export class AgentAddoreditComponent implements OnInit {
    isRegPermit = false;
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
    selectedCluster: string = "0";
    clusterList: any;
    distCode: string;
    currentUserModel: any = {};
    entityId: string;
    isEditMode: boolean = false;
    DistributorCode: string = "";

    positveNumber: RegExp;
    isLoading: boolean = false;
    regDate: any = {};
    dateOfBirth: any = {};
    territoryCode: any;
    msgs: Message[] = [];
    error: boolean = false;
    showDuplicateMsg: boolean = false;
    customeNumberInput: any;
    formValidation: any;
    mobileNoRegEx: RegExp;
    dobRegEx: RegExp;
    alphabetsWithSpace: any;
    checkedAsPresent: boolean = false;
    distributorList: any;
    constructor(private distributionService: DistributorService,
        private router: Router,
        private route: ActivatedRoute,
        private messageService: MessageService,
        private authService: AuthenticationService,
        private agentService: AgentService,
        private dsrService: DsrService,
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
        this.positveNumber = this.mfsUtilityService.getPositiveWholeNumberRegExp();
        this.mobileNoRegEx = this.mfsUtilityService.getMobileNoRegExp();
        this.alphabetsWithSpace = this.mfsUtilityService.getAlphabetsWithSpaceEegExp();
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
        this.getDistributorForDDL();
        this.getPhotoIDTypeListForDDL();
        this.regInfoModel.nationality = 'Bangladeshi';
        this.entityId = this.route.snapshot.paramMap.get('id');
        if (this.entityId) {
            this.isEditMode = true;
            this.GetAgentByMphone();
            this.isRegPermit = this.authService.checkRegisterPermissionAccess(this.route.snapshot.routeConfig.path);
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
    checkMphoneAlreadyExist(): any {
        if (this.regInfoModel.mphone.toString().substring(0, 2) == "01" && this.regInfoModel.mphone.toString().substring(0, 3) != "012") {
            this.agentService.GetAgentByMobilePhone(this.regInfoModel.mphone)
                .pipe(first())
                .subscribe(
                    data => {
                        if (data != null) {
                            this.msgs = [];
                            this.msgs.push({ severity: 'error', summary: 'Agent A/C No : ' + this.regInfoModel.mphone, detail: 'Already Exists!' });
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
    //checkMphoneAlreadyExist(): any {
    //    this.distributionService.GetDistributorByMphone(this.regInfoModel.mphone)
    //        .pipe(first())
    //        .subscribe(
    //            data => {
    //                if (data != null) {
    //                    this.msgs = [];
    //                    this.msgs.push({ severity: 'error', summary: 'Agent A/C No : ' + this.regInfoModel.mphone, detail: 'Already Exists!' });
    //                    this.regInfoModel.mphone = null;
    //                    this.showDuplicateMsg = true;
    //                }
    //            },
    //            error => {
    //                console.log(error);
    //            }
    //        );
    //}
    GetAgentByMphone(): any {
        this.isLoading = true;
        this.agentService.GetAgentByMobilePhone(this.entityId)
            .pipe(first())
            .subscribe(
                data => {
                    if (data.distCode) {
                        this.regInfoModel = data;
                        this.regInfoModel.distCode = this.regInfoModel.distCode;
                        this.getDistCodeByPmhone(this.regInfoModel.pmphone);
                        this.selectedRegion = this.regInfoModel.distCode.substring(0, 2);
                        this.fillAreaDDL();
                        this.selectedArea = this.regInfoModel.distCode.substring(0, 4);
                        this.fillTerritoryDDL();
                        this.fillClusterDDL();
                        this.selectedCluster = this.regInfoModel.distCode.substring(0, 8);
                        this.selectedTerritory = this.regInfoModel.distCode.substring(0, 6);
                        this.selectedDivision = this.regInfoModel.locationCode.substring(0, 2);
                        this.fillDistrictDDLByDivision();
                        this.selectedDistrict = this.regInfoModel.locationCode.substring(0, 4);
                        this.fillThanaDDLByDistrict();
                        //this.getDistCodeByAgentInfo();           
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
    getDistCodeByPmhone(value: string): any {
        this.distributionService.getDistCodeByPmhone(value)
            .pipe(first())
            .subscribe(
                data => {
                    this.DistributorCode = data;
                },
                error => {
                    console.log(error);
                });
    }
    getDistCodeByAgentInfo(): any {
        this.territoryCode = this.regInfoModel.distCode.substring(0, 6);
        this.agentService.getDistCodeByAgentInfo(this.territoryCode, this.regInfoModel.companyName, this.regInfoModel.offAddr)
            .pipe(first())
            .subscribe(
                data => {
                    this.DistributorCode = data.DIST_CODE;
                },
                error => {
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
    async GenerateAgentCode() {
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
    GetInfoByDistributor(): any {
        this.isLoading = true;
        this.dsrService.GetDistributorDataByDistributorCode(this.DistributorCode)
            .pipe(first())
            .subscribe(
                data => {
                    //this.regInfoModel = data;
                    if (data) {
                        this.isLoading = false;
                        if (data.catId === 'D') {
                            this.getRegionListForDDL();
                            this.selectedRegion = data.distCode.substring(0, 2);
                            this.fillAreaDDL();
                            this.selectedArea = data.distCode.substring(0, 4);
                            this.fillTerritoryDDL();
                            this.selectedTerritory = data.distCode.substring(0, 6);
                            this.GetclusterByTerritoryCode();
                            this.regInfoModel.companyName = data.companyName;
                            this.regInfoModel.offAddr = data.offAddr;
                            this.regInfoModel.pmphone = data.mphone;
                        }
                        else {
                            this.DistributorCode = '';
                            this.messageService.add({
                                severity: 'error', summary: 'Not Found',
                                detail: 'Please input distributor dist code', closable: true
                            });
                        }
                    }
                    else {
                        this.isLoading = false;
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
                    this.isLoading = false;
                    console.log(error);
                });
    }

    GetclusterByTerritoryCode() {
        this.agentService.GetclusterByTerritoryCode(this.selectedTerritory)
            .pipe(first())
            .subscribe(
                data => {
                    this.clusterList = data;
                    this.clusterList.unshift({ label: 'Please Select', value: null });
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
    getDistributorForDDL() {
        this.distributionService.getDistributorListWithDistCodeForDDL()
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
                    this.photoIDTypeList.unshift({ label: 'Please Select', value: null });
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
    async getRegionListForDDL() {
        this.distributionService.getRegionList()
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
                        !this.DistributorCode ||
                        !this.selectedCluster || this.selectedCluster == '0') {
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
                    if (!this.regInfoModel.mphone ||
                        !this.regDate.year ||
                        !this.DistributorCode ||
                        !this.selectedCluster || this.selectedCluster == '0' ||
                        !this.regInfoModel.companyName ||
                        !this.regInfoModel.name ||
                        !this.regInfoModel.conMob ||
                        !this.regInfoModel.offAddr ||
                        !this.regInfoModel.photoIdTypeCode ||
                        !this.regInfoModel.photoId ||
                        !this.selectedDivision ||
                        !this.selectedDistrict ||
                        !this.regInfoModel.locationCode ||
                        !this.regInfoModel.perAddr ||
                        !this.dateOfBirth.year ||
                        !this.regInfoModel.tradeLicenseNo
                    ) {
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
                    if (!this.regInfoModel.mphone ||
                        !this.regDate.year ||
                        !this.DistributorCode ||
                        !this.selectedCluster || this.selectedCluster == '0' ||
                        !this.regInfoModel.companyName ||
                        !this.regInfoModel.name ||
                        !this.regInfoModel.conMob ||
                        !this.regInfoModel.offAddr ||
                        !this.regInfoModel.photoIdTypeCode ||
                        !this.regInfoModel.photoId ||
                        !this.selectedDivision ||
                        !this.selectedDistrict ||
                        !this.regInfoModel.locationCode ||
                        !this.regInfoModel.perAddr ||
                        !this.dateOfBirth.year ||
                        !this.regInfoModel.tradeLicenseNo                       
                    ) {
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
                    if (!this.regInfoModel.mphone ||
                        !this.regDate.year ||
                        !this.DistributorCode ||
                        !this.selectedCluster || this.selectedCluster == '0' ||
                        !this.regInfoModel.companyName ||
                        !this.regInfoModel.name ||
                        !this.regInfoModel.conMob ||
                        !this.regInfoModel.offAddr ||
                        !this.regInfoModel.photoIdTypeCode ||
                        !this.regInfoModel.photoId ||
                        !this.selectedDivision ||
                        !this.selectedDistrict ||
                        !this.dateOfBirth.year ||
                        !this.regInfoModel.locationCode ||
                        !this.regInfoModel.perAddr ||
                        !this.regInfoModel.tradeLicenseNo ||                     
                        !this.regInfoModel.branchCode) {
                        this.msgs = [];
                        this.msgs.push({ severity: 'error', summary: 'Warning! ', detail: 'Cannot be left blank' });
                        if (this.regInfoModel.branchCode) {
                            this.messageService.add({ severity: 'error', summary: 'Cannot be left blank', detail: 'Mandatory input Cannot be left blank', closable: true });
                        }
                        this.error = true;
                        break;
                    } else {
                        this.SaveAgent(event);
                        this.error = false;
                        break;
                    }
                }

        }

    }




    SaveAgent(event): any {
        //this.regInfoModel.entryBy = this.currentUserModel.user.username;
        this.regInfoModel.regDate = this.mfsUtilityService.renderDate(this.regDate);
        this.regInfoModel.dateOfBirth = this.mfsUtilityService.renderDate(this.dateOfBirth);
        if (this.isEditMode && !this.isRegPermit) {
            this.regInfoModel.updateBy = this.currentUserModel.user.username;
        }
        if (this.isEditMode && this.isRegPermit) {
            this.regInfoModel.authoBy = this.currentUserModel.user.username;
        }
        if (event === 'save') {
            this.regInfoModel.entryBy = this.currentUserModel.user.username;           
        }
        if (this.regInfoModel.distCode != "" || this.regInfoModel.branchName != "") {
            this.agentService.save(this.regInfoModel, this.isEditMode, event).pipe(first())
                .subscribe(
                    data => {
                        if (data === 200) {
                            window.history.back();
                            if (this.isEditMode && !this.isRegPermit) {
                                this.messageService.add({ severity: 'success', summary: 'Update successfully', sticky: true, detail: 'Agent Updated: ' + this.regInfoModel.mphone });
                            }
                            else if (this.isRegPermit && this.isEditMode) {
                                this.messageService.add({ severity: 'success', summary: 'Register successfully' + this.regInfoModel.mphone, sticky: true, detail: 'Agent Registered: ' + this.regInfoModel.mphone});
                            }
                            else {
                                this.messageService.add({ severity: 'success', summary: 'Save successfully' + this.regInfoModel.mphone, sticky: true, detail: 'Agent added: ' + this.regInfoModel.mphone });
                            }
                        }
                        else {
                            this.messageService.add({ severity: 'error', summary: 'Erros in: ' + this.regInfoModel.mphone, sticky: true, detail: 'Bad Response from BackEnd', closable: true });
                        }
                        
                    },
                    error => {
                        console.log(error);
                        this.messageService.add({ severity: 'error', summary: 'Erros Occured', sticky: true, detail: error, closable: true });
                    });
        }
    }
    cancel() {
        window.history.back();
    }

    checkPhotoIdLength() {
        this.formValidation.photoId = this.mfsUtilityService.checkPhotoIdLength(this.regInfoModel.photoId, this.regInfoModel.photoIdTypeCode);
        if (!this.formValidation.photoId) {
            //this.checkNidValid(this.regInfoModel.photoId);
        }
    };

    checkNidValid(value: any): any {
        this.kycService.checkNidValid(value, 'A')
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
