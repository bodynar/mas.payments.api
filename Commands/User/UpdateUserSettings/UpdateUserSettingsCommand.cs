namespace MAS.Payments.Commands
{
    using System.Collections.Generic;

    using MAS.Payments.Infrastructure.Command;
    using MAS.Payments.Models;

    public class UpdateUserSettingsCommand: ICommand
    {
        public IEnumerable<UpdateUserSettingsRequest> UpdatedSettings { get; } // model isolated

        public UpdateUserSettingsCommand(IEnumerable<UpdateUserSettingsRequest> updatedSettings)
        {
            UpdatedSettings = updatedSettings;
        }
    }
}