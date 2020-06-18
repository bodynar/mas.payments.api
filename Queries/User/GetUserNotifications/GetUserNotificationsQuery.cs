namespace MAS.Payments.Queries
{
    using System.Collections.Generic;

    using MAS.Payments.DataBase;
    using MAS.Payments.Infrastructure.Query;

    public enum GetUserNotificationsType
    {
        All = 1,
        Visible = 2,
        Hidden = 3,
    }

    public class GetUserNotificationsQuery: IQuery<IEnumerable<UserNotification>>
    {
        public GetUserNotificationsType NotificationsType { get; }

        public GetUserNotificationsQuery(GetUserNotificationsType notificationsType)
        {
            NotificationsType = notificationsType;
        }
    }
}