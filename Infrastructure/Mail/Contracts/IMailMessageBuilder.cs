using System.Net.Mail;

namespace MAS.Payments.Infrastructure.MailMessaging
{
    internal interface IMailMessageBuilder<TMailMessage>
        where TMailMessage : IMailMessage
    {
        MailMessage Build(TMailMessage mailMessage);
    }
}