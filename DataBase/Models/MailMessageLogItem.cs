namespace MAS.Payments.DataBase
{
    using System;

    public class MailMessageLogItem: Entity
    {
        public string Recipient { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }
        
        public DateTime SentDate { get; set; }
    }
}