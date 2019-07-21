import { NotificationType } from './NotificationType';

interface Notification {
    message: string;
    type: NotificationType;
    delay?: number;
}

export { Notification };