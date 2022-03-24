import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ApplicationUserService, RoleService } from '../../../../services/security';
import { BankBranchService } from '../../../../services/environment';
import { first } from 'rxjs/operators';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthenticationService } from '../../../../shared/_services';
import { ConfirmationService, Message, MessageService } from 'primeng/api';

@Component({
  selector: 'app-application-user-add-edit',
  templateUrl: './application-user-add-edit.component.html',
    styleUrls: ['./application-user-add-edit.component.css'],
    providers: [ConfirmationService]
})
export class ApplicationUserAddEditComponent implements OnInit {

    error: boolean = false;
    msgs: any = [];
    entityId: number;
    isEditMode: boolean = false;

    applicationUserModel: any = {};
    bankBranchList: any = [];
    securedRoleList: any = [];
    IsCurrentUser: boolean = false;
    currentUserModel: any = {};
    initiateModal: boolean = false;
    changePasswordModel: any = {};
    invalidCredentials: boolean = false;

    hasEditPermission: boolean = false;
    optionList: any;
    isExistingUsername: boolean = false;
    isExistingEmailId: boolean = false;
    isExistingMobileNo: boolean = false;
    isExistingEmployeeId: boolean = false;
    blockSpace: RegExp = /[^\s]/; 
    logInStatusList: any;
    passwordChangedBy: string;
    roleName: any;
    constructor(private applicationUserService: ApplicationUserService, private router: Router,
        private route: ActivatedRoute, private bankBranchService: BankBranchService, private roleService: RoleService,
        private authenticationService: AuthenticationService, private messageService: MessageService, private confirmationService: ConfirmationService) {
        this.authenticationService.currentUser.subscribe(x => {
            this.currentUserModel = x;
        });
        this.optionList = [
            { label: 'Regular', value: 'Y', icon: 'fas fa-unlock-alt' },
            { label: 'Not Verified', value: 'N', icon: 'far fa-clock' },
            { label: 'Locked', value: 'L', icon: 'fas fa-lock' }];
        this.logInStatusList = [
            { label: 'Log Out', value: 'N' },
            { label: 'Log In', value: 'Y' }
        ]
    }

    ngOnInit() {
        this.getBankBranchList();
        this.SecuredRoleList();
        this.entityId = +this.route.snapshot.paramMap.get('id');
        if (this.entityId) {
            this.isEditMode = true;
            this.getApplicationUserById();

            if (this.entityId == this.currentUserModel.user.id) {
                this.IsCurrentUser = true;
            }
        }

        this.hasEditPermission = this.authenticationService.checkEditPermissionAccess(this.route.snapshot.routeConfig.path);
    }

    async SecuredRoleList() {
        this.roleName = this.currentUserModel.user.role_Name;
        this.roleService.GetDropdownListByRoleName(this.roleName).pipe(first())
            .subscribe(
                data => {
                    this.securedRoleList = data;
                },
                error => {
                    console.log(error);
                });

    }

    async getBankBranchList() {
        this.bankBranchService.getBankBranchListForDDL().pipe(first())
            .subscribe(
                data => {
                    this.bankBranchList = data;
                },
                error => {
                    console.log(error);
                });

    }

    getApplicationUserById(): any {
        this.applicationUserService.getApplicationUserById(this.entityId).pipe(first())
            .subscribe(
                data => {
                    this.applicationUserModel = data;
                },
                error => {
                    console.log(error);
                });
    };


    onUserSave() {
        if (!this.applicationUserModel.username || this.applicationUserModel.username == '' || 
            !this.applicationUserModel.mobileNo || this.applicationUserModel.mobileNo == '' ||
            !this.applicationUserModel.name || this.applicationUserModel.name == '' ||
            !this.applicationUserModel.branchCode || this.applicationUserModel.branchCode == '' ||
            !this.applicationUserModel.roleId || this.applicationUserModel.roleId == '' ||
            //this.isExistingUsername || this.isExistingEmailId || this.isExistingMobileNo || this.isExistingEmployeeId) {
            this.isExistingUsername ||  this.isExistingMobileNo ) {
            this.error = true;
            this.msgs = [];
            this.msgs.push({ severity: 'error', summary: 'Warning! ', detail: 'Cannot be left blank' });
        }
        else {
            this.error = false;
            this.msgs = [];
            if (this.isEditMode) {
                this.applicationUserModel.updatedBy = this.currentUserModel.user.username;
            }
            else {
                this.applicationUserModel.createdBy = this.currentUserModel.user.username;
            }
   
            this.applicationUserService.save(this.applicationUserModel).pipe(first())
                .subscribe(
                data => {
                    this.messageService.add({ severity: 'success', summary: 'Success', detail: this.applicationUserModel.name + ' saved Successfully' });
                        //console.log(data);
                        this.goBack();
                    },
                    error => {
                        console.log(error);
                    });

        }
    }

    onDeleteUser(event) {
        if (this.applicationUserModel && this.applicationUserModel.id) {
            this.applicationUserService.delete(this.applicationUserModel).pipe(first())
                .subscribe(
                    data => {
                        console.log(data);
                        this.router.navigateByUrl('../feature-category/worklist');
                    },
                    error => {
                        console.log(error);
                    });
        }
    }

    onChangePassword() {
        this.initiateModal = true;
    }

    confirmPasswordChange() {
        this.changePasswordModel.ApplicationUserId = this.applicationUserModel.id;
        this.passwordChangedBy = this.currentUserModel.user.username;
        this.applicationUserService.changePassword(this.changePasswordModel, this.passwordChangedBy).pipe(first())
            .subscribe(
            data => {
                if (data == 'Old Password is Invalid') {
                    this.invalidCredentials = true;
                }
                else {
                    this.messageService.add({ severity: 'success', summary: 'Success', detail: this.applicationUserModel.name + ' password changed Successfully' });
                    this.initiateModal = false;
                }                    
                },
                error => {
                    this.messageService.add({ severity: 'error', summary: 'Warning', detail: this.applicationUserModel.name + ' password change unsuccessful' });                     
                });
        
    }

    goBack() {
        window.history.back();
    }
    
    onResetPassword() {        
        this.confirmationService.confirm({
            message: 'Warning! Are you sure that you want to reset password for ' + this.applicationUserModel.name + ' ?' ,
            accept: () => {
                this.confirmResetPassword();
            }
        });
    }

    confirmResetPassword() {
        this.applicationUserModel.updatedBy = this.currentUserModel.user.username;
        this.applicationUserService.resetPassword(this.applicationUserModel).pipe(first())
            .subscribe(
            data => {
                this.applicationUserModel = data;
                this.messageService.add({ severity: 'success', summary: 'Success', detail: this.applicationUserModel.name + ' password reset Successfully' });                
                },
                error => {
                    this.messageService.add({ severity: 'error', summary: 'Warning', detail: this.applicationUserModel.name + ' password reset unsuccessful' });
                });
    }

    changePstatus(event) {
        this.applicationUserService.changePasswordStatus(this.applicationUserModel).pipe(first())
            .subscribe(
                data => {
                    this.messageService.add({ severity: 'success', summary: 'Success', detail: this.applicationUserModel.name + ' password status Successfully' });
                },
                error => {
                    this.messageService.add({ severity: 'error', summary: 'Warning', detail: this.applicationUserModel.name + ' password reset unsuccessful' });
                });
    }

    checkExistingUserName() {
        if (this.applicationUserModel.username != null || this.applicationUserModel.username != '') {
            this.applicationUserService.checkExistingUserName(this.applicationUserModel).pipe(first())
                .subscribe(
                data => {
                    if (data == 'data exist') {
                        this.messageService.add({ severity: 'error', summary: 'Warning', detail: ' Duplicate User Name' });
                        this.isExistingUsername = true;
                    }
                    else {
                        this.isExistingUsername = false;
                    }
                    },
                    error => {
                        
                    });
        }
    }

    checkExistingEmailId() {
        if (this.applicationUserModel.emailId != null || this.applicationUserModel.emailId != '') {
            this.applicationUserService.checkExistingEmailId(this.applicationUserModel.emailId).pipe(first())
                .subscribe(
                    data => {
                        if (data == 'data exist') {
                            this.messageService.add({ severity: 'error', summary: 'Warning', detail: ' Duplicate Email Id' });
                            this.isExistingEmailId = true;
                        }
                        else {
                            this.isExistingEmailId = false;
                        }
                    },
                    error => {

                    });
        }
    }

    checkExistingMobileNo() {
        if (this.applicationUserModel.mobileNo != null || this.applicationUserModel.mobileNo != '') {
            this.applicationUserService.checkExistingMobileNo(this.applicationUserModel.mobileNo).pipe(first())
                .subscribe(
                    data => {
                        if (data == 'data exist') {
                            this.messageService.add({ severity: 'error', summary: 'Warning', detail: ' Duplicate Mobile Number' });
                            this.isExistingMobileNo = true;
                        }
                        else {
                            this.isExistingMobileNo = false;
                        }
                    },
                    error => {

                    });
        }
    }

    checkExistingEmployeeId() {
        if (this.applicationUserModel.employeeId != null || this.applicationUserModel.employeeId != '') {
            this.applicationUserService.checkExistingEmployeeId(this.applicationUserModel.employeeId).pipe(first())
                .subscribe(
                    data => {
                        if (data == 'data exist') {
                            this.messageService.add({ severity: 'error', summary: 'Warning', detail: ' Duplicate Employee Id' });
                            this.isExistingEmployeeId = true;
                        }
                        else {
                            this.isExistingEmployeeId = false;
                        }
                    },
                    error => {

                    });
        }
    }


}
