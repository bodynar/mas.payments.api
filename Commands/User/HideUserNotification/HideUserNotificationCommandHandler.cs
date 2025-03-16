﻿namespace MAS.Payments.Commands
{
    using System;
    using System.Linq;

    using MAS.Payments.DataBase;
    using MAS.Payments.DataBase.Access;
    using MAS.Payments.Infrastructure;
    using MAS.Payments.Infrastructure.Command;
    using MAS.Payments.Infrastructure.Specification;

    using Serilog;

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
                    new CommonSpecification<UserNotification>(x => command.Ids.Contains(x.Id))
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

                Repository.Update(userNotification.Id, userNotification);
            }
        }
    }
}