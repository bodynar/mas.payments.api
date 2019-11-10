import { Injectable } from '@angular/core';

import { Observable } from 'rxjs';
import { map, tap } from 'rxjs/operators';

import { isNullOrUndefined } from 'util';

import { AuthenticateRequest } from 'models/request/authenticateRequest';

import { IAuthApiBackendService } from 'services/backend/IAuthApi.backend';
import { IAuthService } from 'services/IAuthService';
import { IHasherService } from 'services/IHasherService';

@Injectable()
class AuthService implements IAuthService {

    constructor(
        private authApiBackend: IAuthApiBackendService,
        private hashService: IHasherService,
        // private loggingService: ILoggingService
    ) {
    }

    public authenticate(authenticateRequest: AuthenticateRequest): Observable<string> {
        const request: AuthenticateRequest =
        {
            login: authenticateRequest.login,
            passwordHash: this.hashService.generateHash(authenticateRequest.password)
        };

        return this.authApiBackend
            .authenticate(request)
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

    public logout(): Observable<boolean> {
        const authToken: string =
            this.getAuthToken();

        return this.authApiBackend
            .logOff(authToken)
            .pipe(
                map(response => isNullOrUndefined(response)),
                tap(withoutError => {
                    if (!withoutError) {
                        // this.loggingService.error()
                    }
                    else {
                        this.removeAuthToken();
                    }
                }),
            );
    }

    public setAuthToken(token: string): void {
        localStorage.setItem('auth-token', token);
    }

    public getAuthToken(): string {
        return localStorage.getItem('auth-token');
    }

    private removeAuthToken(): void {
        localStorage.removeItem('auth-token');
    }
}

export { AuthService };