namespace MAS.Payments.Commands
{

    using MAS.Payments.Infrastructure.Command;

    public class DeletePaymentCommand : ICommand
    {
        public long PaymentId { get; set; }

        public DeletePaymentCommand(long paymentId)
        {
            PaymentId = paymentId;
        }
    }
}