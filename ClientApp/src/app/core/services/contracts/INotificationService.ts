import { Observable } from 'rxjs';

import { Notification } from 'models/notification';
import { NotificationType } from 'models/notificationType';

abstract class INotificationService {

    abstract notify(message: string, notificationType: NotificationType): void;

    abstract error(message: string): void;

    abstract success(message: string): void;

    abstract info(message: string): void;

    abstract warning(message: string): void;

    abstract whenMessageRecieved(): Observable<Notification>;
}

export { INotificationService };