import { Component } from '@angular/core';

import { ReplaySubject, Subject } from 'rxjs';
import { filter, map, switchMapTo } from 'rxjs/operators';

import { version } from '../../../../../../package.json';

import BaseComponent from 'common/components/BaseComponent';

import { IUserService } from 'services/IUserService';
import { INotificationService } from 'services/INotificationService';

type ApplicationVersion = {
    dataBaseName: string;
    serverAppVersion: string;
    clientAppVersion: string;
};

@Component({
    templateUrl: 'userCard.template.pug'
})
export class UserCardComponent extends BaseComponent {

    public applicationInfo$: Subject<ApplicationVersion> =
        new ReplaySubject(1);

    constructor(
        private userService: IUserService,
        private notificationService: INotificationService,
    ) {
        super();

        this.whenComponentInit$
            .pipe(
                switchMapTo(this.userService.getAppInfo()),
                filter(response => {
                    if (!response.success) {
                        this.notificationService.error(response.error);
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