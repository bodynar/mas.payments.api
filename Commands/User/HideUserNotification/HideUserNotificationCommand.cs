namespace MAS.Payments.Commands
{
    using System.Collections.Generic;

    using MAS.Payments.Infrastructure.Command;

    public class HideUserNotificationCommand : ICommand
    {
        public IEnumerable<long> Ids { get; }

        public IEnumerable<long> NotProcessedIds { get; set; } = [];

        public HideUserNotificationCommand(IEnumerable<long> ids)
        {
            Ids = [.. ids];
        }

        public HideUserNotificationCommand(long id)
        {
            Ids = [id];
        }
    }
}