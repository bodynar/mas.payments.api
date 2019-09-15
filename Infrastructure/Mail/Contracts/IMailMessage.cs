namespace MAS.Payments.Infrastructure.MailMessaging
{
    public interface IMailMessage
    {
        string Recipient { get; }

        string TemplateName { get; }

        string Subject { get; }
    }

    public interface IMailMessage<TModel> : IMailMessage
    {
        TModel Model { get; }
    }
}