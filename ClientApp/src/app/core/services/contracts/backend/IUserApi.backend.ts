import { Observable } from 'rxjs';

import { GetNotificationsResponse } from 'models/response/getNotificationsResponse';

abstract class IUserApiBackendService {
    abstract getNotifications(): Observable<Array<GetNotificationsResponse>>;
}

export { IUserApiBackendService };