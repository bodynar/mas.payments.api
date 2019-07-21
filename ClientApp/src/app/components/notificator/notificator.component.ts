import { Component } from '@angular/core';

import { Subject } from 'rxjs';

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

            )
            .subscribe(notification => {
                this.notifications.push(notification);
            });

        this.notificationService
            .whenMessageRecieved()
            .subscribe(notification => this.whenNotificationRecieved$.next(notification));
    }
}

export { NotificatorComponent };