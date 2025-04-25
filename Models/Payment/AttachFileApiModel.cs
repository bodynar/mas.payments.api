namespace MAS.Payments.Models
{
    using Microsoft.AspNetCore.Http;

    public class AttachFileApiModel
    {
        public IFormFile File { get; set; }

        public long PaymentId { get; set; }

        public string FieldName { get; set; }
    }
}
