namespace MAS.Payments.Queries
{
    using System.Threading.Tasks;

    using MAS.Payments.DataBase;
    using MAS.Payments.DataBase.Access;
    using MAS.Payments.Infrastructure;
    using MAS.Payments.Infrastructure.Query;
    using MAS.Payments.Infrastructure.Specification;

    using Microsoft.EntityFrameworkCore;

    internal class GetNamedUserSettingQueryHandler : BaseQueryHandler<GetNamedUserSettingQuery, UserSettings>
    {
        private IRepository<UserSettings> Repository { get; }

        public GetNamedUserSettingQueryHandler(
            IResolver resolver
        ) : base(resolver)
        {
            Repository = GetRepository<UserSettings>();
        }

        public override async Task<UserSettings> HandleAsync(GetNamedUserSettingQuery query)
        {
            return await Repository.Where(new CommonSpecification<UserSettings>(userSetting => userSetting.Name == query.SettingName)).FirstOrDefaultAsync();
        }
    }
}