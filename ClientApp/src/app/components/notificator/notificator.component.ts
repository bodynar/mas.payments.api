import { Component, OnDestroy } from '@angular/core';

import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

import { INotificationService } from 'services/INotificationService';

import { Notification } from 'models/notification';
import { NotificationType } from 'models/notificationType';

@Component({
    selector: 'app-notificator',
    templateUrl: 'notificator.template.pug',
    styleUrls: ['notificator.style.styl']
})
export class NotificatorComponent implements OnDestroy {
    private whenComponentDestroy$: Subject<null> =
        new Subject();

    public notifications: Array<Notification> =
        [];

    constructor(
        private notificationService: INotificationService
    ) {
        this.notificationService
            .whenMessageRecieved()
            .pipe(
                takeUntil(this.whenComponentDestroy$)
            )
            .subscribe(notification => this.notifications.push(notification));
    }

    public ngOnDestroy(): void {
        this.whenComponentDestroy$.next(null);
        this.whenComponentDestroy$.complete();
    }

    public getClassName(notificationType: NotificationType): string {
        switch (notificationType) {
            case NotificationType.Error:
                return 'm-bg-danger';
            case NotificationType.Success:
                return 'm-bg-success';
            case NotificationType.Warning:
                return 'bg-warning';
            default:
                return '';
        }
    }

    public getTypeName(notificationType: number): string {
        return NotificationType[notificationType].toString();
    }

    public removeNotification(notification: Notification): void {
        this.notifications = this.notifications.filter(t => t !== notification);
    }
}