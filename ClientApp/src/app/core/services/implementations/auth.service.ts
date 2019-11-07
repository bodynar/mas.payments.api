import { Injectable } from '@angular/core';

import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { isNullOrUndefined } from 'util';

import { AuthenticateRequest } from 'models/request/authenticateRequest';

import { IAuthApiBackendService } from 'services/backend/IAuthApi.backend';
import { IAuthService } from 'services/IAuthService';

@Injectable()
class AuthService implements IAuthService {

    constructor(
        private authApiBackend: IAuthApiBackendService
    ) {
    }

    public authenticate(authenticateRequest: AuthenticateRequest): Observable<string> {
        return this.authApiBackend
            .authenticate(authenticateRequest)
            .pipe(
                map(response => {
                    const hasError: boolean =
                        isNullOrUndefined(response) || response === '';

                    if (hasError) {
                        // this.loggingService.error(response);
                    }

                    return hasError ? '' : response;
                }));
    }

    public setAuthToken(token: string): void {
        localStorage.setItem('auth-token', token);
    }

    public getAuthToken(): string {
        return localStorage.getItem('auth-token');
    }
}

export { AuthService };