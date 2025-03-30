namespace MAS.Payments.Commands
{
    using MAS.Payments.Infrastructure.Command;

    public enum DeletePdfDocumentTarget
    {
        None = 0,

        Receipent = 1,

        Check = 2,
    }

    public class DeletePdfDocumentCommand(
        long documentId,
        long paymentId,
        DeletePdfDocumentTarget target
    ) : ICommand
    {
        public long DocumentId { get; } = documentId;

        public long PaymentId { get; } = paymentId;

        public DeletePdfDocumentTarget Target { get; } = target;
    }
}
