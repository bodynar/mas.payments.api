namespace MAS.Payments.Commands
{
    using MAS.Payments.Infrastructure.Command;

    using Microsoft.AspNetCore.Http;

    public class AttachFileCommand(
        IFormFile file,
        long paymentId,
        string fieldName
    ) : ICommand
    {
        public IFormFile File { get; } = file;

        public long PaymentId { get; } = paymentId;

        public string FieldName { get; } = fieldName;
    }
}
