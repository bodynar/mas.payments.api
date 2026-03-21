namespace MAS.Payments.Commands
{

    using MAS.Payments.Infrastructure.Command;

    public class DeletePaymentTypeCommand(
        Guid paymentTypeId
    ) : ICommand
    {
        public Guid PaymentTypeId { get; set; } = paymentTypeId;
    }
}
