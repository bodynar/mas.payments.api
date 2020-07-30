namespace MAS.Payments.Commands
{
    using System;

    using MAS.Payments.Infrastructure.Command;

    public class SaveMailMessageLogCommand : ICommand
    {
        public string Recipient { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public DateTime SentDate { get; set; }

        public SaveMailMessageLogCommand(string recipient, string subject, string body, DateTime sentDate)
        {
            Recipient = recipient ?? throw new ArgumentNullException(nameof(recipient));
            Subject = subject ?? throw new ArgumentNullException(nameof(subject));
            Body = body ?? throw new ArgumentNullException(nameof(body));
            SentDate = sentDate;
        }
    }
}