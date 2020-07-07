import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { AuthenticationService } from '../_services';
import { MessageService } from 'primeng/api';
import { Router, ActivatedRoute } from '@angular/router';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

    constructor(private authenticationService: AuthenticationService, private messageService: MessageService,
        private router: Router, private route: ActivatedRoute) { }

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        return next.handle(request).pipe(catchError(err => {
            var obj = { details: err, time: Date.now() };

            var existingError = JSON.parse(localStorage.getItem('errorList'));

            if (existingError) {
                existingError.errorList.push(obj);
            }
            else {
                var list = [];
                list.push(obj);
                localStorage.setItem('errorList', JSON.stringify({ errorList: list }));
            }


            if (err.status === 401) {
                // auto logout if 401 response returned from api
                this.messageService.add({ severity: 'error', summary: 'Warning', detail: 'Unauthorized Access' });
                this.authenticationService.logout();
                this.router.navigateByUrl('./');
            }

            if (err.status === 403) {
                // auto logout if 401 response returned from api
                this.messageService.add({ severity: 'error', summary: 'Warning', detail: 'Forbidden Access' });
                this.authenticationService.logout();
                this.router.navigateByUrl('./');
            }

            if (err.status === 500 || err.status === 404) {
                // auto logout if 401 response returned from api
                this.router.navigateByUrl('error');
            }

            
            const error = err.error.message || err.statusText;
            return throwError(error);
        }))
    }
}