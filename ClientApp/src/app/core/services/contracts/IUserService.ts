import { Observable } from 'rxjs';

import { TestMailMessageRequest } from 'models/request/testMailMessageRequest';
import { GetNotificationsResponse } from 'models/response/getNotificationsResponse';

abstract class IUserService {
    abstract getNotifications(): Observable<Array<GetNotificationsResponse>>;

    abstract sendTestMailMessage(testMailMessage: TestMailMessageRequest): Observable<boolean>;
}

export { IUserService };