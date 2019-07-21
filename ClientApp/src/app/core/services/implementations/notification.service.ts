import { Injectable } from '@angular/core';

import { Observable, ReplaySubject, Subject } from 'rxjs';

import { INotificationService } from 'services/INotificationService';

import { Notification } from 'models/Notification';
import { NotificationType } from 'models/NotificationType';

@Injectable()
class NotificationService implements INotificationService {

    private whenMessageRecieved$: Subject<Notification> =
        new ReplaySubject(1);

    constructor(
    ) {
    }

    public notify(message: string, notificationType: NotificationType, delay?: number): void {
        this.whenMessageRecieved$.next({
            message: message,
            type: notificationType,
            delay: delay
        });
    }

    public error(message: string, delay?: number): void {
        this.notify(message, NotificationType.Error, delay);
    }

    public success(message: string, delay?: number): void {
        this.notify(message, NotificationType.Success, delay);
    }

    public info(message: string, delay?: number): void {
        this.notify(message, NotificationType.Info, delay);
    }

    public warning(message: string, delay?: number): void {
        this.notify(message, NotificationType.Warning, delay);
    }

    public whenMessageRecieved(): Observable<Notification> {
        return this.whenMessageRecieved$.asObservable();
    }

}

export { NotificationService };