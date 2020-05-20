using MAS.Payments.DataBase;
using MAS.Payments.Infrastructure.Command;

namespace MAS.Payments.Commands
{
    public class AddUserSettingCommand: ICommand
    {
        public string Name { get; set; }

        public string RawValue { get; set; }

        public string TypeName { get; set; }

        private AddUserSettingCommand(string name, string rawValue, SettingDataValueType dataValueType)
        {
            Name = name;
            RawValue = rawValue;
            TypeName = dataValueType.ToString();
        }

        public AddUserSettingCommand(string name, int value)
            : this(name, value.ToString(), SettingDataValueType.Number)
        {
        }

        public AddUserSettingCommand(string name, bool value)
            : this(name, value.ToString(), SettingDataValueType.Boolean)
        {
        }

        public AddUserSettingCommand(string name, string value)
            : this(name, value, SettingDataValueType.String)
        {
        }
    }
}