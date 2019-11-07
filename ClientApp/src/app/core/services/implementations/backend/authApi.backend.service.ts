import { HttpClient } from '@angular/common/http';
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
        private http: HttpClient
    ) {
    }

    public authenticate(authenticateRequest: AuthenticateRequest): Observable<string> {
        return this.http
            .post(`${this.apiPrefix}/auth`, authenticateRequest)
            .pipe(catchError(error => of(error)));
    }
}

export { AuthApiBackendService };