namespace MAS.Payments.Commands
{
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

        public override void Handle(HideUserNotificationCommand command)
        {
            var notificationsToHide = 
                Repository.Where(
                    new CommonSpecification.IdIn<UserNotification>(command.NotificationIds)
                );

            foreach (var userNotification in notificationsToHide)
            {
                userNotification.IsHidden = true;
                Repository.Update(userNotification.Id, userNotification);
            }
        }
    }
}