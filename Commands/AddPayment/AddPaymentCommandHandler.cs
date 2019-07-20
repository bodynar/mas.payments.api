using MAS.Payments.DataBase;
using MAS.Payments.Infrastructure;
using MAS.Payments.Infrastructure.Command;
using MAS.Payments.Infrastructure.Exceptions;

namespace MAS.Payments.Commands
{
    internal class AddPaymentCommandHandler : BaseCommandHandler<AddPaymentCommand>
    {
        public AddPaymentCommandHandler(
            IResolver resolver
        ) : base(resolver)
        {
        }

        public override void Handle(AddPaymentCommand command)
        {
            var paymentType =
                GetRepository<PaymentType>().Get(command.PaymentTypeId);

            if (paymentType == null)
            {
                throw new CommandExecutionException(
                    $"Payment type with id {command.PaymentTypeId} doesn't exist");
            }

            var payment = new Payment
            {
                Amount = command.Amount,
                Date = command.Date,
                Description = command.Description,
                PaymentTypeId = command.PaymentTypeId
            };

            GetRepository<Payment>().Add(payment);
        }
    }
}