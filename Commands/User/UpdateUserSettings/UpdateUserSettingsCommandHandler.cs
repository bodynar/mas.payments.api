namespace MAS.Payments.Commands
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using MAS.Payments.DataBase;
    using MAS.Payments.DataBase.Access;
    using MAS.Payments.Infrastructure;
    using MAS.Payments.Infrastructure.Command;
    using MAS.Payments.Infrastructure.Exceptions;

    internal class UpdateUserSettingsCommandHandler : BaseCommandHandler<UpdateUserSettingsCommand>
    {
        private IRepository<UserSettings> Repository { get; }

        public UpdateUserSettingsCommandHandler(IResolver resolver)
            : base(resolver)
        {
            Repository = GetRepository<UserSettings>();
        }

        public override async Task HandleAsync(UpdateUserSettingsCommand command)
        {
            var errors = new List<string>();

            foreach (var settingValue in command.UpdatedSettings)
            {
                var setting = await Repository.Get(settingValue.Id);

                var isSettingValueCorrect = IsSettingValueCorrect(setting, settingValue.RawValue);

                if (!isSettingValueCorrect)
                {
                    errors.Add($"Setting \"{setting.DisplayName}\" value: \"{settingValue.RawValue}\" isn't instance of type \"{setting.TypeName}\".");
                    continue;
                }

                await Repository.Update(settingValue.Id, settingValue);
            }

            if (errors.Count != 0)
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
                    result = bool.TryParse(rawValue, out var _);
                    break;
                }
                case "Number":
                {
                    result = short.TryParse(rawValue, out var _);
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