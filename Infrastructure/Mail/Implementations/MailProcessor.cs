namespace MAS.Payments.Infrastructure.MailMessaging
{
    using System;
    using System.Net.Mail;
    using System.Threading.Tasks;

    using MAS.Payments.Commands;
    using MAS.Payments.Infrastructure.Command;

    public class MailProcessor : IMailProcessor
    {
        private readonly Lazy<IMailSender> mailSender;

        private IResolver Resolver { get; }

        private IMailSender MailSender
            => mailSender.Value;

        public MailProcessor(
            IResolver resolver
        )
        {
            Resolver = resolver;
            mailSender = new Lazy<IMailSender>(() => Resolver.Resolve<IMailSender>());
        }

        public async void Send(IMailMessage message)
        {
            dynamic builder = GetMailBuilder(message);

            var builtMessage = builder.Build((dynamic)message);

            await MailSender.SendMailAsync(builtMessage).ConfigureAwait(true);

            SaveMessageLogItem(builtMessage as MailMessage);
        }

        public async void Send<TModel>(IMailMessage<TModel> message)
        {
            dynamic builder = GetMailBuilder(message);

            var builtMessage = builder.Build((dynamic)message, message.Model);

            await MailSender.SendMailAsync(builtMessage).ConfigureAwait(true);

            SaveMessageLogItem(builtMessage as MailMessage);
        }

        private object GetMailBuilder<TMailMessage>(TMailMessage message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var handlerType = typeof(MailBuilder<>).MakeGenericType(message.GetType());

            return Resolver.GetInstance(handlerType);
        }

        private void SaveMessageLogItem(MailMessage mailMessage)
        {
            var commandProcessor = Resolver.Resolve<ICommandProcessor>();

            commandProcessor.Execute(
                new SaveMailMessageLogCommand(
                    mailMessage.To[0].Address, mailMessage.Subject, mailMessage.Body, DateTime.Now));
        }
    }
}