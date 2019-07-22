import { NotificationType } from './notificationType';

interface Notification {
    id?: number;
    message: string;
    type: NotificationType | string;
    delay?: number;
}

export { Notification };