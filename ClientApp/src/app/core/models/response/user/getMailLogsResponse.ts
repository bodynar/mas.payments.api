export default interface GetMailLogsResponse {
    id: number;
    recipient: string;
    subject: string;
    body: string;
    sentDate: Date;
}