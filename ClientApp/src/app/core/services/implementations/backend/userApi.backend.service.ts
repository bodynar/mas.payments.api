import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { Observable, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

import { boxServerResponse, boxServerQueryResponse } from 'common/utils/api';

import { IUserApiBackendService } from 'services/backend/IUserApi.backend';

import CommandExecutionResult from 'models/response/commandExecutionResult';
import QueryExecutionResult from 'models/response/queryExecutionResult';

import { UpdateUserSettingRequest, TestMailMessageRequest, GetUserNotificationRequest } from 'models/request/user';
import { GetMailLogsResponse, GetNotificationsResponse, GetUserSettingsResponse, GetAppInfoResponse } from 'models/response/user';

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
                catchError(error => of(error)),
                map(x => boxServerQueryResponse<Array<GetNotificationsResponse>>(x)),
            );
    }

    public hideNotification(keys: Array<string>): Observable<CommandExecutionResult> {
        return this.http
            .post(`${this.apiPrefix}/hideNotifications`, keys)
            .pipe(
                catchError(error => of(error)),
                map(x => boxServerResponse(x)),
            );
    }

    public sendTestMailMessage(testMailMessage: TestMailMessageRequest): Observable<CommandExecutionResult> {
        return this.http
            .post(`${this.apiPrefix}/testMailMessage`, testMailMessage)
            .pipe(
                catchError(error => of(error)),
                map(x => boxServerResponse(x)),
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
                catchError(error => of(error)),
                map(x => boxServerQueryResponse<Array<GetUserSettingsResponse>>(x)),
            );
    }

    public updateUserSettings(updatedSettings: Array<UpdateUserSettingRequest>): Observable<CommandExecutionResult> {
        return this.http
            .post(`${this.apiPrefix}/updateUserSettings`, updatedSettings)
            .pipe(
                catchError(error => of(error)),
                map(x => boxServerResponse(x)),
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
                catchError(error => of(error)),
                map(x => boxServerQueryResponse<Array<GetMailLogsResponse>>(x)),
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
                map(x => boxServerQueryResponse<GetAppInfoResponse>(x)),
            );
    }
}

export { UserApiBackendService };