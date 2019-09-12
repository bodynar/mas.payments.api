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

        private Lazy<ICommandProcessor> commandProcessor;

        private Lazy<IQueryProcessor> queryProcessor;

        private Lazy<INotificationProcessor> notificationProcessor;

        private Lazy<IMailProcessor> mailProcessor;

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
            Resolver = resolver;

            commandProcessor = new Lazy<ICommandProcessor>(Resolver.Resolve<ICommandProcessor>());
            queryProcessor = new Lazy<IQueryProcessor>(Resolver.Resolve<IQueryProcessor>());
            notificationProcessor = new Lazy<INotificationProcessor>(Resolver.Resolve<INotificationProcessor>());
            mailProcessor = new Lazy<IMailProcessor>(Resolver.Resolve<IMailProcessor>());
        }
    }
}
