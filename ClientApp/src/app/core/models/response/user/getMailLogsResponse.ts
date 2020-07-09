export default interface GetMailLogsResponse {
    recipient: string;
    subject: string;
    body: string;
    sentDate: Date;
}