import { Component } from '@angular/core';

import { ReplaySubject, Subject } from 'rxjs';
import { filter, switchMapTo, takeUntil } from 'rxjs/operators';

import * as moment from 'moment';

import BaseComponent from 'common/components/BaseComponent';

import { INotificationService } from 'services/INotificationService';
import { IUserService } from 'services/IUserService';

import { getPaginatorConfig } from 'sharedComponents/paginator/paginator';
import PaginatorConfig from 'sharedComponents/paginator/paginatorConfig';

import GetNotificationsResponse from 'models/response/user/getNotificationsResponse';

@Component({
    templateUrl: 'userNotifications.template.pug',
    styleUrls: ['userNotifications.style.styl'],
})
export class UserNotificationsComponent extends BaseComponent {

    public notifications$: Subject<Array<GetNotificationsResponse>> =
        new ReplaySubject(1);

    public paginatorConfig$: Subject<PaginatorConfig> =
        new ReplaySubject(1);

    private whenUpdateNotifications$: Subject<null> =
        new Subject();

    private notifications: Array<GetNotificationsResponse> =
        [];

    private pageSize: number =
        5;

    constructor(
        private userService: IUserService,
        private notificationService: INotificationService,
    ) {
        super();

        this.whenComponentInit$
            .subscribe(() => {
                this.whenUpdateNotifications$.next(null);

                this.userService
                    .onNotificationsHidden()
                    .pipe(
                        takeUntil(this.whenComponentDestroy$),
                        filter(keys => this.notifications.some(x => keys.includes(x.key)))
                    )
                    .subscribe(_ => this.whenUpdateNotifications$.next(null));
            });

        this.whenUpdateNotifications$
            .pipe(
                takeUntil(this.whenComponentDestroy$),
                switchMapTo(this.userService.getNotifications({ onlyActive: false })),
                filter(result => {
                    if (!result.success) {
                        this.notificationService.error(result.error);
                    }

                    return result.success;
                })
            )
            .subscribe(({ result }) => {
                this.notifications = result;

                const paginatorConfig: PaginatorConfig =
                    getPaginatorConfig(this.notifications, this.pageSize);

                if (paginatorConfig.enabled) {
                    this.onPageChange(0);
                } else {
                    this.notifications$.next(this.notifications);
                }

                this.paginatorConfig$.next(paginatorConfig);
            });
    }

    public formatDate(date: Date): string {
        return moment(date).format('DD.MM.YYYY');
    }

    public onPageChange(pageNumber: number): void {
        const slicedNotifications: Array<GetNotificationsResponse> =
            this.notifications.slice(this.pageSize * pageNumber, (pageNumber + 1) * this.pageSize);

        this.notifications$.next(slicedNotifications);
    }
}