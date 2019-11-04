import { Injectable } from '@angular/core';

import { IAuthService } from 'services/IAuthService';

@Injectable()
class AuthService implements IAuthService {
    public setAuthToken(token: string): void {
        localStorage.setItem('auth-token', token);
    }

    public getAuthToken(): string {
        return localStorage.getItem('auth-token');
    }
}

export { AuthService };