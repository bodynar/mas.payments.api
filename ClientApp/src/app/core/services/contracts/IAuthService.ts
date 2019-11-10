import { Observable } from 'rxjs';

import { AuthenticateRequest } from 'models/request/authenticateRequest';

abstract class IAuthService {
    abstract authenticate(authenticateRequest: AuthenticateRequest): Observable<string>;

    abstract logout(): Observable<boolean>;

    abstract isAuthenticated(): Observable<boolean>;

    abstract setAuthToken(token: string): void;

    abstract getAuthToken(): string;
}

export { IAuthService };