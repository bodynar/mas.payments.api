import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { Observable, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

import { IUserApiBackendService } from 'services/backend/IUserApi.backend';

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
                        type: notification['type']
                    }) as GetNotificationsResponse)),
                catchError(error => of(error))
            );
    }
}

export { UserApiBackendService };