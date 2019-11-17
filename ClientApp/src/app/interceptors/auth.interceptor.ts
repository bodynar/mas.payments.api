import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

import { isNullOrUndefined } from 'util';

import { IAuthService } from 'services/IAuthService';

@Injectable()
class AuthHeaderInterceptor implements HttpInterceptor {

    constructor(
        private authService: IAuthService
    ) {
    }

    public intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        const token: string =
            this.authService.getAuthToken();

        if (!isNullOrUndefined(token)) {
            return next.handle(
                req.clone({
                    setHeaders: {
                        'auth-token': token
                    }
                })
            ).pipe(
                map(event => event as HttpEvent<any>),
                catchError((error: HttpErrorResponse) => {

                    if (error.status === 401) {
                        this.authService.removeAuthToken();
                        console.warn('token removed');
                    }

                    return throwError(error);
                })
            );
        }
        else {
            return next.handle(req);
        }
    }
}

export { AuthHeaderInterceptor };