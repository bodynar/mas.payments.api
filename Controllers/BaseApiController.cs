namespace MAS.Payments.Controllers
{
    using System;

    using MAS.Payments.Infrastructure;
    using MAS.Payments.Infrastructure.Command;
    using MAS.Payments.Infrastructure.Query;
    using MAS.Payments.Notifications;

    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Produces("application/json")]
    [Consumes("application/json")]
    public abstract class BaseApiController(
        IResolver resolver
    ) : ControllerBase
    {
        #region Private fields

        private readonly Lazy<ICommandProcessor> commandProcessor = new(resolver.Resolve<ICommandProcessor>());

        private readonly Lazy<IQueryProcessor> queryProcessor = new(resolver.Resolve<IQueryProcessor>());

        private readonly Lazy<INotificationProcessor> notificationProcessor = new(resolver.Resolve<INotificationProcessor>());

        #endregion

        protected IResolver Resolver { get; } = resolver ?? throw new ArgumentNullException(nameof(resolver));

        protected ICommandProcessor CommandProcessor
            => commandProcessor.Value;

        protected IQueryProcessor QueryProcessor
            => queryProcessor.Value;

        protected INotificationProcessor NotificationProcessor
            => notificationProcessor.Value;
    }
}
