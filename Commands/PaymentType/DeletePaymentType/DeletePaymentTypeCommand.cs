
using MAS.Payments.Infrastructure.Command;

namespace MAS.Payments.Commands
{
    public class DeletePaymentTypeCommand : ICommand
    {
        public long PaymentTypeId { get; set; }

        public DeletePaymentTypeCommand(long paymentTypeId)
        {
            PaymentTypeId = paymentTypeId;
        }
    }
}