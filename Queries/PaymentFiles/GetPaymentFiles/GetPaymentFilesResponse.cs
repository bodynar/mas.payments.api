namespace MAS.Payments.Queries
{
    using System;

    public class GetPaymentFilesResponse
    {
        public Guid Id { get; set; }

        public string FileName { get; set; }

        public long FileSize { get; set; }

        public DateTime UploadedAt { get; set; }

        public Guid? PaymentId { get; set; }

        public Guid? PaymentGroupId { get; set; }

        public string LinkedEntity { get; set; }
    }
}
