namespace MAS.Payments.Commands
{
    using System.Collections.Generic;

    using MAS.Payments.Infrastructure.Command;

    public class DeletePaymentFilesCommand(
        IEnumerable<long> fileIds
    ) : ICommand
    {
        public IEnumerable<long> FileIds { get; } = fileIds;
    }
}
