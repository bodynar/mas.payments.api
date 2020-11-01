namespace MAS.Payments.Commands
{

    using MAS.Payments.Infrastructure.Command;

    public class DeletePaymentTypeCommand : ICommand
    {
        public long PaymentTypeId { get; set; }

        public DeletePaymentTypeCommand(long paymentTypeId)
        {
            PaymentTypeId = paymentTypeId;
        }
    }
}