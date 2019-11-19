import { Observable } from 'rxjs';

import { ForgotPasswordRequest } from 'models/request/forgotPasswordRequest';
import { TestMailMessageRequest } from 'models/request/testMailMessageRequest';
import { UserRegisterRequest } from 'models/request/userRegisterRequest';
import { GetNotificationsResponse } from 'models/response/getNotificationsResponse';

abstract class IUserApiBackendService {
    abstract getNotifications(): Observable<Array<GetNotificationsResponse>>;

    abstract sendTestMailMessage(testMailMessage: TestMailMessageRequest): Observable<any>;

    abstract register(userInformation: UserRegisterRequest): Observable<any>;

    abstract confirmRegistration(token: string): Observable<any>;

    abstract forgotPassword(forgotPasswordRequest: ForgotPasswordRequest): Observable<any>;
}

export { IUserApiBackendService };