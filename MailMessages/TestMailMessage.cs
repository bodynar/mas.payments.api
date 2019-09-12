using MAS.Payments.Infrastructure.MailMessaging;

namespace MAS.Payments.MailMessages
{
    public class TestMailMessage : IMailMessage
    {
        public string TemplateName
            => "Test";

        public string Reciepent { get; }

        public string Subject
            => "Test mail message";

        public TestMailMessage(string reciepent)
        {
            Reciepent = reciepent;
        }
    }
}