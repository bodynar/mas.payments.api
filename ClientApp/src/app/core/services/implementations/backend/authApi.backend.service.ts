import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { AuthenticateRequest } from 'models/request/authenticateRequest';

import { IAuthApiBackendService } from 'services/backend/IAuthApi.backend';

@Injectable()
class AuthApiBackendService implements IAuthApiBackendService {

    private readonly apiPrefix: string =
        '/api/auth';

    constructor(
        private http: HttpClient,
    ) {
    }

    public authenticate(authenticateRequest: AuthenticateRequest): Observable<string> {
        return this.http
            .post(`${this.apiPrefix}/authenticate`, authenticateRequest)
            .pipe(catchError(error => of(error)));
    }

    public isAuthenticated(token: string): Observable<boolean> {
        return this.http
            .get(`${this.apiPrefix}/isTokenValid`, {
                params: new HttpParams({
                    fromObject: { token: `${token}` }
                })
            })
            .pipe(catchError(error => of(error)));
    }

    public logout(authToken: string): Observable<any> {
        return this.http
            .post(`${this.apiPrefix}/logout`, { token: authToken })
            .pipe(catchError(error => of(error)));
    }
}

export { AuthApiBackendService };