namespace MAS.Payments.Models
{
    using MAS.Payments.Commands;

    public class DeleteFileRequest
    {
        public long PaymentId { get; set; }

        public DeleteRelatedFileMode Mode { get; set; }
    }
}
