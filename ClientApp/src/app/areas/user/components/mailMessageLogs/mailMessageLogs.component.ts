import { Component } from '@angular/core';

import { BehaviorSubject, ReplaySubject, Subject } from 'rxjs';
import { filter, switchMapTo, takeUntil, tap } from 'rxjs/operators';

import * as moment from 'moment';

import { isNullOrUndefined } from 'common/utils/common';

import { INotificationService } from 'services/INotificationService';
import { IUserService } from 'services/IUserService';
import { IModalService } from 'src/app/components/modal/IModalService';

import { getPaginatorConfig } from 'sharedComponents/paginator/paginator';
import PaginatorConfig from 'sharedComponents/paginator/paginatorConfig';

import GetMailLogsResponse from 'models/response/user/getMailLogsResponse';

import { BaseComponentWithModalComponent } from 'common/components/BaseComponentWithModal';

@Component({
    templateUrl: 'mailMessageLogs.template.pug'
})
export class MailMessageLogsComponent extends BaseComponentWithModalComponent {
    public hasData$: Subject<boolean> =
        new BehaviorSubject(false);

    public isLoading$: Subject<boolean> =
        new BehaviorSubject(false);

    public logItems$: Subject<Array<GetMailLogsResponse>> =
        new ReplaySubject(1);

    public paginatorConfig$: Subject<PaginatorConfig> =
        new ReplaySubject(1);

    private whenUpdateMailMessageLogs$: Subject<null> =
        new Subject();

    private logItems: Array<GetMailLogsResponse> =
        [];

    private pageSize: number =
        15;

    constructor(
        private userService: IUserService,
        private notificationService: INotificationService,
        modalService: IModalService
    ) {
        super(modalService);

        this.whenComponentInit$
            .subscribe(() => this.whenUpdateMailMessageLogs$.next(null));

        this.whenUpdateMailMessageLogs$
            .pipe(
                takeUntil(this.whenComponentDestroy$),
                tap(_ => this.isLoading$.next(true)),
                switchMapTo(this.userService.getMailLogs()),
                tap(_ => this.isLoading$.next(false)),
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

                this.hasData$.next(result.length > 0);
                this.paginatorConfig$.next(paginatorConfig);
            });
    }

    public formatDate(date: Date): string {
        return moment(date).format('DD.MM.YYYY');
    }

    public formatTime(date: Date): string {
        return moment(date).format('hh:mm:ss');
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
            this.showInModal({
                title: logItem.subject,
                size: 'large',
                isHtml: true,
                body: logItem.body
            });
        }
    }
}