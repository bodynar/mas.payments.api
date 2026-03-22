namespace MAS.Payments.Commands
{
    using System.Collections.Generic;

    using MAS.Payments.Infrastructure.Command;

    public class HideUserNotificationCommand : ICommand
    {
        public IEnumerable<Guid> Ids { get; }

        public IEnumerable<Guid> NotProcessedIds { get; set; } = [];

        public HideUserNotificationCommand(IEnumerable<Guid> ids)
        {
            Ids = [.. ids];
        }

        public HideUserNotificationCommand(Guid id)
        {
            Ids = [id];
        }
    }
}
