import { Component } from '@angular/core';

import { BehaviorSubject, ReplaySubject, Subject } from 'rxjs';
import { filter, map, switchMapTo, tap } from 'rxjs/operators';

import { version } from '../../../../../../package.json';

import BaseComponent from 'common/components/BaseComponent';

import { IUserService } from 'services/IUserService';
import { INotificationService } from 'services/INotificationService';

type ApplicationVersion = {
    dataBaseName?: string;
    serverAppVersion?: string;
    clientAppVersion: string;
};

@Component({ templateUrl: 'userCard.template.pug' })
export class UserCardComponent extends BaseComponent {
    public isLoading$: Subject<boolean> =
        new BehaviorSubject(false);

    public applicationInfo$: Subject<ApplicationVersion> =
        new ReplaySubject(1);

    constructor(
        private userService: IUserService,
        private notificationService: INotificationService,
    ) {
        super();

        this.whenComponentInit$
            .pipe(
                tap(_ => this.isLoading$.next(true)),
                switchMapTo(this.userService.getAppInfo()),
                tap(_ => this.isLoading$.next(false)),
                filter(response => {
                    if (!response.success) {
                        this.notificationService.error(response.error);

                        this.applicationInfo$.next({ clientAppVersion: version });
                    }

                    return response.success;
                }),
                map(({ result }) => ({
                    ...result,
                    clientAppVersion: version
                }) as ApplicationVersion)
            )
            .subscribe((appVersion: ApplicationVersion) => this.applicationInfo$.next(appVersion));
    }
}