import { Component, OnInit } from '@angular/core';
import { MerchantService, DistributorService } from 'src/app/services/distribution';
import { Router, ActivatedRoute } from '@angular/router';
import { MessageService, MenuItem, Message } from 'primeng/api';
import { AuthenticationService } from 'src/app/shared/_services';
import { first } from 'rxjs/operators';
import { MfsUtilityService } from 'src/app/services/mfs-utility.service';
import { SelectItem } from 'primeng/api';
@Component({
    selector: 'app-chain-merchant-addoredit',
    templateUrl: './chain-merchant-addoredit.component.html',
    styleUrls: ['./chain-merchant-addoredit.component.css']
})
export class ChainMerchantAddoreditComponent implements OnInit {
    regInfoModel: any = {};
    merchantConfig: any = {};
    activeIndex: number = 0;
    items: MenuItem[];
    selectedChainMerchant: string = "0";
    divisionList: any;
    selectedDivision: string = "0";
    districtList: any;
    selectedDistrict: string = "0";
    thanaList: any;
    photoIDTypeList: any;
    chainMerchantList: any;
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
    selectedCycle: string = "0";
    showWeeklyDate: boolean = false;
    positveNumber: RegExp;
    parentCode: any;
    parentCompanyName: any;
    isLoading: boolean = false;
    serviceCharge: any;
    viewServiceCharge: boolean = false;
    constructor(private merchantService: MerchantService, private distributorService: DistributorService, private router: Router,
        private route: ActivatedRoute, private messageService: MessageService, private authService: AuthenticationService,
        private mfsUtilityService: MfsUtilityService) {
        this.authService.currentUser.subscribe(x => {
            this.currentUserModel = x;
        });
        this.formValidation = {};
        this.positveNumber = this.mfsUtilityService.getPositiveWholeNumberRegExp();
    }

    ngOnInit() {
        this.mAreaList = [
            { label: 'Urban', value: 'Urban' },
            { label: 'Rural', value: 'Rural' }
        ]
        this.getChainMerchantList();
        this.getPhotoIDTypeListForDDL();
        this.getDivisionListForDDL();

        this.entityId = this.route.snapshot.paramMap.get('id');
        if (this.entityId) {
            this.isEditMode = true;
            this.getChildMerChantByMphone();
            this.isRegistrationPermitted = this.authService.checkRegisterPermissionAccess(this.route.snapshot.routeConfig.path);
        }
    }
    getChildMerChantByMphone(): any {
        this.isLoading = true;
        this.merchantService.getChildMerChantByMphone(this.entityId)
            .pipe(first())
            .subscribe(
                data => {
                    if (data) {
                        this.regInfoModel = data;                       
                        this.parentCompanyName = this.regInfoModel.companyName;
                        this.selectedChainMerchant = data.pmphone;
                        this.parentCode = data._Mcode.substring(0, 12);
                        this.selectedAreatype = data.mAreaType;
                        this.selectedCycle = data.settlementCycle;
                        this.regInfoModel._OutletCode = data._Mcode;
                        this.selectedDivision = this.regInfoModel.locationCode.substring(0, 2);
                        this.fillDistrictDDLByDivision();
                        this.selectedDistrict = this.regInfoModel.locationCode.substring(0, 4);
                        this.fillThanaDDLByDistrict();
                        this.isLoading = false;
                    }

                },
                error => {
                    this.isLoading = false;
                    console.log(error);
                }
            )
    }
    checkMphoneAlreadyExist(): any {
        if (this.regInfoModel._ChildMphone.toString().substring(0, 2) == "01" && this.regInfoModel._ChildMphone.toString().substring(0, 3) != "012") {
            this.merchantService.checkMphoneAlreadyExist(this.regInfoModel._ChildMphone)
                .pipe(first())
                .subscribe(
                    data => {
                        if (data) {
                            this.msgs = [];
                            this.msgs.push({ severity: 'error', summary: 'Merchant A/C No : ' + this.regInfoModel._ChildMphone, detail: 'Already Exists!' });
                            this.regInfoModel._ChildMphone = '';
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
            this.regInfoModel._ChildMphone = '';
            this.messageService.add({ severity: 'error', summary: 'Invalid Mobile No', detail: 'Please Input Valid Mobiel No', closable: true });
        }
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
    getChainMerchantList() {
        //this.isLoading = true;
        this.merchantService.getChainMerchantList()
            .pipe(first())
            .subscribe(
                data => {
                    this.chainMerchantList = data;
                    //this.isLoading = false;
                },
                error => {
                    //this.isLoading = false;
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
    getParentMerchantByMphone() {
        this.merchantService.getParentMerchantByMphone(this.selectedChainMerchant)
            .pipe(first())
            .subscribe(
                data => {
                    if (data) {
                        this.regInfoModel = data;
                        this.parentCode = this.regInfoModel._Mcode;
                        this.parentCompanyName = this.regInfoModel.companyName;
                        this.regInfoModel.offAddr = '';
                        this.regInfoModel.companyName = '';
                        this.regInfoModel.locationCode = '';                       
                    }
                    else {
                        this.regInfoModel = {};
                    }

                },
                error => {
                    console.log(error);
                }
            );
    }

    validation(event) {

        switch (this.activeIndex) {
            case 0:
                {
                    if (!this.regInfoModel.mphone ||                     
                        !this.parentCode ||
                        !this.parentCompanyName ||
                        !this.regInfoModel.photoIdTypeCode||
                        !this.regInfoModel._OutletCode ||
                        (this.selectedAreatype == '0') ||
                        (this.selectedDistrict == '0') ||
                        (this.selectedDivision == '0') ||
                        !this.selectedChainMerchant||
                        !this.regInfoModel.locationCode ||
                        !this.regInfoModel._ChildMphone
                    ) {
                        this.msgs = [];                
                        this.messageService.add({ severity: 'error', summary: 'Warning!', detail: 'Cannot be left blank' });
                        this.error = true;
                        break;
                    }
                    else {
                        this.error = false;
                        this.saveMerchant(event);
                    }
                }
        }

    }


    saveMerchant(event): any {
        this.showDuplicateMsg = false;
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

        this.regInfoModel.mAreaType = this.selectedAreatype;
        this.regInfoModel.pmphone = this.selectedChainMerchant;
        //this.regInfoModel.schargePer = this.serviceCharge;
        if (this.regInfoModel._ChildMphone) {
            this.merchantService.saveChildMerchant(this.regInfoModel, this.isEditMode, event).pipe(first())
                .subscribe(
                    data => {
                        if (data === 200) {
                            window.history.back();
                            if (this.isEditMode) {
                                this.messageService.add({ severity: 'success', summary: 'Update successfully', detail: 'Merchant Updated' });

                            }
                            else if
                                (this.isRegistrationPermitted && this.isEditMode) {
                                this.messageService.add({ severity: 'success', summary: 'Register successfully', detail: 'Merchant Registered' });
                            }
                            else {
                                this.messageService.add({ severity: 'success', summary: 'Save successfully', detail: 'Merchant Added' });
                            }
                        }                      
                    },
                    error => {
                        console.log(error);
                    });

        }

    }
    checkPhotoIdLength() {
        this.formValidation.photoId = this.mfsUtilityService.checkPhotoIdLength(this.regInfoModel.photoId, this.regInfoModel.photoIdTypeCode)
    };
}
