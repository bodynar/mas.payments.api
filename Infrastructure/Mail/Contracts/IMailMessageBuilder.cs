namespace MAS.Payments.Infrastructure.MailMessaging
{
    using System.Net.Mail;

    internal interface IMailMessageBuilder<TMailMessage>
        where TMailMessage : IMailMessage
    {
        MailMessage Build(TMailMessage mailMessage, object model = null);
    }
}