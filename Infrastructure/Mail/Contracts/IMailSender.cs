using System.Net.Mail;
using System.Threading.Tasks;

namespace MAS.Payments.Infrastructure.MailMessaging
{
    public interface IMailSender
    {
        Task SendMailAsync(MailMessage message);
    }
}