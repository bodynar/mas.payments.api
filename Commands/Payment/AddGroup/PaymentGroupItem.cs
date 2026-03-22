namespace MAS.Payments.Commands
{
    public class PaymentGroupItem(
        double amount,
        Guid paymentTypeId,
        string description
    )
    {
        public double Amount { get; } = amount;

        public Guid PaymentTypeId { get; } = paymentTypeId;

        public string Description { get; } = description;
    }
}
