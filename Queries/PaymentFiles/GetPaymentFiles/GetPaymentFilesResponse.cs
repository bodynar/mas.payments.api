namespace MAS.Payments.Queries
{
    using System;

    public class GetPaymentFilesResponse
    {
        public long Id { get; set; }

        public string FileName { get; set; }

        public long FileSize { get; set; }

        public DateTime UploadedAt { get; set; }

        public long? PaymentId { get; set; }

        public long? PaymentGroupId { get; set; }

        public string LinkedEntity { get; set; }
    }
}
