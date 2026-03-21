namespace MAS.Payments.Commands
{
    using MAS.Payments.Infrastructure.Command;

    public class DeletePaymentGroupCommand(
        Guid paymentGroupId
    ) : ICommand
    {
        public Guid PaymentGroupId { get; } = paymentGroupId;
    }
}
