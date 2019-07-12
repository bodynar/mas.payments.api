using MAS.Payments.DataBase;
using MAS.Payments.Infrastructure;
using MAS.Payments.Infrastructure.Command;

namespace MAS.Payments.Commands
{
    internal class AddPaymentTypeCommandHandler : BaseCommandHandler<AddPaymentTypeCommand>
    {
        public AddPaymentTypeCommandHandler(
            IResolver resolver
        ) : base(resolver)
        {
        }

        public override void Handle(AddPaymentTypeCommand command)
        {
            var paymentType = new PaymentType
            {
                Name = command.Name,
                Description = command.Description,
                Company = command.Company
            };

            GetRepository<PaymentType>().Add(paymentType);
        }
    }
}