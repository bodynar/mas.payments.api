namespace MAS.Payments.Queries
{
    using System.Collections.Generic;
    using System.Linq;

    using MAS.Payments.DataBase;
    using MAS.Payments.DataBase.Access;
    using MAS.Payments.Infrastructure;
    using MAS.Payments.Infrastructure.Query;

    internal class GetUserSettingsQueryHandler : BaseQueryHandler<GetUserSettingsQuery, IReadOnlyCollection<GetUserSettingsQueryResult>>
    {
        private IRepository<UserSettings> Repository { get; }

        public GetUserSettingsQueryHandler(
            IResolver resolver
        ) : base(resolver)
        {
            Repository = GetRepository<UserSettings>();
        }

        public override IReadOnlyCollection<GetUserSettingsQueryResult> Handle(GetUserSettingsQuery query)
        {
            return Repository
                .GetAll()
                .Select(x => new GetUserSettingsQueryResult
                {
                    Id = x.Id,
                    Name = x.Name,
                    DisplayName = x.DisplayName,
                    TypeName = x.TypeName,
                    RawValue = x.RawValue,
                })
                .ToList();
        }
    }
}