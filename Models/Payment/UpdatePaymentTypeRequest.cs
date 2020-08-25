namespace MAS.Payments.Models
{
    public class UpdatePaymentTypeRequest
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Company { get; set; }

        public string Color { get; set; }
    }
}