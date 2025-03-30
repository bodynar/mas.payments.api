namespace MAS.Payments.Commands
{
    using MAS.Payments.Infrastructure.Command;

    public enum DeleteRelatedFileMode
    {
        None = 0,

        Receipt = 1,

        Check = 2,

        Both = 3,
    }

    public class DeleteRelatedFileCommand(
        long paymentId,
        DeleteRelatedFileMode mode = 0
    ) : ICommand
    {
        public long PaymentId { get; } = paymentId;

        public DeleteRelatedFileMode Mode { get; } = mode;
    }
}
