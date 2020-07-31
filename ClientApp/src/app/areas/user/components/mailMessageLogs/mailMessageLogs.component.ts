import { Component, OnDestroy, OnInit } from '@angular/core';

import { ReplaySubject, Subject } from 'rxjs';
import { filter, switchMapTo, takeUntil } from 'rxjs/operators';

import * as moment from 'moment';

import { isNullOrUndefined } from 'util';

import { INotificationService } from 'services/INotificationService';
import { IUserService } from 'services/IUserService';
import { IModalService } from 'src/app/components/modal/IModalService';

import { getPaginatorConfig } from 'src/common/paginator/paginator';
import PaginatorConfig from 'src/common/paginator/paginatorConfig';

import GetMailLogsResponse from 'models/response/user/getMailLogsResponse';

import { TextInModalComponent } from 'src/app/components/modal/components/text/text.component';

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
        private modalService: IModalService
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
                this.logItems = result;

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

    public displayBody(logItemId: number): void {
        const logItem: GetMailLogsResponse =
            this.logItems.filter(x => x.id === logItemId).pop();

        if (!isNullOrUndefined(logItem)) {
            this.modalService.show(TextInModalComponent, {
                size: 'large',
                title: logItem.subject,
                body: {
                    content: logItem.body,
                    isHtml: true
                },
            });
        }
    }
}