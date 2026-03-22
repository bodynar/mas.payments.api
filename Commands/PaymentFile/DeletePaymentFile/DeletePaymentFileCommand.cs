namespace MAS.Payments.Commands
{
    using MAS.Payments.Infrastructure.Command;

    public class DeletePaymentFileCommand(
        Guid fileId
    ) : ICommand
    {
        public Guid FileId { get; } = fileId;
    }
}
