import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { Observable, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

import { isNullOrUndefined } from 'common/utils/common';

import { IUserApiBackendService } from 'services/backend/IUserApi.backend';

import GetUserNotificationRequest from 'models/request/user/getUserNotificationRequest';
import TestMailMessageRequest from 'models/request/user/testMailMessageRequest';
import UpdateUserSettingRequest from 'models/request/user/updateUserSettingRequest';

import CommandExecutionResult from 'models/response/commandExecutionResult';
import QueryExecutionResult from 'models/response/queryExecutionResult';

import GetMailLogsResponse from 'models/response/user/getMailLogsResponse';
import GetNotificationsResponse from 'models/response/user/getNotificationsResponse';
import GetUserSettingsResponse from 'models/response/user/getUserSettingsResponse';
import GetAppInfoResponse from 'models/response/user/getAppInfoResponse';

@Injectable()
class UserApiBackendService implements IUserApiBackendService {

    private readonly apiPrefix: string =
        '/api/user';

    constructor(
        private http: HttpClient
    ) {
    }

    public getNotifications(request: GetUserNotificationRequest): Observable<QueryExecutionResult<Array<GetNotificationsResponse>>> {
        const apiMethod: string =
            request.onlyActive
                ? `${this.apiPrefix}/getActiveUserNotifications`
                : `${this.apiPrefix}/getUserNotifications`;

        return this.http
            .get(apiMethod)
            .pipe(
                map((response: Array<any>) =>
                    response.map(x => ({
                        title: x['title'],
                        text: x['text'],
                        key: x['key'],
                        type: x['type'],
                        createdAt: x['createdAt'],

                        id: request.onlyActive ? undefined : x['id'],
                        hiddenAt: request.onlyActive ? undefined : x['hiddenAt'],
                        isHidden: request.onlyActive ? undefined : x['isHidden'],
                    }) as GetNotificationsResponse)),
                catchError(error => of(error.error)),
                map(x => isNullOrUndefined(x.Success)
                    ? ({
                        success: true,
                        result: x
                    })
                    : ({
                        success: false,
                        error: x['Message'],
                    })
                ),
            );
    }

    public hideNotification(keys: Array<string>): Observable<CommandExecutionResult> {
        return this.http
            .post(`${this.apiPrefix}/hideNotifications`, keys)
            .pipe(
                catchError(error => of(error.error)),
                map(x => x
                    ? (({
                        success: false,
                        error: x['Message'],
                    }) as CommandExecutionResult)
                    : ({
                        success: true,
                        args: keys
                    })
                ),
            );
    }

    public sendTestMailMessage(testMailMessage: TestMailMessageRequest): Observable<CommandExecutionResult> {
        return this.http
            .post(`${this.apiPrefix}/testMailMessage`, testMailMessage)
            .pipe(
                catchError(error => of(error.error)),
                map(x => x
                    ? (({
                        success: false,
                        error: x['Message'],
                    }) as CommandExecutionResult)
                    : ({ success: true })
                ),
            );
    }

    public getUserSettings(): Observable<QueryExecutionResult<Array<GetUserSettingsResponse>>> {
        const getMappedValue = (typeName: string, rawValue: string): any => {
            if (typeName === 'Boolean') {
                return rawValue.toLocaleLowerCase() === 'true';
            } else if (typeName === 'Number') {
                return +rawValue;
            } else {
                return rawValue;
            }
        };

        const updateTypeName = (typeName: string): string => {
            return typeName === 'Int32' ? 'Number' : typeName;
        };

        return this.http
            .get(`${this.apiPrefix}/getSettings`)
            .pipe(
                map((response: Array<any>) =>
                    response.map(setting => ({
                        id: setting['id'],
                        name: setting['name'],
                        typeName: updateTypeName(setting['typeName']),
                        rawValue: setting['rawValue'],
                        displayName: setting['displayName'],
                        value: getMappedValue(setting['typeName'], setting['rawValue'])
                    }) as GetUserSettingsResponse)),
                catchError(error => of(error.error)),
                map(x => isNullOrUndefined(x.Success)
                    ? ({
                        success: true,
                        result: x
                    })
                    : ({
                        success: false,
                        error: x['Message'],
                    })
                ),
            );
    }

    public updateUserSettings(updatedSettings: Array<UpdateUserSettingRequest>): Observable<CommandExecutionResult> {
        return this.http
            .post(`${this.apiPrefix}/updateUserSettings`, updatedSettings)
            .pipe(
                catchError(error => of(error.error)),
                map(x => x
                    ? (({
                        success: false,
                        error: x['Message'],
                    }) as CommandExecutionResult)
                    : ({ success: true })
                ),
            );
    }

    public getMailLogs(): Observable<QueryExecutionResult<Array<GetMailLogsResponse>>> {
        return this.http
            .get(`${this.apiPrefix}/getMailMessageLogs`)
            .pipe(
                map((response: Array<any>) =>
                    response.map(logItem => ({
                        id: logItem['id'],
                        recipient: logItem['recipient'],
                        subject: logItem['subject'],
                        body: logItem['body'],
                        sentDate: logItem['sentDate'],
                    }) as GetMailLogsResponse)),
                catchError(error => of(error.error)),
                map(x => isNullOrUndefined(x.Success)
                    ? ({
                        success: true,
                        result: x
                    })
                    : ({
                        success: false,
                        error: x['Message'],
                    })
                ),
            );
    }

    public getAppInfo(): Observable<QueryExecutionResult<GetAppInfoResponse>> {
        return this.http
            .get(`${this.apiPrefix}/getAppInfo`)
            .pipe(
                map(response => ({
                    dataBaseName: response['dataBaseName'],
                    serverAppVersion: response['serverAppVersion']
                }) as GetAppInfoResponse),
                catchError(error => of(error)),
                map(x => isNullOrUndefined(x.Success) && (isNullOrUndefined(x.ok) || x.ok === true)
                    ? ({
                        success: true,
                        result: x
                    })
                    : ({
                        success: false,
                        error: x['Message'] || x.error,
                    })
                ),
            );
    }
}

export { UserApiBackendService };