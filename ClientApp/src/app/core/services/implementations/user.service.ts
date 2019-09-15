import { Injectable } from '@angular/core';

import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { isNullOrUndefined } from 'util';

import { IUserApiBackendService } from 'services/backend/IUserApi.backend';
import { IUserService } from 'services/IUserService';

import { TestMailMessageRequest } from 'models/request/testMailMessageRequest';
import { GetNotificationsResponse } from 'models/response/getNotificationsResponse';

@Injectable()
class UserService implements IUserService {
    constructor(
        private userApiBackend: IUserApiBackendService,
        // private loggingService: ILoggingService
    ) {
    }

    public getNotifications(): Observable<Array<GetNotificationsResponse>> {
        return this.userApiBackend
            .getNotifications()
            .pipe(
                map(response => {
                    const hasError: boolean =
                        isNullOrUndefined(response) || !(response instanceof Array);

                    if (hasError) {
                        // this.loggingService.error(response);
                    }

                    return hasError ? [] : response;
                }),
            );
    }

    public sendTestMailMessage(testMailMessage: TestMailMessageRequest): Observable<boolean> {
        return this.userApiBackend
            .sendTestMailMessage(testMailMessage)
            .pipe(map(response => isNullOrUndefined(response)));
    }
}

export { UserService };