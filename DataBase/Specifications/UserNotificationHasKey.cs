namespace MAS.Payments.DataBase
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using MAS.Payments.Infrastructure.Specification;

    public static class UserNotificationSpec
    {
        public class IsKeyIn : Specification<UserNotification>
        {
            public IEnumerable<string> Keys { get; }

            public IsKeyIn(IEnumerable<string> keys)
            {
                Keys = [.. keys];
            }

            public IsKeyIn(string key)
            {
                Keys = [key];
            }

            public override Expression<Func<UserNotification, bool>> IsSatisfied()
                => notification => Keys.Contains(notification.Key);
        }
    }
}
