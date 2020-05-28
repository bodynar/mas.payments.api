using System;
using System.Collections.Generic;
using System.Linq;

using MAS.Payments.DataBase;
using MAS.Payments.DataBase.Access;
using MAS.Payments.Infrastructure;
using MAS.Payments.Infrastructure.Command;
using MAS.Payments.Infrastructure.Exceptions;

namespace MAS.Payments.Commands
{
    internal class UpdateUserSettingsCommandHandler : BaseCommandHandler<UpdateUserSettingsCommand>
    {
        private IRepository<UserSettings> Repository { get; }

        public UpdateUserSettingsCommandHandler(IResolver resolver)
            : base(resolver)
        {
            Repository = GetRepository<UserSettings>();
        }

        public override void Handle(UpdateUserSettingsCommand command)
        {
            var errors = new List<string>();

            foreach (var settingValue in command.UpdatedSettings)
            {
                var setting = Repository.Get(settingValue.Id);

                var isSettingValueCorrect = IsSettingValueCorrect(setting, settingValue.RawValue);

                if (!isSettingValueCorrect)
                {
                    errors.Add($"Setting \"{setting.DisplayName}\" value: \"{settingValue.RawValue}\" isn't instance of type \"{setting.TypeName}\".");
                    continue;
                }

                Repository.Update(settingValue.Id, settingValue);
            }

            if (errors.Any())
            {
                throw new CommandExecutionException(CommandType, string.Join(", ", errors));
            }
        }

        private bool IsSettingValueCorrect(UserSettings setting, string rawValue)
        {
            var result = true;

            switch (setting.TypeName)
            {
                case "Boolean":
                {
                    result = Boolean.TryParse(rawValue, out var _);
                    break;
                }
                case "Number":
                {
                    result = Int16.TryParse(rawValue, out var _);
                    break;
                }
                case "String":
                {
                    break;
                }
                default:
                    throw new CommandExecutionException(CommandType, $"Setting {setting.DisplayName} has incorrect dataValueType: {setting.TypeName}");
            }

            return result;
        }
    }
}