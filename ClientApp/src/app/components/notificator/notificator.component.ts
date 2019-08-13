import { Component, OnDestroy, OnInit } from '@angular/core';

import { Subject } from 'rxjs';
import { delay, map, takeUntil, tap } from 'rxjs/operators';

import { INotificationService } from 'services/INotificationService';

import { Notification } from 'models/notification';
import { NotificationType } from 'models/notificationType';

@Component({
    selector: 'app-notificator',
    templateUrl: 'notificator.template.pug',
    styleUrls: ['notificator.style.styl']
})
class NotificatorComponent implements OnInit, OnDestroy {

    public notifications$: Subject<Array<Notification>> =
        new Subject();

    private whenUpdateNotifications$: Subject<Array<Notification>> =
        new Subject();

    private whenNotificationRecieved$: Subject<Notification> =
        new Subject();

    private whenNotificationDismissDelayed$: Subject<number> =
        new Subject();

    private notifications: Array<Notification> =
        [];

    private whenComponentDestroy$: Subject<null> =
        new Subject();

    constructor(
        private notificationService: INotificationService
    ) {
        this.whenNotificationDismissDelayed$
            .pipe(
                takeUntil(this.whenComponentDestroy$),
                delay(5 * 1000),
            )
            .subscribe(notificationIndex => {
                this.removeNotification(notificationIndex);
            });

        this.whenNotificationRecieved$
            .pipe(
                takeUntil(this.whenComponentDestroy$),
                map(notification => ({
                    ...notification,
                    id: this.notifications.length
                })),
                tap(notification => this.notifications.push(notification))
            )
            .subscribe(notification => {
                this.whenUpdateNotifications$.next(this.notifications);
                this.whenNotificationDismissDelayed$.next(notification.id);
            });

        this.whenUpdateNotifications$
            .pipe(
                takeUntil(this.whenComponentDestroy$),
                map(notifications =>
                    notifications.map(notification =>
                        ({
                            ...notification,
                            type: NotificationType[notification.type]
                        }))
                )
            )
            .subscribe(notifications => this.notifications$.next(notifications));

        this.notificationService
            .whenMessageRecieved()
            .subscribe(notification => this.whenNotificationRecieved$.next(notification));
    }

    public ngOnInit(): void {
    }

    public ngOnDestroy(): void {
        this.whenComponentDestroy$.next(null);
        this.whenComponentDestroy$.complete();
    }

    public removeNotification(notificationId: number): void {
        const notification: Notification =
            this.notifications.find(x => x.id === notificationId);

        const notificationIndex: number =
            this.notifications.indexOf(notification);

        this.notifications.splice(notificationIndex, 1);

        this.whenUpdateNotifications$.next(this.notifications);
    }
}

export { NotificatorComponent };