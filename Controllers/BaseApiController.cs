using MAS.Payments.Infrastructure.Command;
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
        protected ICommandProcessor CommandProcessor { get; }

        protected IQueryProcessor QueryProcessor { get; }

        protected INotificationProcessor NotificationProcessor { get; }

        public BaseApiController(
            ICommandProcessor commandProcessor,
            IQueryProcessor queryProcessor,
            INotificationProcessor notificationProcessor
        )
        {
            CommandProcessor = commandProcessor;
            QueryProcessor = queryProcessor;
            NotificationProcessor = notificationProcessor;
        }
    }
}
