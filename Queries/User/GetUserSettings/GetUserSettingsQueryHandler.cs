using System.Collections.Generic;
using System.Linq;

using MAS.Payments.DataBase;
using MAS.Payments.DataBase.Access;
using MAS.Payments.Infrastructure;
using MAS.Payments.Infrastructure.Query;
using MAS.Payments.Projectors;

namespace MAS.Payments.Queries
{
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
            return Repository.GetAll(new Projector.ToFlat<UserSettings, GetUserSettingsQueryResult>()).ToList();
        }
    }
}
