import { Observable } from 'rxjs';

import { AuthenticateRequest } from 'models/request/authenticateRequest';

abstract class IAuthApiBackendService {
    abstract authenticate(authenticateRequest: AuthenticateRequest): Observable<string>;

    abstract isAuthenticated(token: string): Observable<boolean>;

    abstract logout(authToken: string): Observable<boolean>;
}

export { IAuthApiBackendService };