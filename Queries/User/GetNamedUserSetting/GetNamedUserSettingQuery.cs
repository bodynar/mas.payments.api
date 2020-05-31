using MAS.Payments.DataBase;
using MAS.Payments.Infrastructure.Query;

namespace MAS.Payments.Queries
{
    public class GetNamedUserSettingQuery : IQuery<UserSettings>
    {
        public string SettingName { get; }

        public GetNamedUserSettingQuery(string settingName)
        {
            SettingName = settingName;
        }
    }
}