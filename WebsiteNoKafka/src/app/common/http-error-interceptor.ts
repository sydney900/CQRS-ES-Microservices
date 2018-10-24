import { HttpInterceptor, HttpRequest, HttpHandler, HttpErrorResponse, HttpEvent } from '@angular/common/http';
import { Observable, throwError, observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import {Router} from '@angular/router';
import { NOT_FOUND, UNAUTHORIZED, BAD_REQUEST, FORBIDDEN, getStatusText } from 'http-status-codes';
import { LogService } from '../services/log.service';
import { NotificationService } from '../services/notification.service';

@Injectable({
    providedIn: 'root'
})
export class HttpErrorInterceptor implements HttpInterceptor {
    constructor(private log: LogService, private notification: NotificationService, private router: Router) {
    }

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        return next.handle(request)
            .pipe(
                catchError((error: HttpErrorResponse) => {
                    let errMsg = '';
                    // Client Side Error
                    if (error.error instanceof ErrorEvent) {
                        errMsg = `Error: ${error.error.message}`;
                    } else {
                        // Server Side Error
                        console.log(error);
                        const errorInfo = error.message;
                        this.notification.showError(errorInfo);
                        const statusCode = error.status;
                        switch (statusCode) {
                            case NOT_FOUND:
                                // return Observable.throw(error.message);
                                break;
                            case BAD_REQUEST:
                                // return Observable.throw(error.message);
                                break;
                                case UNAUTHORIZED:
                                this.router.navigateByUrl('/login');
                                break;
                            case FORBIDDEN:
                                this.router.navigateByUrl('/unauthorized');
                                break;
                        }

                        // errMsg = `Error Code: ${error.status},  Message: ${error.message}`;
                        // errMsg = error.message;
                        errMsg = getStatusText(statusCode);
                    }

                    return throwError(errMsg);
                })
            );
    }
}
