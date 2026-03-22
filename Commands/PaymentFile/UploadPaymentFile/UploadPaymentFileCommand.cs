namespace MAS.Payments.Commands
{
    using MAS.Payments.Infrastructure.Command;

    public class UploadPaymentFileCommand(
        string fileName,
        string contentType,
        byte[] data,
        Guid? paymentId,
        Guid? paymentGroupId
    ) : ICommand
    {
        public string FileName { get; } = fileName;

        public string ContentType { get; } = contentType;

        public byte[] Data { get; } = data;

        public Guid? PaymentId { get; } = paymentId;

        public Guid? PaymentGroupId { get; } = paymentGroupId;
    }
}
