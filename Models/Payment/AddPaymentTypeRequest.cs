namespace MAS.Payments.Models
{
    public class AddPaymentTypeRequest
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Company { get; set; }

        public string Color { get; set; }
    }
}