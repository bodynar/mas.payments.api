namespace MAS.Payments.Commands
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using MAS.Payments.DataBase;
    using MAS.Payments.DataBase.Access;
    using MAS.Payments.Infrastructure;
    using MAS.Payments.Infrastructure.Command;

    public class HideUserNotificationCommandHandler : BaseCommandHandler<HideUserNotificationCommand>
    {
        private IRepository<UserNotification> Repository { get; }

        public HideUserNotificationCommandHandler(IResolver resolver)
            : base(resolver)
        {
            Repository = GetRepository<UserNotification>();
        }

        public override async Task HandleAsync(HideUserNotificationCommand command)
        {
            var notificationsToHide = 
                Repository.Where(
                    new CommonSpecification.IdIn<UserNotification>(command.Ids)
                );

            var foundIds = notificationsToHide.Select(x => x.Id).ToList();

            var notFoundNotifications = command.Ids.Except(foundIds);

            if (notFoundNotifications.Any())
            {
                command.NotProcessedIds = notFoundNotifications;
            }

            foreach (var userNotification in notificationsToHide)
            {
                userNotification.IsHidden = true;
                userNotification.HiddenAt = DateTime.Now;

                await Repository.Update(userNotification.Id, userNotification);
            }
        }
    }
}