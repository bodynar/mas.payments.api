import { Injectable } from '@angular/core';

import { Observable, ReplaySubject, Subject } from 'rxjs';
import { tap } from 'rxjs/operators';

import { IUserApiBackendService } from 'services/backend/IUserApi.backend';
import { IUserService } from 'services/IUserService';

import GetUserNotificationRequest from 'models/request/user/getUserNotificationRequest';
import TestMailMessageRequest from 'models/request/user/testMailMessageRequest';
import UpdateUserSettingRequest from 'models/request/user/updateUserSettingRequest';

import CommandExecutionResult from 'models/response/commandExecutionResult';
import QueryExecutionResult from 'models/response/queryExecutionResult';
import GetNotificationsResponse from 'models/response/user/getNotificationsResponse';
import GetUserSettingsResponse from 'models/response/user/getUserSettingsResponse';

@Injectable()
class UserService implements IUserService {

    private whenHideNotifications$: Subject<Array<string>> =
        new ReplaySubject(1);

    constructor(
        private userApiBackend: IUserApiBackendService,
        // private loggingService: ILoggingService
    ) {
    }

    public getNotifications(request: GetUserNotificationRequest): Observable<QueryExecutionResult<Array<GetNotificationsResponse>>> {
        return this.userApiBackend
            .getNotifications(request)
            .pipe(
                tap(response => {
                    if (!response.success) {
                        // this.loggingService.error(response);
                    }
                }),
            );
    }

    public hideNotification(keys: Array<string>): Observable<CommandExecutionResult> {
        return this.userApiBackend
            .hideNotification(keys)
            .pipe(
                tap(response => {
                    if (response.success) {
                        this.whenHideNotifications$.next(keys);
                    }
                })
            );
    }

    public onNotificationsHidden(): Observable<Array<string>> {
        return this.whenHideNotifications$.asObservable();
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