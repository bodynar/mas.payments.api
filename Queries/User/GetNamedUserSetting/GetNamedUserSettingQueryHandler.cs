namespace MAS.Payments.Queries
{
    using System.Linq;

    using MAS.Payments.DataBase;
    using MAS.Payments.DataBase.Access;
    using MAS.Payments.Infrastructure;
    using MAS.Payments.Infrastructure.Query;
    using MAS.Payments.Infrastructure.Specification;

    internal class GetNamedUserSettingQueryHandler : BaseQueryHandler<GetNamedUserSettingQuery, UserSettings>
    {
        private IRepository<UserSettings> Repository { get; }

        public GetNamedUserSettingQueryHandler(
            IResolver resolver
        ) : base(resolver)
        {
            Repository = GetRepository<UserSettings>();
        }

        public override UserSettings Handle(GetNamedUserSettingQuery query)
        {
            return Repository.Where(new CommonSpecification<UserSettings>(userSetting => userSetting.Name == query.SettingName)).FirstOrDefault();
        }
    }
}