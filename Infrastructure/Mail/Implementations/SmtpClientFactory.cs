using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;

namespace MAS.Payments.Infrastructure.MailMessaging
{
    internal class SmtpClientFactory : ISmtpClientFactory
    {
        private SmtpSettings SmtpSettings { get; }

        public SmtpClientFactory(
            IOptions<SmtpSettings> smtpSettings
        )
        {
            SmtpSettings = smtpSettings.Value;
        }

        public SmtpClient CreateSmtpClient() => new SmtpClient(SmtpSettings.Host, SmtpSettings.Port)
        {
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(SmtpSettings.Username, SmtpSettings.Password),
            EnableSsl = SmtpSettings.EnableSsl
        };
    }
}