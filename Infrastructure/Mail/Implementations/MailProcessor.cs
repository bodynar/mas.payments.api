namespace MAS.Payments.Infrastructure.MailMessaging
{
    using System;
    using System.Linq;
    using System.Net.Mail;

    using MAS.Payments.Commands;
    using MAS.Payments.Infrastructure.Command;

    public class MailProcessor : IMailProcessor
    {
        private readonly Lazy<IMailSender> mailSender;
        private readonly Lazy<ICommandProcessor> commandProcessor;

        private IResolver Resolver { get; }

        private IMailSender MailSender
            => mailSender.Value;

        private ICommandProcessor CommandProcessor
            => commandProcessor.Value;

        public MailProcessor(
            IResolver resolver
        )
        {
            Resolver = resolver;
            mailSender = new Lazy<IMailSender>(() => Resolver.Resolve<IMailSender>());
            commandProcessor = new Lazy<ICommandProcessor>(() => Resolver.Resolve<ICommandProcessor>());
        }

        public async void Send(IMailMessage message)
        {
            var builtMessage = BuildMailMessage(message: message);

            SaveMessageLogItem(builtMessage as MailMessage); // todo: re-design mail message components to store log items correctly

            await MailSender.SendMailAsync(builtMessage).ConfigureAwait(true);
        }

        public async void Send<TModel>(IMailMessage<TModel> message)
        {
            var builtMessage = BuildMailMessage(messageWithModel: message as IMailMessage<object>);

            await MailSender.SendMailAsync(builtMessage).ConfigureAwait(true);

            SaveMessageLogItem(builtMessage as MailMessage);
        }

        private MailMessage BuildMailMessage(IMailMessage message = null, IMailMessage<object> messageWithModel = null)
        {
            dynamic builder = GetMailBuilder(message ?? messageWithModel);

            var builtMessage = builder.Build((dynamic)(message ?? messageWithModel), (dynamic)messageWithModel?.Model);

            return builtMessage;
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
            var messageBody = GetMessageBody(mailMessage.Body);
            var recipients = string.Join(", ", mailMessage.To.Select(x => x.Address));

            CommandProcessor.Execute(
                new SaveMailMessageLogCommand(
                    recipients, mailMessage.Subject, messageBody, DateTime.Now));
        }

        private static string GetMessageBody(string htmlMailMessage)
        {
            var bodyEnd = htmlMailMessage.ToLower().IndexOf("</body>");
            var bodyStart = htmlMailMessage.ToLower().IndexOf("<body>") + "<body>".Length;

            return htmlMailMessage.Substring(bodyStart, bodyEnd - bodyStart);
        }
    }
}