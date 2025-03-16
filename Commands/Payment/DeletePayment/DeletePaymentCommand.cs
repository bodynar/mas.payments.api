namespace MAS.Payments.Commands
{

    using MAS.Payments.Infrastructure.Command;

    public class DeletePaymentCommand(
        long paymentId
    ) : ICommand
    {
        public long PaymentId { get; set; } = paymentId;
    }
}