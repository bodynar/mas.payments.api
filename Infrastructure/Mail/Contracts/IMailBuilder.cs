using System.Net.Mail;

namespace MAS.Payments.Infrastructure.MailMessaging
{
    public interface IMailBuilder
    {
        MailMessage FormTestMailMessage(string to);
    }
}