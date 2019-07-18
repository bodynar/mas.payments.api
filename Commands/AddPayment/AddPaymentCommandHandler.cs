using MAS.Payments.DataBase;
using MAS.Payments.Infrastructure;
using MAS.Payments.Infrastructure.Command;
using MAS.Payments.Queries;

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
                QueryProcessor.Execute(
                    new GetEntityQuery<PaymentType>(command.PaymentTypeID));

            var payment = new Payment
            {
                Amount = command.Amount,
                Date = command.Date,
                Description = command.Description,
                PaymentType = paymentType
            };

            GetRepository<Payment>().Add(payment);
        }
    }
}