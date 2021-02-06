namespace MAS.Payments.Commands
{
    public class PaymentGroup
    {
        public double Amount { get; }

        public long PaymentTypeId { get; }

        public string Description { get; }

        public PaymentGroup(double amount, long paymentTypeId, string description)
        {
            Amount = amount;
            PaymentTypeId = paymentTypeId;
            Description = description;
        }
    }
}
