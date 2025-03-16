namespace MAS.Payments.Commands
{

    using MAS.Payments.Infrastructure.Command;

    public class DeletePaymentTypeCommand(
        long paymentTypeId
    ) : ICommand
    {
        public long PaymentTypeId { get; set; } = paymentTypeId;
    }
}