import { Observable } from 'rxjs';

import { Notification } from 'models/notification';
import { NotificationType } from 'models/notificationType';

abstract class INotificationService {

    abstract notify(message: string, notificationType: NotificationType, delay?: number): void;

    abstract error(message: string, delay?: number): void;

    abstract success(message: string, delay?: number): void;

    abstract info(message: string, delay?: number): void;

    abstract warning(message: string, delay?: number): void;

    abstract whenMessageRecieved(): Observable<Notification>;
}

export { INotificationService };