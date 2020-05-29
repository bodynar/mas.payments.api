import { Component, OnDestroy, OnInit } from '@angular/core';

import { fromEvent, Observable, Subject, ReplaySubject } from 'rxjs';
import { filter, map, takeUntil } from 'rxjs/operators';

import { isNullOrUndefined } from 'util';

import { IUserService } from 'services/IUserService';

import GetNotificationsResponse from 'models/response/user/getNotificationsResponse';

@Component({
    selector: 'app-bell',
    templateUrl: 'bell.template.pug',
    styleUrls: ['bell.style.styl']
})
class BellComponent implements OnInit, OnDestroy {

    public notifications$: Subject<Array<GetNotificationsResponse>> =
        new ReplaySubject();

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
        if (this.isBellChild(target) && !this.isBellList(target)) {
            this.isNotificationsHidden = !this.isNotificationsHidden;
        }
    }

    public removeNotification(notification: GetNotificationsResponse): void {
        this.notifications = this.notifications.filter(notificationItem => notificationItem != notification);
        this.notifications$.next(this.notifications);
    }

    private isBellChild(element: HTMLElement): boolean {
        let isBellChild: boolean =
            element.nodeName.toLowerCase() === 'app-bell';

        const bellListItemAttribute = element.attributes.getNamedItem('data-bell-list-child');

        if (isBellChild || (!isNullOrUndefined(bellListItemAttribute) && bellListItemAttribute.value === 'true')) {
            return true;
        }

        if (!isNullOrUndefined(element.parentElement)) {
            isBellChild = this.isBellChild(element.parentElement);
        }

        return isBellChild;
    }

    private isBellList(element: HTMLElement): boolean {
        const isRootBell: boolean =
            element.nodeName.toLowerCase() === 'app-bell';

        if (isRootBell) {
            return false;
        }

        const bellListItemAttribute = element.attributes.getNamedItem('data-bell-list-child');
        if (!isNullOrUndefined(bellListItemAttribute) && bellListItemAttribute.value === 'true') {
            return true;
        }

        if (!isNullOrUndefined(element.parentElement)) {
            return this.isBellList(element.parentElement);
        }

        return false;
    }
}

export { BellComponent };