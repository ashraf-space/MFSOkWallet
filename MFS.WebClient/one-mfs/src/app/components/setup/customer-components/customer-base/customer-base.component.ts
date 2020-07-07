import { Component, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { first } from 'rxjs/operators';
import { Router, ActivatedRoute } from '@angular/router';
import { User } from '../../../../shared/_models';
import { UserService, AuthenticationService } from '../../../../shared/_services';

@Component({
  selector: 'app-customer-base',
  templateUrl: './customer-base.component.html',
  styleUrls: ['./customer-base.component.css']
})

export class CustomerBaseComponent implements OnInit {
    currentUser: User;
    currentUserSubscription: Subscription;
    returnUrl: any;

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private authenticationService: AuthenticationService,
        private userService: UserService
    ) {
        this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
        this.currentUserSubscription = this.authenticationService.currentUser.subscribe(user => {
            this.currentUser = user;
            if (!user) {
                this.router.navigate(['/login']);
            }
            else {
                this.router.navigate(this.returnUrl);
            }
            console.log(this.currentUser);
        });
    }

  ngOnInit() {
  }

}
