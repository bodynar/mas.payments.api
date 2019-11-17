import { Injectable } from '@angular/core';

import { Observable, of } from 'rxjs';
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

    public isAuthenticated(): Observable<boolean> {
        const authToken: string =
            this.getAuthToken();

        if (isNullOrUndefined(authToken) || authToken === '') {
            return of(false);
        }

        return this.authApiBackend
            .isAuthenticated(authToken)
            .pipe(
                tap(isTokenValid => {
                    if (!isTokenValid) {
                        this.removeAuthToken();
                    }
                }),
            );
    }

    public logout(): Observable<boolean> {
        const authToken: string =
            this.getAuthToken();

        return this.authApiBackend
            .logout(authToken)
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
        let token: string =
            localStorage.getItem('auth-token');

        if (token === '[object Object]') {
            this.removeAuthToken();
            // this.loggingService.errro()'
            token = '';
        }

        return token;
    }

    public removeAuthToken(): void {
        this.removeAuthToken();
    }

    private removeToken(): void {
        localStorage.removeItem('auth-token');
    }
}

export { AuthService };