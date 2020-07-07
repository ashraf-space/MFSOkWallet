import { AfterViewInit, Component, ElementRef, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { first } from 'rxjs/operators';
import { AlertService, AuthenticationService } from '../../../shared/_services';

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
    constructor(
        private formBuilder: FormBuilder,
        private route: ActivatedRoute,
        private router: Router,
        private authenticationService: AuthenticationService,
        private elementRef: ElementRef
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
    
}
