
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
import { Reginfo } from '../../../../shared/_models/reginfo';
@Component({
    selector: 'app-customer-addoredit',
    templateUrl: './customer-addoredit.component.html',
    styleUrls: ['./customer-addoredit.component.css'],
    encapsulation: ViewEncapsulation.None
})
export class CustomerAddoreditComponent implements OnInit {
    public reginfo: Reginfo;
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
        this.reginfo = new Reginfo();
        this.reginfo.customeNumberInput = this.mfsUtilityService.getPositiveWholeNumberRegExp();
        this.reginfo.alphabetsWithSpace = this.mfsUtilityService.getAlphabetsWithSpaceEegExp();
        this.reginfo.emailRegx = this.mfsUtilityService.getEmailRegExp();
        ngbDatepickerConfig.minDate = { year: 1919, month: 1, day: 1 };
        var currentDate = new Date();
        ngbDatepickerConfig.maxDate = { year: currentDate.getFullYear(), month: currentDate.getMonth() + 1, day: currentDate.getDate() };
        this.authService.currentUser.subscribe(x => {
            this.reginfo.currentUserModel = x;
        });
        this.reginfo.formValidation = {};
    }

    ngOnInit() {
        this.reginfo.items = [
            {
                label: 'Office use only',
                command: (event: any) => {
                    this.reginfo.activeIndex = 0;
                    //this.reginfo.messageService.add({ severity: 'success', summary: 'Service Message', detail: 'Via MessageService' });
                }
            },
            {
                label: 'Personal details',
                command: (event: any) => {
                    this.reginfo.activeIndex = 1;
                }
            },
            {
                label: 'Nominee info',
                command: (event: any) => {
                    this.reginfo.activeIndex = 2;
                    //this.reginfo.messageService.add({ severity: 'info', summary: 'Pay with CC', detail: event.item.label });
                }
            },
            {
                label: 'Introducer info',
                command: (event: any) => {
                    this.reginfo.activeIndex = 3;
                    //this.reginfo.messageService.add({ severity: 'info', summary: 'Pay with CC', detail: event.item.label });
                }
            }
            //},
            //{
            //    label: 'Bank info',
            //    command: (event: any) => {
            //        this.reginfo.activeIndex = 4;
            //        //this.reginfo.messageService.add({ severity: 'info', summary: 'Last Step', detail: event.item.label });
            //    }
            //}
        ];

        this.reginfo.genderTypes = [
            { label: 'Male', value: 'M', icon: 'fas fa-male' },
            { label: 'Female', value: 'F', icon: 'fas fa-female' },
            { label: 'Others', value: 'O', icon: 'fas fa-transgender-alt' }
        ];
        this.reginfo.kycTypes = [
            { label: 'From KYC Paper', value: 'K', icon: 'fa fa-file' },
            { label: 'From CBS Account', value: 'C', icon: 'fa fa-university' }
           
        ]
        this.reginfo.religeonList = [
            { label: 'Islam', value: 'Islam' },
            { label: 'Hinduism', value: 'Hinduism' },
            { label: 'Chritianity', value: 'Chritianity' },
            { label: 'Buddhism', value: 'Buddhism' },
            { label: 'Other', value: 'Other' }
        ];

        this.reginfo.relationList = [
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
        this.reginfo.regInfoModel.nationality = 'Bangladeshi';
        this.reginfo.entityId = this.route.snapshot.paramMap.get('id');
        if (this.reginfo.entityId) {
            this.reginfo.isEditMode = true;
            this.GetCustomerByMphone();
            this.reginfo.isRegPermit = this.authService.checkRegisterPermissionAccess(this.route.snapshot.routeConfig.path);
            if (this.reginfo.isRegPermit) {
                this.reginfo.isNidVerified = false;
            }
            else {
                this.reginfo.isNidVerified = true;
            }          
        }
        if (!this.reginfo.entityId) {
            var currentDate = new Date();
            this.reginfo.isNidVerified = true;
            this.reginfo.regDate = { year: currentDate.getFullYear(), month: currentDate.getMonth() + 1, day: currentDate.getDate() };
        }
        this.reginfo.regInfoModel.partOfFirst = 100;
    }
    validateDatepicker(event) {
        if (this.reginfo.dateOfBirth) {
            var validate = this.mfsUtilityService.validateDatePickerInput(this.reginfo.dateOfBirth);

            if (!validate) {
                //this.reginfo.messageService.add({ severity: 'error', summary: 'Invalid Date Formate', detail: 'Please Input Valid Date', closable: true });
                this.reginfo.dateOfBirth = null;
            }
        }
    }
    switchInputMethod() {
        if (this.reginfo.selectedKycType === 'C') {
            this.reginfo.showCbsModal = true;
        }
        else {
            this.reginfo.showCbsModal = false;
        }
    }
    checkMphoneAlreadyExist(): any {
        //var mobileNo: string = this.reginfo.regInfoModel.mphone;
        //var operatorCode: string = mobileNo.substring(0, 2);
        //var toMatch: string = "01";        
        if (this.reginfo.regInfoModel.mphone.toString().substring(0, 2) == "01" && this.reginfo.regInfoModel.mphone.toString().substring(0, 3) != "012") {
            this.distributionService.GetDistributorByMphone(this.reginfo.regInfoModel.mphone)
                .pipe(first())
                .subscribe(
                    data => {
                        if (data != null) {
                            this.reginfo.msgs = [];
                            this.reginfo.msgs.push({ severity: 'error', summary: 'Customer A/C No : ' + this.reginfo.regInfoModel.mphone, detail: 'Already Exists!' });
                            this.reginfo.regInfoModel.mphone = null;
                            this.reginfo.showDuplicateMsg = true;
                        }
                        else {
                            this.reginfo.showDuplicateMsg = false;
                        }
                    },
                    error => {
                        console.log(error);
                    }
                );
        }
        else {
            this.reginfo.regInfoModel.mphone = '';
            this.messageService.add({ severity: 'error', summary: 'Invalid Mobile No', detail: 'Please Input Valid Mobiel No', closable: true });
        }
    }
    GetCustomerByMphone(): any {
        this.reginfo.isLoading = true;
        this.customerService.getCustomerByMphone(this.reginfo.entityId)
            .pipe(first())
            .subscribe(
                data => {
                    if (data) {
                        this.reginfo.regInfoModel = data;
                        if (data.locationCode) {
                            this.reginfo.selectedDivision = this.reginfo.regInfoModel.locationCode.substring(0, 2);
                            this.fillDistrictDDLByDivision();
                            this.reginfo.selectedDistrict = this.reginfo.regInfoModel.locationCode.substring(0, 4);
                            this.fillThanaDDLByDistrict();
                        }

                        //this.reginfo.GetDistributorCodeByPhotoId(data.photoId);
                        this.reginfo.regDate = this.mfsUtilityService.renderDateObject(data.regDate);
                        if (data.dateOfBirth != null) {
                            this.reginfo.dateOfBirth = this.mfsUtilityService.renderDateObject(data.dateOfBirth);
                        }
                        if (this.reginfo.regInfoModel.regStatus === 'P') {
                            this.reginfo.isReject = false;
                        }
                        else {
                            this.reginfo.isReject = true;
                        }
                    }
                    this.reginfo.isLoading = false;
                },
                error => {
                    this.reginfo.isLoading = false;
                    console.log(error);
                });
    }
    GetDistributorCodeByPhotoId(value: string) {
        this.distributionService.GetDistributorCodeByPhotoId(value)
            .pipe(first())
            .subscribe(
                data => {
                    this.reginfo.DistributorCode = data;
                },
                error => {
                    console.log(error);
                });
    }
    GenerateAgentCode() {
        this.agentService.GenerateAgentCode(this.reginfo.selectedCluster)
            .pipe(first())
            .subscribe(
                data => {
                    this.reginfo.regInfoModel.distCode = data.DIST_CODE;
                },
                error => {
                    console.log(error);
                });
    }


    GetclusterByTerritoryCode() {
        this.agentService.GetclusterByTerritoryCode(this.reginfo.selectedTerritory)
            .pipe(first())
            .subscribe(
                data => {
                    this.reginfo.clusterList = data;
                },
                error => {
                    console.log(error);
                }
            );
    }
    async fillThanaDDLByDistrict() {
        this.distributionService.getDistrictListByDivision(this.reginfo.selectedDistrict)
            .pipe(first())
            .subscribe(
                data => {
                    this.reginfo.thanaList = data;
                },
                error => {
                    console.log(error);
                }
            );
    }
    async fillDistrictDDLByDivision() {
        this.distributionService.getDistrictListByDivision(this.reginfo.selectedDivision)
            .pipe(first())
            .subscribe(
                data => {
                    this.reginfo.districtList = data;
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
                    this.reginfo.clusterList = data;
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
                    this.reginfo.photoIDTypeList = data;
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
                    this.reginfo.bankBranchList = data;
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
                    this.reginfo.divisionList = data;
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
                    this.reginfo.occupationList = data;
                },
                error => {
                    console.log(error);
                }
            );
    }

    onStepAhead(event) {
        if (this.reginfo.activeIndex < 4) {
            this.validation(event);
            //this.reginfo.activeIndex++;
        }
        else {
            //this.reginfo.SaveCustomer(event);
        }
    }

    onStepBack() {
        if (this.reginfo.activeIndex > 0) {
            this.reginfo.activeIndex--;
        }
    }
    validation(event) {
        if (event == 'reject') {
            this.SaveCustomer(event);
            this.reginfo.error = false;
        }
        else {
            switch (this.reginfo.activeIndex) {
                case 0:
                    {
                        if (!this.reginfo.regInfoModel.mphone || !this.reginfo.regDate.year) {
                            this.reginfo.msgs = [];
                            this.reginfo.msgs.push({ severity: 'error', summary: 'Warning! ', detail: 'Cannot be left blank' });
                            this.reginfo.error = true;
                            break;
                        }
                        else {
                            this.reginfo.activeIndex++;
                            this.reginfo.error = false;
                            break;
                        }
                    }
                case 1:
                    {
                        if (!this.reginfo.regInfoModel.name ||
                            !this.reginfo.regInfoModel.photoIdTypeCode ||
                            !this.reginfo.regInfoModel.photoId ||
                            !this.reginfo.selectedDivision ||
                            !this.reginfo.selectedDistrict ||
                            !this.reginfo.regInfoModel.locationCode ||
                            !this.reginfo.regInfoModel.occupation ||
                            !this.reginfo.regInfoModel.perAddr ||
                            !this.reginfo.regInfoModel.mphone ||
                            !this.reginfo.regDate.year ||
                            !this.reginfo.regInfoModel.fatherName ||
                            !this.reginfo.regInfoModel.motherName ||
                            !this.reginfo.dateOfBirth.year ||
                            !this.reginfo.regInfoModel.gender) {
                            this.reginfo.msgs = [];
                            this.reginfo.msgs.push({ severity: 'error', summary: 'Warning! ', detail: 'Cannot be left blank' });
                            this.reginfo.error = true;
                            break;
                        } else {
                            this.reginfo.activeIndex++;
                            this.reginfo.error = false;
                            break;
                        }
                    }
                case 2:
                    {
                        if (!this.reginfo.regInfoModel.name ||
                            !this.reginfo.regInfoModel.photoIdTypeCode ||
                            !this.reginfo.regInfoModel.photoId ||
                            !this.reginfo.selectedDivision ||
                            !this.reginfo.selectedDistrict ||
                            !this.reginfo.regInfoModel.locationCode ||
                            !this.reginfo.regInfoModel.occupation ||
                            !this.reginfo.regInfoModel.perAddr ||
                            !this.reginfo.regInfoModel.mphone ||
                            !this.reginfo.regDate.year ||
                            !this.reginfo.dateOfBirth.year ||
                            !this.reginfo.regInfoModel.fatherName ||
                            !this.reginfo.regInfoModel.motherName ||
                            !this.reginfo.regInfoModel.firstNomineeName ||
                            !this.reginfo.regInfoModel.gender) {
                            this.reginfo.msgs = [];
                            this.reginfo.msgs.push({ severity: 'error', summary: 'Warning! ', detail: 'Cannot be left blank' });
                            this.reginfo.error = true;
                            break;
                        } else {
                            this.reginfo.activeIndex++;
                            this.reginfo.error = false;
                            break;
                        }
                    }
                //case 3:
                //    {
                //        //this.reginfo.activeIndex++;
                //        //this.reginfo.error = false;
                //        //break;
                //        this.reginfo.SaveCustomer(event);
                //        this.reginfo.error = false;
                //        break;
                //    }
                case 3:
                    {
                        if (!this.reginfo.regInfoModel.name ||
                            !this.reginfo.regInfoModel.photoIdTypeCode ||
                            !this.reginfo.regInfoModel.photoId ||
                            !this.reginfo.selectedDivision ||
                            !this.reginfo.selectedDistrict ||
                            !this.reginfo.regInfoModel.locationCode ||
                            !this.reginfo.regInfoModel.occupation ||
                            !this.reginfo.regInfoModel.perAddr ||
                            !this.reginfo.regInfoModel.mphone ||
                            !this.reginfo.regDate.year ||
                            !this.reginfo.regInfoModel.fatherName ||
                            !this.reginfo.regInfoModel.motherName ||
                            !this.reginfo.regInfoModel.firstNomineeName ||
                            !this.reginfo.dateOfBirth.year ||
                            !this.reginfo.regInfoModel.gender) {
                            this.reginfo.msgs = [];
                            this.reginfo.msgs.push({ severity: 'error', summary: 'Warning! ', detail: 'Cannot be left blank' });
                            this.messageService.add({ severity: 'error', summary: 'Cannot be left blank', detail: 'Mandatory input Cannot be left blank', closable: true });
                            this.reginfo.error = true;
                            break;
                        } else {
                            this.SaveCustomer(event);
                            this.reginfo.error = false;
                            break;
                        }
                    }

            }
        }

    }

    onRejectCustomer(event) {
        this.reginfo.disableButton = true;
        if (event === 'reject') {
            this.customerService.save(this.reginfo.regInfoModel, this.reginfo.isEditMode, event).pipe(first())
                .subscribe(
                    data => {
                        if (data === 200) {
                            this.reginfo.showRejectModal = false;
                            this.reginfo.disableButton = false;
                            window.history.back();
                            if (event == 'reject') {
                                this.messageService.add({ severity: 'success', summary: 'Reject successfully', sticky: true, detail: 'Customer rejected: ' + this.reginfo.regInfoModel.mphone });
                            }
                        }
                        else {
                            this.messageService.add({ severity: 'error', summary: 'Erros in: ' + this.reginfo.regInfoModel.mphone, sticky: true, detail: 'Bad Response from BackEnd', closable: true });
                        }
                    },
                    error => {
                        console.log(error);
                        this.messageService.add({ severity: 'error', summary: 'Erros Occured', sticky: true, detail: error, closable: false });
                    });
        }
        this.reginfo.showRejectModal = false;
        this.reginfo.disableButton = false;
    }

    sameAsPresent() {
        if (this.reginfo.checkedAsPresent) {
            this.reginfo.regInfoModel.preAddr = this.reginfo.regInfoModel.perAddr;
        }
        else {
            this.reginfo.regInfoModel.preAddr = '';
        }

    }
    SaveCustomer(event): any {
        this.reginfo.regInfoModel.regDate = this.mfsUtilityService.renderDate(this.reginfo.regDate);
        this.reginfo.regInfoModel.dateOfBirth = this.mfsUtilityService.renderDate(this.reginfo.dateOfBirth);
        if (this.reginfo.isNidVerified) {
            this.reginfo.regInfoModel.photoidValidation = 'Y';
        }
        else {
            this.reginfo.regInfoModel.photoidValidation = 'N';
        }

        if (event === 'edit') {
            this.reginfo.regInfoModel.updateBy = this.reginfo.currentUserModel.user.username;
            this.reginfo.regInfoModel.branchCode = this.reginfo.currentUserModel.user.branchCode;
        }
        if (event === 'register') {
            this.reginfo.regInfoModel.authoBy = this.reginfo.currentUserModel.user.username;
        }
        if (event === 'save') {
            this.reginfo.regInfoModel.entryBy = this.reginfo.currentUserModel.user.username;
            this.reginfo.regInfoModel.branchCode = this.reginfo.currentUserModel.user.branchCode;
        }
        if (event === 'reject') {
            this.reginfo.regInfoModel.updateBy = this.reginfo.currentUserModel.user.username;
            this.reginfo.showRejectModal = true;
        }
        if (this.reginfo.regInfoModel.mphone != "" && event != 'reject') {
            this.customerService.save(this.reginfo.regInfoModel, this.reginfo.isEditMode, event).pipe(first())
                .subscribe(
                    data => {
                        if (data === 200) {
                            window.history.back();
                            if (event == 'reject') {
                                this.messageService.add({ severity: 'error', summary: 'Reject successfully', sticky: true, detail: 'Customer rejected: ' + this.reginfo.regInfoModel.mphone });
                            }

                            else if (this.reginfo.isEditMode && !this.reginfo.isRegPermit) {
                                this.messageService.add({ severity: 'success', summary: 'Update successfully', sticky: true, detail: 'Customer Updated: ' + this.reginfo.regInfoModel.mphone });
                            }
                            else if (this.reginfo.isRegPermit && this.reginfo.isRegPermit) {
                                this.messageService.add({ severity: 'success', summary: 'Register successfully', sticky: true, detail: 'Customer Registered: ' + this.reginfo.regInfoModel.mphone });
                            }
                            else {
                                this.messageService.add({ severity: 'success', summary: 'Save successfully', sticky: true, detail: 'Customer added: ' + this.reginfo.regInfoModel.mphone });
                            }
                        }
                        else if (data === 'DATAEXIST') {
                            window.history.back();
                            this.messageService.add({ severity: 'warn', summary: 'Warning For: ' + this.reginfo.regInfoModel.mphone, sticky: true, detail: 'Account is already Exist', closable: true });
                        }
                        else {
                            this.messageService.add({ severity: 'error', summary: 'Erros in: ' + this.reginfo.regInfoModel.mphone, sticky: true, detail: 'Bad Response from BackEnd', closable: true });
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
        this.reginfo.formValidation.photoId = this.mfsUtilityService.checkPhotoIdLength(this.reginfo.regInfoModel.photoId, this.reginfo.regInfoModel.photoIdTypeCode);
        if (!this.reginfo.formValidation.photoId) {
            this.checkNidValid(this.reginfo.regInfoModel.photoId);
        }
        else {
            this.reginfo.regInfoModel.photoId = '';
        }
    };

    checkNidValid(value: any): any {
        this.kycService.checkNidValid(value, 'C')
            .pipe(first())
            .subscribe(
                data => {
                    if (data === false) {
                        this.messageService.add({ severity: 'error', summary: 'Duplicate ID', detail: 'Photo Id Already Exists!', closable: true });
                        this.reginfo.regInfoModel.photoId = '';
                    }
                },
                error => {
                    console.log(error);
                });
    }
}
