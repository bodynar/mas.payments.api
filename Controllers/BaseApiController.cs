using System;
using MAS.Payments.Infrastructure;
using MAS.Payments.Infrastructure.Command;
using MAS.Payments.Infrastructure.MailMessaging;
using MAS.Payments.Infrastructure.Query;
using MAS.Payments.Notifications;
using Microsoft.AspNetCore.Mvc;

namespace MAS.Payments.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Consumes("application/json")]
    public abstract class BaseApiController : Controller
    {
        #region Private fields

        private readonly Lazy<ICommandProcessor> commandProcessor;

        private readonly Lazy<IQueryProcessor> queryProcessor;

        private readonly Lazy<INotificationProcessor> notificationProcessor;

        private readonly Lazy<IMailProcessor> mailProcessor;

        #endregion

        protected IResolver Resolver { get; }

        protected ICommandProcessor CommandProcessor
            => commandProcessor.Value;

        protected IQueryProcessor QueryProcessor
            => queryProcessor.Value;

        protected INotificationProcessor NotificationProcessor
            => notificationProcessor.Value;

        protected IMailProcessor MailProcessor
            => mailProcessor.Value;

        public BaseApiController(
            IResolver resolver
        )
        {
            Resolver = resolver ?? throw new ArgumentNullException(nameof(Resolver));

            commandProcessor = new Lazy<ICommandProcessor>(resolver.Resolve<ICommandProcessor>());
            queryProcessor = new Lazy<IQueryProcessor>(resolver.Resolve<IQueryProcessor>());
            notificationProcessor = new Lazy<INotificationProcessor>(resolver.Resolve<INotificationProcessor>());
            mailProcessor = new Lazy<IMailProcessor>(resolver.Resolve<IMailProcessor>());
        }
    }
}
