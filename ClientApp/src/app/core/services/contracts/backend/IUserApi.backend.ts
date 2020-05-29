import { Observable } from 'rxjs';

import TestMailMessageRequest from 'models/request/user/testMailMessageRequest';
import UpdateUserSettingRequest from 'models/request/user/updateUserSettingRequest';
import CommandExecutionResult from 'models/response/commandExecutionResult';
import GetNotificationsResponse from 'models/response/user/getNotificationsResponse';
import GetUserSettingsResponse from 'models/response/user/getUserSettingsResponse';

abstract class IUserApiBackendService {
    abstract getNotifications(): Observable<Array<GetNotificationsResponse>>;

    abstract sendTestMailMessage(testMailMessage: TestMailMessageRequest): Observable<CommandExecutionResult>;

    abstract getUserSettings(): Observable<Array<GetUserSettingsResponse>>;

    abstract updateUserSettings(updatedSettings: Array<UpdateUserSettingRequest>): Observable<CommandExecutionResult>;
}

export { IUserApiBackendService };