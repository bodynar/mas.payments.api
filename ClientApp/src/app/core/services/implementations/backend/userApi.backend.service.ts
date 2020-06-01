import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { Observable, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

import { isNullOrUndefined } from 'util';

import { IUserApiBackendService } from 'services/backend/IUserApi.backend';

import TestMailMessageRequest from 'models/request/user/testMailMessageRequest';
import UpdateUserSettingRequest from 'models/request/user/updateUserSettingRequest';
import CommandExecutionResult from 'models/response/commandExecutionResult';
import QueryExecutionResult from 'models/response/queryExecutionResult';
import GetNotificationsResponse from 'models/response/user/getNotificationsResponse';
import GetUserSettingsResponse from 'models/response/user/getUserSettingsResponse';

@Injectable()
class UserApiBackendService implements IUserApiBackendService {

    private readonly apiPrefix: string =
        '/api/user';
    constructor(
        private http: HttpClient
    ) {
    }

    public getNotifications(): Observable<QueryExecutionResult<Array<GetNotificationsResponse>>> {
        return this.http
            .get(`${this.apiPrefix}/getNotifications`)
            .pipe(
                map((response: Array<any>) =>
                    response.map(notification => ({
                        name: notification['name'],
                        description: notification['description'],
                        type: notification['type'].toLowerCase()
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

    public sendTestMailMessage(testMailMessage: TestMailMessageRequest): Observable<CommandExecutionResult> {
        const url: string =
            isNullOrUndefined(testMailMessage.name)
                ? `${this.apiPrefix}/testMailMessage`
                : `${this.apiPrefix}/testMailWithModelMessage`;

        return this.http
            .post(url, testMailMessage)
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

        return this.http
            .get(`${this.apiPrefix}/getSettings`)
            .pipe(
                map((response: Array<any>) =>
                    response.map(setting => ({
                        id: setting['id'],
                        name: setting['name'],
                        typeName: setting['typeName'],
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
}

export { UserApiBackendService };