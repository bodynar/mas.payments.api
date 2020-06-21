import { Component, OnDestroy, OnInit } from '@angular/core';

import { ReplaySubject, Subject } from 'rxjs';
import { filter, takeUntil } from 'rxjs/operators';

import { INotificationService } from 'services/INotificationService';
import { IUserService } from 'services/IUserService';

import GetNotificationsResponse from 'models/response/user/getNotificationsResponse';

@Component({
    templateUrl: 'userNotifications.template.pug'
})
export class UserNotificationsComponent implements OnInit, OnDestroy {

    public notifications$: Subject<Array<GetNotificationsResponse>> =
        new ReplaySubject(1);

    private whenComponentDestroy$: Subject<null> =
        new Subject();

    private notifications: Array<GetNotificationsResponse> =
        [];

    constructor(
        private userService: IUserService,
        private notificationService: INotificationService,
    ) {
    }

    public ngOnInit(): void {
        this.userService
            .getNotifications({ onlyActive: false })
            .pipe(
                takeUntil(this.whenComponentDestroy$),
                filter(result => {
                    if (!result.success) {
                        this.notificationService.error(result.error);
                    }

                    return result.success;
                })
            )
            .subscribe(({ result }) => {
                this.notifications = result;
                this.notifications$.next(this.notifications);
            });
    }

    public ngOnDestroy(): void {
        this.whenComponentDestroy$.next(null);
        this.whenComponentDestroy$.complete();
    }
}