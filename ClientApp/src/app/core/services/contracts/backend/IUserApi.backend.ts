import { Observable } from 'rxjs';

import { TestMailMessageRequest } from 'models/request/testMailMessageRequest';
import { UserLoginRequest } from 'models/request/userLoginRequest';
import { UserRegisterRequest } from 'models/request/userRegisterRequest';
import { GetNotificationsResponse } from 'models/response/getNotificationsResponse';

abstract class IUserApiBackendService {
    abstract getNotifications(): Observable<Array<GetNotificationsResponse>>;

    abstract sendTestMailMessage(testMailMessage: TestMailMessageRequest): Observable<any>;

    abstract register(userInformation: UserRegisterRequest): Observable<any>;

    abstract confirmRegistration(token: string): Observable<any>;

    abstract login(loginInformation: UserLoginRequest): Observable<string>;
}

export { IUserApiBackendService };