export default interface GetNotificationsResponse {
    title: string;
    text: string;
    key: string;
    type: string;
    createdAt: Date;
    id?: number;
    hiddenAt?: Date;
    isHidden?: boolean;
}