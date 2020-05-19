using System;

namespace MAS.Payments.Infrastructure.MailMessaging
{
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

            await MailSender.SendMailAsync(builtMessage);
        }

        public async void Send<TModel>(IMailMessage<TModel> message)
        {
            dynamic builder = GetMailBuilder(message);

            var builtMessage = builder.Build((dynamic)message, message.Model);

            await MailSender.SendMailAsync(builtMessage);
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
    }
}