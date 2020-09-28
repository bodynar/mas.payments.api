namespace MAS.Payments.Infrastructure.MailMessaging
{
    using System.Net.Mail;

    public interface ISmtpClientFactory
    {
        SmtpClient CreateSmtpClient();
    }
}