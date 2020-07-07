import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';

import { AuthenticationService } from '../_services';

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {
    constructor(
        private router: Router,
        private authenticationService: AuthenticationService
    ) {}

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {        
        const currentUser = this.authenticationService.currentUserValue;
        var isAuthorized = false;
        if (currentUser) {
            if (currentUser.featureList) {
                if (route.routeConfig.path == 'home' || route.routeConfig.path == 'error') {
                    return true;
                }
                else if (currentUser.featureList.length != 0) {
                    currentUser.featureList.forEach(obj => {
                        if (obj.FEATURELINK == route.routeConfig.path) {
                            //console.log(route.routeConfig.path + '  ' + obj.FEATURELINK);
                            isAuthorized = true;
                        }
                        else {
                            var targetRoutePath = route.routeConfig.path.split('/');
                            var authorizedRoutePath = obj.FEATURELINK.split('/');

                            if (targetRoutePath[0] == authorizedRoutePath[0]) {
                                if (targetRoutePath.length > 3) {
                                    if (targetRoutePath[1] == authorizedRoutePath[1]) {
                                        isAuthorized = true;
                                    }
                                }
                                else {
                                    isAuthorized = true;
                                }
                            }
                        }
                    });
                    
                    if (isAuthorized) {
                        return true;
                    }
                    else {
                        this.router.navigate(['/home']);
                        return false;
                    }
                }
            }
        }

        // not logged in so redirect to login page with the return url
        this.authenticationService.logout();
        this.router.navigate(['/login'], { queryParams: { returnUrl: state.url }});
        return false;
    }
}