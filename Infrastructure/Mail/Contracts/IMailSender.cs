namespace MAS.Payments.Infrastructure.MailMessaging
{
    using System.Net.Mail;
    using System.Threading.Tasks;

    public interface IMailSender
    {
        Task SendMailAsync(MailMessage message);
    }
}