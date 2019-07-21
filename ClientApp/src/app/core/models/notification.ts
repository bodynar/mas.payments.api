import { NotificationType } from './notificationType';

interface Notification {
    message: string;
    type: NotificationType;
    delay?: number;
}

export { Notification };