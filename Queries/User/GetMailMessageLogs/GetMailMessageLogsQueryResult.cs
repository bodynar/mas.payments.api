namespace MAS.Payments.Queries
{
    using System;

    public class GetMailMessageLogsQueryResult
    {
        public string Recipient { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public DateTime SentDate { get; set; }
    }
}