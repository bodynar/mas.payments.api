namespace MAS.Payments.Queries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using MAS.Payments.DataBase;
    using MAS.Payments.DataBase.Access;
    using MAS.Payments.Infrastructure;
    using MAS.Payments.Infrastructure.Query;
    using MAS.Payments.Infrastructure.Specification;

    internal class GetUserNotificationsQueryHandler : BaseQueryHandler<GetUserNotificationsQuery, IEnumerable<GetUserNotificationsQueryResult>>
    {
        private IRepository<UserNotification> Repository { get; }

        public GetUserNotificationsQueryHandler(
            IResolver resolver
        ) : base(resolver)
        {
            Repository = GetRepository<UserNotification>();
        }

        public override Task<IEnumerable<GetUserNotificationsQueryResult>> HandleAsync(GetUserNotificationsQuery query)
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

            return Task.FromResult(
                Repository
                    .Where(specification)
                    .OrderBy(x => x.CreatedAt)
                    .Select(x => new
                    {
                        x.Id,
                        x.CreatedAt,
                        x.HiddenAt,
                        x.IsHidden,
                        x.Text,
                        x.Title,
                        x.Key,
                        x.Type,
                    })
                    .AsEnumerable()
                    .Select(x =>
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
                        }
                    )
                );
        }
    }
}