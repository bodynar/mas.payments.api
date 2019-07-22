import { Injectable } from '@angular/core';

import { Observable, ReplaySubject, Subject } from 'rxjs';

import { INotificationService } from 'services/INotificationService';

import { Notification } from 'models/notification';
import { NotificationType } from 'models/notificationType';

@Injectable()
class NotificationService implements INotificationService {

    private whenMessageRecieved$: Subject<Notification> =
        new ReplaySubject(1);

    constructor(
    ) {
    }

    public notify(message: string, notificationType: NotificationType): void {
        this.whenMessageRecieved$.next({
            message: message,
            type: notificationType
        });
    }

    public error(message: string): void {
        this.notify(message, NotificationType.Error);
    }

    public success(message: string): void {
        this.notify(message, NotificationType.Success);
    }

    public info(message: string): void {
        this.notify(message, NotificationType.Info);
    }

    public warning(message: string): void {
        this.notify(message, NotificationType.Warning);
    }

    public whenMessageRecieved(): Observable<Notification> {
        return this.whenMessageRecieved$.asObservable();
    }
}

export { NotificationService };