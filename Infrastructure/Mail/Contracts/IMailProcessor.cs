namespace MAS.Payments.Infrastructure.MailMessaging
{
    public interface IMailProcessor
    {
        void Send(IMailMessage message);

        void Send<TModel>(IMailMessage<TModel> message);
    }
}