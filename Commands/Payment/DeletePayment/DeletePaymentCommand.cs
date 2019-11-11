
using MAS.Payments.Infrastructure.Command;

namespace MAS.Payments.Commands
{
    public class DeletePaymentCommand : UserCommand
    {
        public long PaymentId { get; set; }

        public DeletePaymentCommand(long userId, long paymentId)
            : base(userId)
        {
            PaymentId = paymentId;
        }
    }
}