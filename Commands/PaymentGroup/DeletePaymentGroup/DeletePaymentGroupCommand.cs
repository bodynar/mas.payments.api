namespace MAS.Payments.Commands
{
    using MAS.Payments.Infrastructure.Command;

    public class DeletePaymentGroupCommand(
        long paymentGroupId
    ) : ICommand
    {
        public long PaymentGroupId { get; } = paymentGroupId;
    }
}
