import { Observable } from 'rxjs';

import TestMailMessageRequest from 'models/request/user/testMailMessageRequest';
import UpdateUserSettingRequest from 'models/request/user/updateUserSettingRequest';
import GetNotificationsResponse from 'models/response/user/getNotificationsResponse';
import GetUserSettingsResponse from 'models/response/user/getUserSettingsResponse';

abstract class IUserService {
    abstract getNotifications(): Observable<Array<GetNotificationsResponse>>;

    abstract sendTestMailMessage(testMailMessage: TestMailMessageRequest): Observable<boolean>;

    abstract getUserSettings(): Observable<Array<GetUserSettingsResponse>>;

    abstract updateUserSettings(updatedSettings: Array<UpdateUserSettingRequest>): Observable<boolean>;
}

export { IUserService };