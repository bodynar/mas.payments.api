namespace MAS.Payments.Commands
{

    using MAS.Payments.Infrastructure.Command;

    public class DeletePaymentCommand(
        Guid paymentId
    ) : ICommand
    {
        public Guid PaymentId { get; set; } = paymentId;
    }
}
