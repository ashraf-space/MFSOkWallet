import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { MerchantService, DistributorService } from 'src/app/services/distribution';
import { Router, ActivatedRoute } from '@angular/router';
import { MessageService, MenuItem, Message } from 'primeng/api';
import { AuthenticationService } from 'src/app/shared/_services';
import { first } from 'rxjs/operators';
import { MfsUtilityService } from 'src/app/services/mfs-utility.service';
import { SelectItem } from 'primeng/api';
import { KycService } from '../../../../services/distribution/kyc.service';

@Component({
    selector: 'app-merchant-addoredit',
    templateUrl: './merchant-addoredit.component.html',
    styleUrls: ['./merchant-addoredit.component.css'],
    encapsulation: ViewEncapsulation.None
})
export class MerchantAddoreditComponent implements OnInit {
    regInfoModel: any = {};
    merchantConfig: any = {};
    activeIndex: number = 0;
    items: MenuItem[];
    genderTypes: any;
    religeonList: any;
    relationList: any;
    divisionList: any;
    selectedDivision: string = "0";
    districtList: any;
    selectedDistrict: string = "0";
    thanaList: any;
    photoIDTypeList: any;
    merchantCodeList: any;
    merchantBankBranchList: any;
    bankBranchList: any;
    bankBranchByDistBankCodeList: any;
    bankDistrictList: any;
    distCode: string;
    currentUserModel: any = {};
    entityId: string;
    isEditMode: boolean = false;
    isRegistrationPermitted: boolean = false;
    msgs: Message[] = [];
    msgsDuplicate: Message[] = [];
    error: boolean = false;
    showDuplicateMsg: boolean = false;
    formValidation: any;
    selectedCategory: string = "0";
    selectedMtype: string = "0";
    selectedAreatype: string = "0";
    selectSettlementDate: string[];
    mAreaList: any;
    mTypeList: any;
    mCatList: any;
    dateList: SelectItem[];
    cycleList: any;
    // selectedCycle: string[];
    selectedCycle: string = "0";
    showWeeklyDate: boolean = false;
    positveNumber: RegExp;
    isLoading: boolean = false;
    selectedCycleWeekDay: SelectItem[];
    constructor(private merchantService: MerchantService,
        private distributorService: DistributorService,
        private router: Router,
        private route: ActivatedRoute,
        private messageService: MessageService,
        private authService: AuthenticationService,
        private mfsUtilityService: MfsUtilityService,
        private kycService: KycService) {

        this.authService.currentUser.subscribe(x => {
            this.currentUserModel = x;
        });
        this.formValidation = {};
        this.positveNumber = this.mfsUtilityService.getPositiveWholeNumberRegExp();
    }

    ngOnInit() {

        this.getDivisionListForDDL();
        this.getBankBranchListForDDL();

        this.getPhotoIDTypeListForDDL();

        //this.getMerchantCodeListForDDL();
        this.getMerchantBankBranchList();
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
        this.genderTypes = [
            { label: 'Male', value: 'M', icon: 'fas fa-male' },
            { label: 'Female', value: 'F', icon: 'fas fa-female' },
            { label: 'Others', value: 'O', icon: 'fas fa-transgender-alt' }
        ];

        this.mCatList = [
            { label: 'Individual Merchant', value: 'M' },
            { label: 'Chain (Parent) Merchant', value: 'C' }
        ]

        this.mAreaList = [
            { label: 'Urban', value: 'Urban' },
            { label: 'Rural', value: 'Rural' }
        ]

        this.cycleList = [
            { label: 'Daily', value: 'daily' },
            { label: 'Weekly', value: 'weekly' },
            { label: 'Monthly', value: 'monthly' }
        ]
        this.mTypeList = this.mfsUtilityService.getMTypelIst();
        this.dateList = this.mfsUtilityService.getDateList();
        this.entityId = this.route.snapshot.paramMap.get('id');
        if (this.entityId) {
            this.isEditMode = true;
            this.getMerChantByMphone();
            this.isRegistrationPermitted = this.authService.checkRegisterPermissionAccess(this.route.snapshot.routeConfig.path);
        }
    }
    viewWeeklyDate() {
        if (this.selectedCycle.toString() === 'weekly') {
            this.showWeeklyDate = true;
        }
        else {
            this.showWeeklyDate = false;
            this.selectSettlementDate = [];
        }
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



    getMerchantCodeListForDDL(): any {
        this.merchantService.getMerchantCodeListForDDL()
            .pipe(first())
            .subscribe(
                data => {
                    this.merchantCodeList = data;
                },
                error => {
                    console.log(error);
                }
            );
    }

    generateMerchantCode(): any {
        this.merchantService.generateMerchantCode(this.selectedCategory)
            .pipe(first())
            .subscribe(
                data => {
                    this.regInfoModel._mcode = data.M_CODE;
                },
                error => {
                    console.log(error);
                }
            );
    }

    getMerChantByMphone(): any {
        this.isLoading = true;
        this.merchantService.getMerChantByMphone(this.entityId)
            .pipe(first())
            .subscribe(
                data => {
                    if (data) {
                        this.regInfoModel = data;
                        if (data._SelectedCycleWeekDay) {
                            this.selectedCycleWeekDay = this.regInfoModel._SelectedCycleWeekDay;
                            this.showWeeklyDate = true;
                        }
                        this.selectedCategory = data._MCategory;
                        this.selectedMtype = data.mType;
                        this.selectedAreatype = data.mAreaType;
                        this.selectedCycle = data.settlementCycle;
                        this.regInfoModel._mcode = data._Mcode;
                        this.getDistrictByBank();
                        this.getBankBranchListByBankCodeAndDistCode();
                        this.selectedDivision = this.regInfoModel.locationCode.substring(0, 2);
                        this.fillDistrictDDLByDivision();
                        this.selectedDistrict = this.regInfoModel.locationCode.substring(0, 4);
                        this.fillThanaDDLByDistrict();
                        this.isLoading = false;
                    }

                },
                error => {
                    console.log(error);
                }
            )
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

    getPhotoIDTypeListForDDL(): any {
        this.distributorService.getPhotoIDTypeListForDDL()
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

    onStepAhead(event) {
        if (this.activeIndex < 3) {
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



    saveMerchant(event): any {
        this.showDuplicateMsg = false;
        if (!this.isEditMode) {
            this.regInfoModel.entryBy = this.currentUserModel.user.username;
        }

        if (this.isEditMode && !this.isRegistrationPermitted) {
            this.regInfoModel.updateBy = this.currentUserModel.user.username;
        }
        if (this.isEditMode && this.isRegistrationPermitted) {
            this.regInfoModel.authoBy = this.currentUserModel.user.username;
        }
        this.regInfoModel._mCategory = this.selectedCategory;
        this.regInfoModel.mType = this.selectedMtype;
        this.regInfoModel.mAreaType = this.selectedAreatype;
        this.regInfoModel.settlementCycle = this.selectedCycle;
        this.regInfoModel._SelectedCycleWeekDay = this.selectedCycleWeekDay;
        if (this.regInfoModel.mphone != "") {
            this.merchantService.save(this.regInfoModel, this.isEditMode, event).pipe(first())
                .subscribe(
                    data => {
                        window.history.back();
                        if (this.isEditMode) {
                            this.messageService.add({ severity: 'success', summary: 'Update successfully', detail: 'Merchant updated' });
                        }
                        else if
                            (this.isRegistrationPermitted && this.isEditMode) {
                            this.messageService.add({ severity: 'success', summary: 'Register successfully', detail: 'Merchant Registered' });
                        }
                        else {
                            this.messageService.add({ severity: 'success', summary: 'Save successfully', detail: 'Merchant added' });

                        }
                    },
                    error => {
                        console.log(error);
                    });

        }

    }

    validation(event) {

        switch (this.activeIndex) {
            case 0:
                {
                    if (!this.regInfoModel.mphone ||
                        !this.regInfoModel._mcode ||
                        (this.selectedCategory == '0') ||
                        (this.selectedMtype == '0') ||
                        (this.selectedAreatype == '0') ||
                        (!this.selectedCycle)
                    ) {
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
                        (this.selectedCategory == '0') ||
                        (this.selectedMtype == '0') ||
                        (this.selectedAreatype == '0') ||
                        (!this.selectedCycle) ||
                        !this.regInfoModel.mphone ||
                        !this.regInfoModel.companyName ||
                        !this.regInfoModel.name ||
                        !this.regInfoModel.conMob ||
                        !this.regInfoModel.offAddr ||
                        !this.regInfoModel.photoIdTypeCode ||
                        !this.regInfoModel.photoId ||
                        this.selectedDivision == '0' ||
                        this.selectedDistrict == '0' ||
                        !this.regInfoModel.locationCode ||
                        !this.regInfoModel.mEmployeeId
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
                        (this.selectedCategory == '0') ||
                        (this.selectedMtype == '0') ||
                        (this.selectedAreatype == '0') ||
                        !this.regInfoModel.mphone ||
                        !this.regInfoModel.companyName ||
                        !this.regInfoModel.name ||
                        !this.regInfoModel.conMob ||
                        !this.regInfoModel.offAddr ||
                        !this.regInfoModel.photoIdTypeCode ||
                        !this.regInfoModel.photoId ||
                        !this.selectedDivision ||
                        !this.selectedDistrict ||
                        !this.regInfoModel.locationCode ||
                        !this.regInfoModel.mEmployeeId
                    ) {
                        this.msgs = [];
                        this.msgs.push({ severity: 'error', summary: 'Warning! ', detail: 'Cannot be left blank' });
                        this.error = true;
                        break;
                    } else {
                        this.saveMerchant(event);
                        this.error = false;
                        break;
                    }
                }

        }

    }

    cancel() {
        window.history.back();
    }



    async getDivisionListForDDL() {
        this.distributorService.getDivisionList()
            .pipe(first())
            .subscribe(
                data => {
                    this.divisionList = data;
                    this.divisionList.unshift({ label: 'Please Select', value: null });
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

    checkMphoneAlreadyExist(): any {
        if (this.regInfoModel.mphone.toString().substring(0, 2) == "01" && this.regInfoModel.mphone.toString().substring(0, 3) != "012") {
            this.merchantService.checkMphoneAlreadyExist(this.regInfoModel.mphone)
                .pipe(first())
                .subscribe(
                    data => {
                        if (data) {
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


    checkPhotoIdLength() {
        this.formValidation.photoId = this.mfsUtilityService.checkPhotoIdLength(this.regInfoModel.photoId, this.regInfoModel.photoIdTypeCode);
        if (!this.formValidation.photoId) {
            //this.checkNidValid(this.regInfoModel.photoId);
        }
    };

    checkNidValid(value: any): any {
        this.kycService.checkNidValid(value, 'M')
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

    onDateListChange(e) {
        console.log(e);
        console.log(this.selectSettlementDate);
    }


}
