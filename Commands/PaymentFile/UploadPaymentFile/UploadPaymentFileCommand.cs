namespace MAS.Payments.Commands
{
    using MAS.Payments.Infrastructure.Command;

    public class UploadPaymentFileCommand(
        string fileName,
        string contentType,
        byte[] data,
        long? paymentId,
        long? paymentGroupId
    ) : ICommand
    {
        public string FileName { get; } = fileName;

        public string ContentType { get; } = contentType;

        public byte[] Data { get; } = data;

        public long? PaymentId { get; } = paymentId;

        public long? PaymentGroupId { get; } = paymentGroupId;
    }
}
