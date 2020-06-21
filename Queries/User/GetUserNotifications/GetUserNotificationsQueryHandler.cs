namespace MAS.Payments.Queries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using MAS.Payments.DataBase;
    using MAS.Payments.DataBase.Access;
    using MAS.Payments.Infrastructure;
    using MAS.Payments.Infrastructure.Projector;
    using MAS.Payments.Infrastructure.Query;
    using MAS.Payments.Infrastructure.Specification;
    using MAS.Payments.Projectors;

    internal class GetUserNotificationsQueryHandler : BaseQueryHandler<GetUserNotificationsQuery, IEnumerable<GetUserNotificationsQueryResult>>
    {
        private IRepository<UserNotification> Repository { get; }

        private IProjector<UserNotification, GetUserNotificationsQueryResult> Projector { get;  }

        public GetUserNotificationsQueryHandler(
            IResolver resolver
        ) : base(resolver)
        {
            Repository = GetRepository<UserNotification>();
            Projector =
                new Projector.Common<UserNotification, GetUserNotificationsQueryResult>(x => 
                    new GetUserNotificationsQueryResult
                    {
                        Id = x.Id,
                        CreatedAt = x.CreatedAt,
                        HiddenAt = x.HiddenAt,
                        IsHidden = x.IsHidden,
                        Text = x.Text,
                        Title = x.Title,
                        Key = x.Key,
                        Type = Enum.GetName(typeof(UserNotificationType), x.Type)
                    });
        }

        public override IEnumerable<GetUserNotificationsQueryResult> Handle(GetUserNotificationsQuery query)
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
                .Where(specification, Projector)
                .ToList();
        }
    }
}