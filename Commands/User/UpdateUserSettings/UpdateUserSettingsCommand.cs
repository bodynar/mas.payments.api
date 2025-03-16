namespace MAS.Payments.Commands
{
    using System.Collections.Generic;

    using MAS.Payments.Infrastructure.Command;
    using MAS.Payments.Models;

    public class UpdateUserSettingsCommand(
        IEnumerable<UpdateUserSettingsRequest> updatedSettings
    ) : ICommand
    {
        public IEnumerable<UpdateUserSettingsRequest> UpdatedSettings { get; } = updatedSettings ?? [];
    }
}