namespace MAS.Payments.Commands
{
    public class PaymentGroup(
        double amount,
        long paymentTypeId,
        string description
    )
    {
        public double Amount { get; } = amount;

        public long PaymentTypeId { get; } = paymentTypeId;

        public string Description { get; } = description;
    }
}
