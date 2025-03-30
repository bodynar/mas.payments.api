namespace MAS.Payments.Controllers
{
    using System;

    using MAS.Payments.Infrastructure;
    using MAS.Payments.Infrastructure.Command;
    using MAS.Payments.Infrastructure.Query;
    using MAS.Payments.Notifications;

    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Produces("application/json", "multipart/form-data")]
    [Consumes("application/json", "multipart/form-data")]
    public abstract class BaseApiController(
        IResolver resolver
    ) : ControllerBase
    {
        #region Private fields

        private ICommandProcessor commandProcessor;

        private IQueryProcessor queryProcessor;

        private INotificationProcessor notificationProcessor;

        #endregion

        protected IResolver Resolver { get; } = resolver ?? throw new ArgumentNullException(nameof(resolver));

        protected ICommandProcessor CommandProcessor
            => commandProcessor ??= resolver.Resolve<ICommandProcessor>();

        protected IQueryProcessor QueryProcessor
            => queryProcessor ??= resolver.Resolve<IQueryProcessor>();

        protected INotificationProcessor NotificationProcessor
            => notificationProcessor ??= resolver.Resolve<INotificationProcessor>();
    }
}
