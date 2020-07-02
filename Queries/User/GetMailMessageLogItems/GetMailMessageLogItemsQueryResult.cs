using System;

namespace MAS.Payments.Queries
{
    public class GetMailMessageLogItemsQueryResult
    {
        public string Recipient { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public DateTime SentDate { get; set; }
    }
}