import { Component, OnDestroy, OnInit } from '@angular/core';

import { ReplaySubject, Subject } from 'rxjs';
import { filter, switchMapTo, takeUntil } from 'rxjs/operators';

import * as moment from 'moment';

import { INotificationService } from 'services/INotificationService';
import { IUserService } from 'services/IUserService';

import { getPaginatorConfig } from 'src/common/paginator/paginator';

import GetNotificationsResponse from 'models/response/user/getNotificationsResponse';
import PaginatorConfig from 'src/common/paginator/paginatorConfig';

@Component({
    templateUrl: 'userNotifications.template.pug',
    styleUrls: ['userNotifications.style.styl'],
})
export class UserNotificationsComponent implements OnInit, OnDestroy {

    public notifications$: Subject<Array<GetNotificationsResponse>> =
        new ReplaySubject(1);

    public paginatorConfig$: Subject<PaginatorConfig> =
        new ReplaySubject(1);

    private whenComponentDestroy$: Subject<null> =
        new Subject();

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

                this.onPageChange(0);

                const paginatorConfig: PaginatorConfig =
                    getPaginatorConfig(this.notifications, this.pageSize);
                this.paginatorConfig$.next(paginatorConfig);
            });
    }

    public ngOnInit(): void {
        this.whenUpdateNotifications$.next(null);

        this.userService
            .onNotificationsHidden()
            .pipe(
                takeUntil(this.whenComponentDestroy$),
                filter(keys => this.notifications.some(x => keys.includes(x.key)))
            )
            .subscribe(_ => this.whenUpdateNotifications$.next(null));
    }

    public ngOnDestroy(): void {
        this.whenComponentDestroy$.next(null);
        this.whenComponentDestroy$.complete();
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