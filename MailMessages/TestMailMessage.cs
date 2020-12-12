namespace MAS.Payments.MailMessages
{
    using System;

    using MAS.Payments.Infrastructure.MailMessaging;

    public class TestMailMessage : IMailMessage
    {
        public string TemplateName
            => "Test";

        public string Recipient { get; }

        public string Subject
            => "Test mail message";

        public TestMailMessage(string recipient)
        {
            Recipient = recipient ?? throw new ArgumentException(nameof(recipient));
        }
    }
}