using System.Collections.Generic;

namespace MAS.Payments.Infrastructure.MailMessaging
{
    public interface IMailMessage
    {
        string Reciepent { get; }

        string TemplateName { get; }

        string Subject { get; }
    }
}