﻿namespace MAS.Payments.Queries
{
    using System.Collections.Generic;

    using MAS.Payments.Infrastructure.Query;

    public enum GetUserNotificationsType
    {
        All = 1,
        Visible = 2,
        Hidden = 3,
    }

    public class GetUserNotificationsQuery(
        GetUserNotificationsType notificationsType
    ) : IQuery<IEnumerable<GetUserNotificationsQueryResult>>
    {
        public GetUserNotificationsType NotificationsType { get; } = notificationsType;
    }
}