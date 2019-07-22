import { NotificationType } from './notificationType';

interface Notification {
    id?: number;
    message: string;
    type: NotificationType | string;
}

export { Notification };