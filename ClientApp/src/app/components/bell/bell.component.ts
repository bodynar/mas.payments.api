import { Component, OnDestroy, OnInit } from '@angular/core';

import { fromEvent, Observable, Subject } from 'rxjs';
import { filter, map, takeUntil } from 'rxjs/operators';

import { isNullOrUndefined } from 'util';

import { IUserService } from 'services/IUserService';

import { GetNotificationsResponse } from 'models/response/getNotificationsResponse';

@Component({
    selector: 'app-bell',
    templateUrl: 'bell.template.pug',
    styleUrls: ['bell.style.styl']
})
class BellComponent implements OnInit, OnDestroy {

    public notifications$: Subject<Array<GetNotificationsResponse>> =
        new Subject();

    public isNotificationsHidden: boolean =
        true;


    private notifications: Array<GetNotificationsResponse>;

    private pageClicks$: Observable<Event> =
        fromEvent(document, 'click');

    private whenComponentDestroy$: Subject<null> =
        new Subject();

    constructor(
        private userService: IUserService
    ) {
        this.pageClicks$
            .pipe(
                takeUntil(this.whenComponentDestroy$),
                filter(_ => !this.isNotificationsHidden),
                map(event => (event as MouseEvent).target),
                filter((target: HTMLElement) => !this.isBellChild(target))
            )
            .subscribe(_ => this.isNotificationsHidden = !this.isNotificationsHidden);
    }

    public ngOnInit(): void {
        this.userService
            .getNotifications()
            .pipe(takeUntil(this.whenComponentDestroy$))
            .subscribe(notifications => {
                this.notifications = notifications;
                this.notifications$.next(this.notifications);
            });
    }

    public ngOnDestroy(): void {
        this.whenComponentDestroy$.next(null);
        this.whenComponentDestroy$.complete();
    }

    public notificationsToggle(target: HTMLElement): void {
        if (this.isBellChild(target)) {
            this.isNotificationsHidden = !this.isNotificationsHidden;
        }
    }

    public removeNotification(notification: GetNotificationsResponse): void {
        console.warn(notification);
    }

    private isBellChild(element: HTMLElement) {
        let isBellChild: boolean =
            element.nodeName.toLowerCase() === 'app-bell';

        if (isBellChild) {
            return true;
        }

        if (!isNullOrUndefined(element.parentElement)) {
            isBellChild = this.isBellChild(element.parentElement);
        }

        return isBellChild;
    }
}

export { BellComponent };