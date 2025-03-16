namespace MAS.Payments.Utilities
{
    using System;
    using System.Linq;

    using MAS.Payments.DataBase;

    public static class UserSettingUtilities
    {
        public static T GetTypedSettingValue<T>(UserSettings userSetting)
        {
            var type = typeof(T);
            var allowedTypes = Enum.GetNames<SettingDataValueType>().Select(x => x.ToLower());

            if (!allowedTypes.Contains(type.Name.ToLower()))
            {
                throw new ArgumentException(string.Format("Type must be declared in [{0}] array.", allowedTypes));
            }

            return (T)Convert.ChangeType(userSetting.RawValue, type);
        }
    }
}