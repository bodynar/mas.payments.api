namespace MAS.Payments.Queries
{
    using MAS.Payments.DataBase;
    using MAS.Payments.Infrastructure.Query;

    public class GetNamedUserSettingQuery : IQuery<UserSettings>
    {
        public string SettingName { get; }

        public GetNamedUserSettingQuery(string settingName)
        {
            SettingName = settingName;
        }
    }
}