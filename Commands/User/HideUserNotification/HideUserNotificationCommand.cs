namespace MAS.Payments.Commands
{
    using System.Collections.Generic;

    using MAS.Payments.Infrastructure.Command;

    public class HideUserNotificationCommand : ICommand
    {
        public IEnumerable<long> Ids { get; } = new List<long>();

        public IEnumerable<long> NotProcessedIds { get; set; } = new List<long>();

        public HideUserNotificationCommand(IEnumerable<long> ids)
        {
            Ids = new List<long>(ids);
        }

        public HideUserNotificationCommand(long id)
        {
            Ids = new List<long>() { id };
        }
    }
}