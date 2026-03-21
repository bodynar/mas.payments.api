namespace MAS.Payments.Queries
{
    public class GetPaymentFileResponse
    {
        public string FileName { get; set; }

        public string ContentType { get; set; }

        public byte[] Data { get; set; }
    }
}
