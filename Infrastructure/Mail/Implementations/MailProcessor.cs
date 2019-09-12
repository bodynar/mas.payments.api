using System;

namespace MAS.Payments.Infrastructure.MailMessaging
{
    public class MailProcessor : IMailProcessor
    {
        private Lazy<IMailSender> mailSender;

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

        public async void Send<TMailMessage>(TMailMessage message)
            where TMailMessage : IMailMessage
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var handlerType = typeof(MailBuilder<>).MakeGenericType(message.GetType());

            dynamic handler = Resolver.GetInstance(handlerType);

            var builtMessage = handler.Build((dynamic)message);

            await MailSender.SendMailAsync(builtMessage);
        }
    }
}