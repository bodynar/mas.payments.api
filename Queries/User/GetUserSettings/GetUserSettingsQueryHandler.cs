namespace MAS.Payments.Queries
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using MAS.Payments.DataBase;
    using MAS.Payments.DataBase.Access;
    using MAS.Payments.Infrastructure;
    using MAS.Payments.Infrastructure.Query;

    using Microsoft.EntityFrameworkCore;

    internal class GetUserSettingsQueryHandler : BaseQueryHandler<GetUserSettingsQuery, IReadOnlyCollection<GetUserSettingsQueryResult>>
    {
        private IRepository<UserSettings> Repository { get; }

        public GetUserSettingsQueryHandler(
            IResolver resolver
        ) : base(resolver)
        {
            Repository = GetRepository<UserSettings>();
        }

        public override async Task<IReadOnlyCollection<GetUserSettingsQueryResult>> HandleAsync(GetUserSettingsQuery query)
        {
            return await Repository
                .GetAll()
                .Select(x => new GetUserSettingsQueryResult
                {
                    Id = x.Id,
                    Name = x.Name,
                    DisplayName = x.DisplayName,
                    TypeName = x.TypeName,
                    RawValue = x.RawValue,
                })
                .ToListAsync();
        }
    }
}