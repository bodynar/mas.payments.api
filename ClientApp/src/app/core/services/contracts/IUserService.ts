import { Observable } from 'rxjs';

import { TestMailMessageRequest } from 'models/request/testMailMessageRequest';
import { UserLoginRequest } from 'models/request/userLoginRequest';
import { UserRegisterRequest } from 'models/request/userRegisterRequest';
import { GetNotificationsResponse } from 'models/response/getNotificationsResponse';

abstract class IUserService {
    abstract getNotifications(): Observable<Array<GetNotificationsResponse>>;

    abstract sendTestMailMessage(testMailMessage: TestMailMessageRequest): Observable<boolean>;

    abstract register(userInformation: UserRegisterRequest): Observable<boolean>;

    abstract confirmRegistration(token: string): Observable<boolean>;

    abstract login(loginInformation: UserLoginRequest): Observable<string>;

    abstract setAuthToken(token: string): void;
}

export { IUserService };