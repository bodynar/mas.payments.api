using MAS.Payments.DataBase;
using MAS.Payments.DataBase.Access;
using MAS.Payments.Infrastructure;
using MAS.Payments.Infrastructure.Command;
using MAS.Payments.Infrastructure.Exceptions;

namespace MAS.Payments.Commands
{
    internal class AddPaymentCommandHandler : BaseCommandHandler<AddPaymentCommand>
    {
        private IRepository<Payment> Repository { get; }

        private IRepository<PaymentType> PaymentTypeRepository { get; }
        public AddPaymentCommandHandler(
            IResolver resolver
        ) : base(resolver)
        {
            Repository = GetRepository<Payment>();
            PaymentTypeRepository = GetRepository<PaymentType>();
        }

        public override void Handle(AddPaymentCommand command)
        {
            var paymentType =
                PaymentTypeRepository.Get(command.PaymentTypeId);

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
                PaymentTypeId = command.PaymentTypeId,
                AuthorId = command.UserId
            };

            Repository.Add(payment);
        }
    }
}