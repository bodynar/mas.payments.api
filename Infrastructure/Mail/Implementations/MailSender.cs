namespace MAS.Payments.Infrastructure.MailMessaging
{
    using System.Net.Mail;
    using System.Threading.Tasks;

    public class MailSender : IMailSender
    {
        private ISmtpClientFactory Factory { get; }

        public MailSender(
            ISmtpClientFactory factory
        )
        {
            Factory = factory;
        }

        public async Task SendMailAsync(MailMessage message)
        {
            var smtpClient = Factory.CreateSmtpClient();

            await smtpClient.SendMailAsync(message).ConfigureAwait(true);
        }
    }
}