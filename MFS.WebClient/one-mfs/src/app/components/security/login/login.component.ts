import { AfterViewInit, Component, ElementRef, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { first } from 'rxjs/operators';
import { AlertService, AuthenticationService, EmailService } from '../../../shared/_services';
import { MessageService } from 'primeng/api';

@Component({ templateUrl: 'login.component.html', styleUrls: ['login.component.css'] })

export class LoginComponent implements OnInit {
    loginForm: FormGroup;
    loading = false;
    submitted = false;
    returnUrl: string;
    loginModel: any;
    invalidCredentials: boolean = false;

    failedAttemptsCount: number = 0;
    fixedUserName: boolean = false;
    lockDownState: boolean = false;
    forceLocklockDownState: boolean = false;
    promptForgotPasswordModal: boolean = false;
    forgotPassResetModel: any = {};
    isDisableSendVC: boolean = true;
    verificationMatched: boolean = false;
    emailSendMsg: string;
    matchedMsg: string;
    showConfirmMsg: any;
    veriCodeAsMd5Password: any;

    isDisableUserName: boolean = false;
    isDisableEmployeeId: boolean = true;
    isDisableMobileNo: boolean = true;
    isDisableEmail: boolean = true;

    constructor(
        private formBuilder: FormBuilder,
        private route: ActivatedRoute,
        private router: Router,
        private authenticationService: AuthenticationService,
        private elementRef: ElementRef,
        private emailService: EmailService,
        private messageService: MessageService
    ) {
        if (this.authenticationService.currentUserValue) {
            this.router.navigate(['/']);
        }

    }

    ngOnInit() {
        this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
        this.loginModel = {};
    }

    onSignIn() {
        this.loading = true;
        this.authenticationService.login(this.loginModel)
            .pipe(first())
            .subscribe(
                data => {
                    if (data.isAuthenticated) {
                        location.reload();
                    }
                    else {
                        this.loading = false;
                        if (data.user.pstatus == 'L') {
                            this.loginModel.fullName = data.user.name;
                            this.lockDownState = true;
                            this.invalidCredentials = false;
                        }
                        else if (data.user.logInStatus == 'N') {
                            this.loginModel.fullName = data.user.name;
                            this.forceLocklockDownState = true;
                            this.invalidCredentials = false;
                        }
                        else {
                            this.invalidCredentials = true;
                            this.proceedToLockout(data);
                        }
                    }

                },
                error => {
                    this.loading = false;
                    console.log(error);
                    this.invalidCredentials = true;
                });
    }

    proceedToLockout(data: any): any {
        if (data.user.username != null) {
            this.fixedUserName = true;
            this.loginModel.UserName = data.user.username;
            this.loginModel.fullName = data.user.name;
            this.failedAttemptsCount++;
        }

        if (this.failedAttemptsCount >= 3) {
            this.authenticationService.lockAccount(data.user).pipe(first())
                .subscribe(
                    resp => {
                        this.lockDownState = true;
                        this.invalidCredentials = false;
                    },
                    error => {
                        console.log(error);

                    });
        }
    }

    onFinishEvent(event) {
        if (event.keyCode == 13) {
            this.onSignIn();
        }
    }

    ForgotPassword() {
        this.isDisableUserName = false;
        this.isDisableEmployeeId = true;
        this.isDisableMobileNo = true;
        this.isDisableEmail = true;
        this.isDisableSendVC = true;

        this.emailSendMsg = null;
        this.forgotPassResetModel = {};
        this.promptForgotPasswordModal = true;
    }

    EnableSendVeriCode() {
        this.emailSendMsg = null;
        if (this.forgotPassResetModel.userName && this.forgotPassResetModel.employeeId
            && this.forgotPassResetModel.mobileNo && this.forgotPassResetModel.officialEmail) {
            this.isDisableSendVC = false;
        }
        else {
            this.isDisableSendVC = true;

        }
    }
    EnableDisableFields(field) {
        this.emailSendMsg = null;
        this.emailService.ifExistsField(field, this.forgotPassResetModel.userName, this.forgotPassResetModel.employeeId, this.forgotPassResetModel.mobileNo)
            .pipe(first())
            .subscribe(
                data => {
                    if (field == 'userName') {
                        if (data == true) {
                            this.isDisableUserName = true;
                            this.isDisableEmployeeId = false;
                        }
                        else {
                            this.emailSendMsg = "Please enter correct user name.";
                        }
                    }
                    else if (field == 'employeeId') {
                        if (data == true) {
                            this.isDisableUserName = true;
                            this.isDisableEmployeeId = true;
                            this.isDisableMobileNo = false;

                        }
                        else {
                            this.emailSendMsg = "Please enter correct Employee Id.";
                        }
                    }
                    else {
                        if (data == true) {
                            this.isDisableUserName = true;
                            this.isDisableEmployeeId = true;
                            this.isDisableMobileNo = true;
                            this.isDisableEmail = false;
                        }
                        else {
                            this.emailSendMsg = "Please enter correct Mobile No.";
                        }
                    }


                },
                error => {
                    console.log(error);
                }
            );
    }

    sendVeriCodeToEmail() {
        //this.isLoading = true;
        if (this.forgotPassResetModel.officialEmail) {
            //this.emailService.sendVeriCodeToEmail(this.forgotPassResetModel.officialEmail)
            this.emailService.sendVeriCodeToEmailAfterChecking(this.forgotPassResetModel)
                .pipe(first())
                .subscribe(
                    data => {
                        if (data) {
                            //this.isLoading = false;
                            this.veriCodeAsMd5Password = data;
                            //this.messageService.add({ severity: 'success', summary: 'Mail send successfully', sticky: true, detail: 'Verification code sent to your email!' });
                            this.emailSendMsg = "Please check your email. A Verification code is sent to your email.";
                        }
                        else {
                            this.emailSendMsg = "Please enter correct user name, employeeId, mobile No and email.";
                        }

                    },
                    error => {
                        console.log(error);
                    }
                );
        }
    }

    checkVerificationCode() {
        //this.isLoading = true;
        if (this.forgotPassResetModel.verificationCode) {
            this.emailService.getMd5Password(this.forgotPassResetModel.verificationCode.trim())
                .pipe(first())
                .subscribe(
                    data => {
                        //this.isLoading = false;
                        if (this.veriCodeAsMd5Password == data) {
                            this.verificationMatched = true;
                            this.matchedMsg = "Verification code match!";
                        }
                        else {
                            this.verificationMatched = false;
                            //this.messageService.add({ severity: 'error', summary: 'Warning', detail: 'Verification code not match!' });
                            this.matchedMsg = "Verification code not match!";
                        }



                    },
                    error => {
                        console.log(error);
                    }
                );
        }
    }

    confirmResetPassword() {
        this.emailService.resetPassword(this.forgotPassResetModel).pipe(first())
            .subscribe(
                data => {
                    this.messageService.add({ severity: 'success', summary: 'Success', detail: this.forgotPassResetModel.userName + ' password reset Successfully and send to your email.' });
                    //this.showConfirmMsg = this.forgotPassResetModel.userName + ' password reset Successfully and send to your email.';
                    this.promptForgotPasswordModal = false;
                },
                error => {
                    this.messageService.add({ severity: 'error', summary: 'Warning', detail: this.forgotPassResetModel.userName + ' password reset unsuccessful' });
                });
    }

}
