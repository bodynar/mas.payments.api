import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { Observable, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

import { isNullOrUndefined } from 'util';

import { IUserApiBackendService } from 'services/backend/IUserApi.backend';

import TestMailMessageRequest from 'models/request/user/testMailMessageRequest';
import UpdateUserSettingRequest from 'models/request/user/updateUserSettingRequest';
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

    public getNotifications(): Observable<Array<GetNotificationsResponse>> {
        return this.http
            .get(`${this.apiPrefix}/getNotifications`)
            .pipe(
                map((response: Array<any>) =>
                    response.map(notification => ({
                        name: notification['name'],
                        description: notification['description'],
                        type: notification['type'].toLowerCase()
                    }) as GetNotificationsResponse)),
                catchError(error => of(error))
            );
    }

    public sendTestMailMessage(testMailMessage: TestMailMessageRequest): Observable<any> {
        const url: string =
            isNullOrUndefined(testMailMessage.name)
                ? `${this.apiPrefix}/testMailMessage`
                : `${this.apiPrefix}/testMailWithModelMessage`;

        return this.http
            .post(url, testMailMessage)
            .pipe(catchError(error => of(error)));
    }

    public getUserSettings(): Observable<Array<GetUserSettingsResponse>> {
        return this.http
            .get(`${this.apiPrefix}/getSettings`)
            .pipe(
                map((response: Array<any>) =>
                    response.map(setting => ({
                        id: setting['Id'],
                        name: setting['Name'],
                        typeName: setting['TypeName'],
                        rawValue: setting['RawValue'],
                        displayName: setting['DisplayName'],
                    }) as GetUserSettingsResponse)),
                catchError(error => of(error))
            );
    }

    public updateUserSettings(updatedSettings: Array<UpdateUserSettingRequest>): Observable<boolean> {
        return this.http
            .post(`${this.apiPrefix}/updateSettings`, updatedSettings)
            .pipe(catchError(error => of(error)));
    }
}

export { UserApiBackendService };