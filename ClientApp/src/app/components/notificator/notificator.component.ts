import { Component } from '@angular/core';

import { Subject } from 'rxjs';
import { map } from 'rxjs/operators';

import { INotificationService } from 'services/INotificationService';

import { Notification } from 'models/notification';

@Component({
    selector: 'app-notificator',
    templateUrl: 'notificator.template.pug',
    styleUrls: ['notificator.style.styl']
})
class NotificatorComponent {

    public notifications$: Subject<Array<Notification>> =
        new Subject();

    private whenNotificationRecieved$: Subject<Notification> =
        new Subject<Notification>();

    private notifications: Array<Notification> =
        [];

    constructor(
        private notificationService: INotificationService
    ) {
        this.whenNotificationRecieved$
            .pipe(
                map(notification => ({
                    ...notification,
                    id: this.notifications.length
                }))
            )
            .subscribe(notification => {
                this.notifications.push(notification);
                this.notifications$.next(this.notifications);
            });

        this.notificationService
            .whenMessageRecieved()
            .subscribe(notification => this.whenNotificationRecieved$.next(notification));
    }

    public removeNotification(notificationId: number): void {
        this.notifications =
            this.notifications.filter(x => x.id !== notificationId);

            this.notifications$.next(this.notifications);
    }
}

export { NotificatorComponent };