import { Component, OnDestroy, OnInit } from '@angular/core';

import { ReplaySubject, Subject } from 'rxjs';
import { filter, switchMapTo, takeUntil } from 'rxjs/operators';

import * as moment from 'moment';

import PaginatorConfig from 'src/common/paginator/paginatorConfig';

import GetMailLogsResponse from 'models/response/user/getMailLogsResponse';

import { INotificationService } from 'services/INotificationService';
import { IUserService } from 'services/IUserService';
import { getPaginatorConfig } from 'src/common/paginator/paginator';

@Component({
    templateUrl: 'mailMessageLogs.template.pug'
})
export class MailMessageLogsComponent implements OnInit, OnDestroy {
    public logItems$: Subject<Array<GetMailLogsResponse>> =
        new ReplaySubject(1);

    public paginatorConfig$: Subject<PaginatorConfig> =
        new ReplaySubject(1);

    private whenComponentDestroy$: Subject<null> =
        new Subject();

    private whenUpdateMailMessageLogs$: Subject<null> =
        new Subject();

    private logItems: Array<GetMailLogsResponse> =
        [];

    private pageSize: number =
        15;

    constructor(
        private userService: IUserService,
        private notificationService: INotificationService,
    ) {
        this.whenUpdateMailMessageLogs$
        .pipe(
            takeUntil(this.whenComponentDestroy$),
            switchMapTo(this.userService.getMailLogs()),
            filter(result => {
                if (!result.success) {
                    this.notificationService.error(result.error);
                }

                return result.success;
            })
        )
        .subscribe(({ result }) => {
            this.logItems = [...result, ...result, ...result, ...result, ...result];

            const paginatorConfig: PaginatorConfig =
                getPaginatorConfig(this.logItems, this.pageSize);

            if (paginatorConfig.enabled) {
                this.onPageChange(0);
            } else {
                this.logItems$.next(this.logItems);
            }

            this.paginatorConfig$.next(paginatorConfig);
        });
    }

    public ngOnInit(): void {
        this.whenUpdateMailMessageLogs$.next(null);
    }

    public ngOnDestroy(): void {
        this.whenComponentDestroy$.next(null);
        this.whenComponentDestroy$.complete();
    }

    public formatDate(date: Date): string {
        return moment(date).format('DD.MM.YYYY');
    }

    public onPageChange(pageNumber: number): void {
        const slicedNotifications: Array<GetMailLogsResponse> =
            this.logItems.slice(this.pageSize * pageNumber, (pageNumber + 1) * this.pageSize);

        this.logItems$.next(slicedNotifications);
    }
}