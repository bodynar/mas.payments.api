import { Component } from '@angular/core';

import { fromEvent, Observable, ReplaySubject, Subject } from 'rxjs';
import { filter, map, switchMap, takeUntil, switchMapTo } from 'rxjs/operators';

import BaseComponent from 'common/components/BaseComponent';

import { isNullOrUndefined } from 'common/utils/common';

import { INotificationService } from 'services/INotificationService';
import { IRouterService } from 'services/IRouterService';
import { IUserService } from 'services/IUserService';

import GetNotificationsResponse from 'models/response/user/getNotificationsResponse';

@Component({
    selector: 'app-bell',
    templateUrl: 'bell.template.pug',
    styleUrls: ['bell.style.styl']
})
export class BellComponent extends BaseComponent {

    public notifications$: Subject<Array<GetNotificationsResponse>> =
        new ReplaySubject();

    public isNotificationsHidden: boolean =
        true;


    private notifications: Array<GetNotificationsResponse>;

    private pageClicks$: Observable<Event> =
        fromEvent(document, 'click');

    private onHideNotification$: Subject<string> =
        new Subject();

    private onHideNotifications$: Subject<Array<string>> =
        new Subject();

    constructor(
        private userService: IUserService,
        private notificationService: INotificationService,
        private routerService: IRouterService,
    ) {
        super();

        this.whenComponentInit$
            .pipe(
                switchMapTo(this.userService.getNotifications({ onlyActive: true })),
                takeUntil(this.whenComponentDestroy$),
                filter(response => {
                    if (!response.success) {
                        this.notificationService.error(response.error);
                    }
                    return response.success;
                }),
                map(response => response.result),
            )
            .subscribe(notifications => {
                this.notifications = notifications;
                this.notifications$.next(this.notifications);
            });

        this.pageClicks$
            .pipe(
                takeUntil(this.whenComponentDestroy$),
                filter(_ => !this.isNotificationsHidden),
                map(event => (event as MouseEvent).target),
                filter((target: HTMLElement) => !this.isBellChild(target))
            )
            .subscribe(_ => this.isNotificationsHidden = !this.isNotificationsHidden);

        this.onHideNotifications$
            .pipe(
                takeUntil(this.whenComponentDestroy$),
                filter(key =>
                    !isNullOrUndefined(key)
                    && key.length > 0
                    && !key.some(x => isNullOrUndefined(x))
                ),
                switchMap(keys => this.userService.hideNotification(keys)),
                filter(result => {
                    if (!result.success) {
                        this.notificationService.error(result.error);
                    }

                    return result.success;
                })
            )
            .subscribe(_ => {
                this.notifications = [];
                this.notifications$.next(this.notifications);
            });

        this.onHideNotification$
            .pipe(
                takeUntil(this.whenComponentDestroy$),
                filter(key => this.notifications.some(x => x.key === key)),
                switchMap(key => this.userService.hideNotification([key])),
                filter(result => {
                    if (!result.success) {
                        this.notificationService.error(result.error);
                    }

                    return result.success;
                })
            )
            .subscribe(({ args }) => {
                const key: string =
                    (args as Array<string>).pop();

                this.notifications = this.notifications.filter(notificationItem => notificationItem.key !== key);
                this.notifications$.next(this.notifications);
            });
    }

    public notificationsToggle(target: HTMLElement): void {
        if (this.isBellChild(target) && !this.isBellList(target)) {
            if (this.notifications.length > 0) {
                this.isNotificationsHidden = !this.isNotificationsHidden;
            } else {
                this.onNotificationListClick();
            }
        }
    }

    public hideNotification(key: string): void {
        this.onHideNotification$.next(key);
    }

    public hideAll(): void {
        const keys: Array<string> =
            this.notifications.map(x => x.key);

        this.onHideNotifications$.next(keys);
    }

    public onNotificationListClick(): void {
        this.routerService.navigate(['app', 'user', 'notifications']);
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