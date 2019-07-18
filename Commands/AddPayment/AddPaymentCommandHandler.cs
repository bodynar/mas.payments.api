using MAS.Payments.DataBase;
using MAS.Payments.Infrastructure;
using MAS.Payments.Infrastructure.Command;

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
                GetRepository<PaymentType>().Get(command.PaymentTypeID);

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