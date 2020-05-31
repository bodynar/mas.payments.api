using System.Net.Mail;

namespace MAS.Payments.Infrastructure.MailMessaging
{
    public interface ISmtpClientFactory
    {
        SmtpClient CreateSmtpClient();
    }
}