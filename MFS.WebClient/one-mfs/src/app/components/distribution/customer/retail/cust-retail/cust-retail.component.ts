import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { MerchantService, DistributorService } from 'src/app/services/distribution';
import { Router, ActivatedRoute } from '@angular/router';
import { MessageService, MenuItem, Message } from 'primeng/api';
import { AuthenticationService } from 'src/app/shared/_services';
import { first } from 'rxjs/operators';
import { MfsUtilityService } from 'src/app/services/mfs-utility.service';
import { SelectItem } from 'primeng/api';
import { KycService } from '../../../../../services/distribution/kyc.service';

@Component({
  selector: 'app-cust-retail',
  templateUrl: './cust-retail.component.html',
  styleUrls: ['./cust-retail.component.css']
})
export class CustRetailComponent implements OnInit {
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
    isSecuredViewPermitted: boolean = false;
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
    showWeeklyDate: boolean = false;
    positveNumber: RegExp;
    isLoading: boolean = false;
    disabledEdit: boolean = true;
    alphabetsWithSpace: any;
    dateOfBirth: any = {};
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
        this.alphabetsWithSpace = this.mfsUtilityService.getAlphabetsWithSpaceEegExp();
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
            { label: 'Personal Retail', value: 'R' }           
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
        this.checkForSecureView();
    }
    checkMphoneAlreadyExist(): any {
        if (this.regInfoModel.mphone.toString().substring(0, 2) == "01" && this.regInfoModel.mphone.toString().substring(0, 3) != "012") {
            this.merchantService.checkMphoneAlreadyExist(this.regInfoModel.mphone)
                .pipe(first())
                .subscribe(
                    data => {
                        if (data) {
                            this.msgs = [];
                            this.msgs.push({ severity: 'error', summary: 'Retail A/C No : ' + this.regInfoModel.mphone, detail: 'Already Exists!' });
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
        this.formValidation.photoId = this.mfsUtilityService.checkPhotoIdLength(this.regInfoModel.photoId, '1');
        if (this.formValidation.photoId) {  
            this.regInfoModel.photoId = '';
        }     
    };
    
    validateDatepicker(event) {
        if (this.dateOfBirth) {
            var validate = this.mfsUtilityService.validateDatePickerInput(this.dateOfBirth);

            if (!validate) {
                //this.messageService.add({ severity: 'error', summary: 'Invalid Date Formate', detail: 'Please Input Valid Date', closable: true });
                this.dateOfBirth = null;
            }
        }
    }
    getMerChantByMphone(): any {
        this.isLoading = true;
        this.merchantService.getMerChantByMphone(this.entityId)
            .pipe(first())
            .subscribe(
                data => {
                    if (data) {
                        this.regInfoModel = data;                      
                        this.selectedCategory = data._MCategory;
                        this.selectedMtype = data.mType;
                        this.selectedAreatype = data.mAreaType;
                        this.regInfoModel._mcode = data._Mcode;
                        if (data.dateOfBirth != null) {
                            this.dateOfBirth = this.mfsUtilityService.renderDateObject(data.dateOfBirth);
                        }
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
    saveRetail(event): any {
        this.showDuplicateMsg = false;       
        if (event === 'save') {
            this.regInfoModel.entryBy = this.currentUserModel.user.username;
        }       
        if (event === 'edit') {
            this.regInfoModel.updateBy = this.currentUserModel.user.username;          
        }
        if (event === 'register') {
            this.regInfoModel.authoBy = this.currentUserModel.user.username;
        }
        this.regInfoModel._mCategory = this.selectedCategory;
        this.regInfoModel.mType = this.selectedMtype;  
        this.regInfoModel.dateOfBirth = this.mfsUtilityService.renderDate(this.dateOfBirth);
        if (this.regInfoModel.mphone) {
            this.merchantService.saveRetail(this.regInfoModel, this.isEditMode, event).pipe(first())
                .subscribe(
                    data => {
                        if (data === 200) {
                            window.history.back();
                            if (this.isEditMode) {
                                this.messageService.add({ severity: 'success', summary: 'Update successfully', detail: 'Retail Customer updated' });
                            }
                            else if
                                (this.isRegistrationPermitted && this.isEditMode) {
                                this.messageService.add({ severity: 'success', summary: 'Register successfully', detail: 'Retail Customer Registered' });
                            }
                            else {
                                this.messageService.add({ severity: 'success', summary: 'Save successfully', detail: 'Retail Customer added' });

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

    validation(event) {

        switch (this.activeIndex) {
            case 0:
                {
                    if (!this.regInfoModel.mphone || !this.selectedCategory
                        
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
                        !this.regInfoModel.companyName ||
                        !this.regInfoModel.name ||
                        !this.regInfoModel.conMob ||
                        !this.regInfoModel.offAddr ||
                        !this.regInfoModel.photoId ||
                        this.selectedDivision == '0' ||
                        this.selectedDistrict == '0' ||
                        !this.regInfoModel.locationCode ||
                        !this.dateOfBirth.year ||
                        !this.regInfoModel.gender
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
                        !this.regInfoModel.companyName ||
                        !this.regInfoModel.name ||
                        !this.regInfoModel.conMob ||
                        !this.regInfoModel.offAddr ||                      
                        !this.regInfoModel.photoId ||
                        !this.selectedDivision ||
                        !this.selectedDistrict ||
                        !this.regInfoModel.locationCode
                    ) {
                        this.msgs = [];
                        this.msgs.push({ severity: 'error', summary: 'Warning! ', detail: 'Cannot be left blank' });
                        this.error = true;
                        break;
                    } else {
                        this.saveRetail(event);
                        this.error = false;
                        break;
                    }
                }

        }

    }

    cancel() {
        window.history.back();
    }

}
