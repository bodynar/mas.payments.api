namespace MAS.Payments.Commands
{
    using System.Linq;

    using MAS.Payments.DataBase;
    using MAS.Payments.DataBase.Access;
    using MAS.Payments.Infrastructure;
    using MAS.Payments.Infrastructure.Command;
    using MAS.Payments.Infrastructure.Exceptions;

    public class AddUserNotificationCommandHandler : BaseCommandHandler<AddUserNotificationCommand>
    {
        private IRepository<UserNotification> Repository { get; }

        public AddUserNotificationCommandHandler(IResolver resolver) 
            : base(resolver)
        {
            Repository = GetRepository<UserNotification>();
        }

        public override void Handle(AddUserNotificationCommand command)
        {
            var notificationKeys = command.UserNotifications.Select(x => x.Key);

            if (notificationKeys.Any(string.IsNullOrEmpty))
            {
                throw new CommandExecutionException(CommandType, "Cannot add user notification with empty key.");
            }

            var hasDuplicateKeys = Repository.Any(new UserNotificationSpec.IsKeyIn(notificationKeys));

            if (hasDuplicateKeys)
            {
                throw new CommandExecutionException(CommandType, "Cannot add user notification with duplicate keys.");
            }

            Repository.AddRange(command.UserNotifications);
        }
    }
}
