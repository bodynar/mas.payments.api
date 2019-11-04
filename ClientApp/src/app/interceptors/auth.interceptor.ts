import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { Observable } from 'rxjs';

import { IAuthService } from 'services/IAuthService';
import { isNullOrUndefined } from 'util';

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
            );
        }
        else {
            return next.handle(req);
        }
    }
}

export { AuthHeaderInterceptor };