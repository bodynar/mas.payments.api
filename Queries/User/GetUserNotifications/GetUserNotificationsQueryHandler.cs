namespace MAS.Payments.Queries
{
    using System.Collections.Generic;
    using System.Linq;

    using MAS.Payments.DataBase;
    using MAS.Payments.DataBase.Access;
    using MAS.Payments.Infrastructure;
    using MAS.Payments.Infrastructure.Query;
    using MAS.Payments.Infrastructure.Specification;

    internal class GetUserNotificationsQueryHandler : BaseQueryHandler<GetUserNotificationsQuery, IEnumerable<UserNotification>>
    {
        private IRepository<UserNotification> Repository { get; }

        public GetUserNotificationsQueryHandler(
            IResolver resolver
        ) : base(resolver)
        {
            Repository = GetRepository<UserNotification>();
        }

        public override IEnumerable<UserNotification> Handle(GetUserNotificationsQuery query)
        {
            Specification<UserNotification> specification = new CommonSpecification<UserNotification>(x => true);

            switch (query.NotificationsType)
            {
                case GetUserNotificationsType.Visible:
                    specification = new CommonSpecification<UserNotification>(x => !x.IsHidden);
                    break;
                case GetUserNotificationsType.Hidden:
                    specification = new CommonSpecification<UserNotification>(x => x.IsHidden);
                    break;
                default:
                    break;
            }

            return Repository
                .Where(specification)
                .ToList();
        }
    }
}