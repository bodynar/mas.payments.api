using System.Collections.Generic;

using MAS.Payments.Infrastructure.Command;
using MAS.Payments.Models;

namespace MAS.Payments.Commands
{
    public class UpdateUserSettingsCommand: ICommand
    {
        public IEnumerable<UpdateUserSettingsRequest> UpdatedSettings { get; } // model isolated

        public UpdateUserSettingsCommand(IEnumerable<UpdateUserSettingsRequest> updatedSettings)
        {
            UpdatedSettings = updatedSettings;
        }
    }
}