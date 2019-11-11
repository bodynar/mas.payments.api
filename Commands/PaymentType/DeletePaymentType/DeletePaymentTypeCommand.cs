
using MAS.Payments.Infrastructure.Command;

namespace MAS.Payments.Commands
{
    public class DeletePaymentTypeCommand : UserCommand
    {
        public long PaymentTypeId { get; set; }

        public DeletePaymentTypeCommand(long userId, long paymentTypeId)
            : base(userId)
        {
            PaymentTypeId = paymentTypeId;
        }
    }
}