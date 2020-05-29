import { Injectable } from '@angular/core';

import { Observable } from 'rxjs';
import { map, tap } from 'rxjs/operators';

import { isNullOrUndefined } from 'util';

import { IUserApiBackendService } from 'services/backend/IUserApi.backend';
import { IUserService } from 'services/IUserService';

import TestMailMessageRequest from 'models/request/user/testMailMessageRequest';
import UpdateUserSettingRequest from 'models/request/user/updateUserSettingRequest';
import CommandExecutionResult from 'models/response/commandExecutionResult';
import QueryExecutionResult from 'models/response/queryExecutionResult';
import GetNotificationsResponse from 'models/response/user/getNotificationsResponse';
import GetUserSettingsResponse from 'models/response/user/getUserSettingsResponse';

@Injectable()
class UserService implements IUserService {
    constructor(
        private userApiBackend: IUserApiBackendService,
        // private loggingService: ILoggingService
    ) {
    }

    public getNotifications(): Observable<QueryExecutionResult<Array<GetNotificationsResponse>>> {
        return this.userApiBackend
            .getNotifications()
            .pipe(
                tap(response => {
                    if (!response.success) {
                        // this.loggingService.error(response);
                    }
                }),
            );
    }

    public sendTestMailMessage(testMailMessage: TestMailMessageRequest): Observable<CommandExecutionResult> {
        return this.userApiBackend
            .sendTestMailMessage(testMailMessage);
    }

    public getUserSettings(): Observable<QueryExecutionResult<Array<GetUserSettingsResponse>>> {
        return this.userApiBackend
            .getUserSettings()
            .pipe(
                tap(response => {
                    if (!response.success) {
                        // this.loggingService.error(response);
                    }
                }),
            );
    }

    public updateUserSettings(updatedSettings: Array<UpdateUserSettingRequest>): Observable<CommandExecutionResult> {
        return this.userApiBackend
            .updateUserSettings(updatedSettings);
    }
}

export { UserService };