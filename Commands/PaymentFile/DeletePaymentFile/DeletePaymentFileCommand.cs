namespace MAS.Payments.Commands
{
    using MAS.Payments.Infrastructure.Command;

    public class DeletePaymentFileCommand(
        long fileId
    ) : ICommand
    {
        public long FileId { get; } = fileId;
    }
}
