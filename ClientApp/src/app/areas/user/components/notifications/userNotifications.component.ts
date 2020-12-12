import { Component } from '@angular/core';

import { BehaviorSubject, ReplaySubject, Subject } from 'rxjs';
import { delay, filter, switchMap, takeUntil } from 'rxjs/operators';

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
    public hasData$: Subject<boolean> =
        new BehaviorSubject(false);

    public isLoading$: Subject<boolean> =
        new BehaviorSubject(true);

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
                switchMap(_ => {
                    this.isLoading$.next(true);
                    return this.userService.getNotifications({ onlyActive: false });
                }),
                delay(1.5 * 1000),
                filter(result => {
                    this.isLoading$.next(false);
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

                this.hasData$.next(result.length > 0);
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