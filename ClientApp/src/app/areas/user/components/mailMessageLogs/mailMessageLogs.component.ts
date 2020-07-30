import { Component, OnDestroy, OnInit } from '@angular/core';

import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

import { ReplaySubject, Subject } from 'rxjs';
import { filter, switchMapTo, takeUntil } from 'rxjs/operators';

import * as moment from 'moment';

import PaginatorConfig from 'src/common/paginator/paginatorConfig';

import GetMailLogsResponse from 'models/response/user/getMailLogsResponse';

import { INotificationService } from 'services/INotificationService';
import { IUserService } from 'services/IUserService';
import { TextInModalComponent } from 'src/app/components/modal/text/text.component';
import { getPaginatorConfig } from 'src/common/paginator/paginator';
import { isNullOrUndefined } from 'util';

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
        private modalService: NgbModal
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
            const modalRef = this.modalService.open(TextInModalComponent, { size: 'lg' });
            modalRef.componentInstance.text = logItem.body;
            modalRef.componentInstance.isHtml = true;
            modalRef.componentInstance.title = logItem.subject;
        }
    }
}