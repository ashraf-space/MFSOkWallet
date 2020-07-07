import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

import { AuthenticationService } from '../_services';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {
    currentUser: any = {};
    constructor(private authenticationService: AuthenticationService) {
        this.currentUser = this.authenticationService.currentUserValue;
    }

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        // add authorization header with jwt token if available        
        if (this.currentUser && this.currentUser.bearerToken) {
            request = request.clone({
                headers: new HttpHeaders({
                    Authorization: `Bearer ${this.currentUser.bearerToken}`,
                    UserInfo: this.currentUser.user.id + ',' + this.currentUser.user.roleId,
                    ApiKey: 'okwallet'
                })
            });
        }
        else {
            request = request.clone({
                headers: request.headers.set('ApiKey', 'okwallet')
            });
        }

        return next.handle(request);
    }
}