import { NotificationType } from './notificationType';

interface Notification {
    id?: number;
    message: string;
    type: NotificationType;
    delay: number;
}

export { Notification };