namespace MAS.Payments.Commands
{
    using System.Collections.Generic;

    using MAS.Payments.Infrastructure.Command;

    public class DeletePaymentFilesCommand(
        IEnumerable<Guid> fileIds
    ) : ICommand
    {
        public IEnumerable<Guid> FileIds { get; } = fileIds;
    }
}
