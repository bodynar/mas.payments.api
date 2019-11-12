
using MAS.Payments.Infrastructure.Command;

namespace MAS.Payments.Commands
{
    public class DeletePaymentCommand : ICommand
    {
        public long PaymentId { get; set; }

        public DeletePaymentCommand(long paymentId)
        {
            PaymentId = paymentId;
        }
    }
}