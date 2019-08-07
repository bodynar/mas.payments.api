using System.Collections.Generic;
using MAS.Payments.Infrastructure.Command;
using MAS.Payments.Infrastructure.Query;
using MAS.Payments.Notifications;
using Microsoft.AspNetCore.Mvc;

namespace MAS.Payments.Controllers
{
    [Route("api/user")]
    public class UserApiController : BaseApiController
    {
        public UserApiController(
            ICommandProcessor commandProcessor,
            IQueryProcessor queryProcessor,
            INotificationProcessor notificationProcessor
        ) : base(commandProcessor, queryProcessor, notificationProcessor)
        {
        }

        [HttpGet("[action]")]
        public IEnumerable<Notification> GetNotifications()
        {
            return NotificationProcessor.GetNotifications();
        }
    }
}