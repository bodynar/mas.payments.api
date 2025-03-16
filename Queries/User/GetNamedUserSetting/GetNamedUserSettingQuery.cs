namespace MAS.Payments.Queries
{
    using MAS.Payments.DataBase;
    using MAS.Payments.Infrastructure.Query;

    public class GetNamedUserSettingQuery(
        string settingName
    ) : IQuery<UserSettings>
    {
        public string SettingName { get; } = settingName;
    }
}