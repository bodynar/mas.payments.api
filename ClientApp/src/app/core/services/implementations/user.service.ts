import { Injectable } from '@angular/core';

import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { isNullOrUndefined } from 'util';

import { IUserApiBackendService } from 'services/backend/IUserApi.backend';
import { IHasherService } from 'services/IHasherService';
import { IUserService } from 'services/IUserService';

import { TestMailMessageRequest } from 'models/request/testMailMessageRequest';
import { UserRegisterRequest } from 'models/request/userRegisterRequest';
import { GetNotificationsResponse } from 'models/response/getNotificationsResponse';

@Injectable()
class UserService implements IUserService {
    constructor(
        private userApiBackend: IUserApiBackendService,
        private hashService: IHasherService,
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

    public register(userRegisterRequest: UserRegisterRequest): Observable<boolean> {
        const request: UserRegisterRequest = {
            login: userRegisterRequest.login,
            passwordHash: this.hashService.generateHash(userRegisterRequest.password),
            email: userRegisterRequest.email,
            firstName: userRegisterRequest.firstName,
            lastName: userRegisterRequest.lastName
        };

        return this.userApiBackend
            .register(request)
            .pipe(map(response => isNullOrUndefined(response)));
    }

    public confirmRegistration(token: string): Observable<boolean> {
        return this.userApiBackend
            .confirmRegistration(token)
            .pipe(map(response => isNullOrUndefined(response)));
    }
}

export { UserService };