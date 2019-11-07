import { Observable } from 'rxjs';

import { AuthenticateRequest } from 'models/request/authenticateRequest';

abstract class IAuthApiBackendService {
    abstract authenticate(authenticateRequest: AuthenticateRequest): Observable<string>;

    abstract logOff(authToken: string): Observable<boolean>;
}

export { IAuthApiBackendService };