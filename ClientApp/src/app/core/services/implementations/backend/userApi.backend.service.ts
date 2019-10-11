import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { Observable, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

import { isNullOrUndefined } from 'util';

import { IUserApiBackendService } from 'services/backend/IUserApi.backend';

import { TestMailMessageRequest } from 'models/request/testMailMessageRequest';
import { UserLoginRequest } from 'models/request/userLoginRequest';
import { UserRegisterRequest } from 'models/request/userRegisterRequest';
import { GetNotificationsResponse } from 'models/response/getNotificationsResponse';

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

    public register(userRegisterRequest: UserRegisterRequest): Observable<any> {
        return this.http
            .post(`${this.apiPrefix}/register`, userRegisterRequest)
            .pipe(catchError(error => of(error)));
    }

    public confirmRegistration(token: string): Observable<any> {
        return this.http
            .post(`${this.apiPrefix}/confirmRegistration`, { token: token })
            .pipe(catchError(error => of(error)));
    }

    public login(loginInformation: UserLoginRequest): Observable<any> {
        return this.http
            .post(`${this.apiPrefix}/auth`, loginInformation)
            .pipe(catchError(error => of(error)));
    }
}

export { UserApiBackendService };