namespace MAS.Payments.Infrastructure.MailMessaging
{
    public interface IMailProcessor
    {
        void Send<TMailMessage>(TMailMessage message)
            where TMailMessage: IMailMessage;
    }
}