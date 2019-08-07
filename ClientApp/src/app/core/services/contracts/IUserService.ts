import { Observable } from 'rxjs';

import { GetNotificationsResponse } from 'models/response/getNotificationsResponse';

abstract class IUserService {
    abstract getNotifications(): Observable<Array<GetNotificationsResponse>>;
}

export { IUserService };