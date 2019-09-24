using MAS.Payments.DataBase;
using MAS.Payments.DataBase.Access;
using MAS.Payments.Infrastructure;
using MAS.Payments.Infrastructure.Command;
using MAS.Payments.Infrastructure.Exceptions;

namespace MAS.Payments.Commands
{
    internal class UpdatePaymentCommandHandler : BaseCommandHandler<UpdatePaymentCommand>
    {
        private IRepository<Payment> Repository { get; }

        private IRepository<PaymentType> PaymentTypeRepository { get; }
        public UpdatePaymentCommandHandler(
            IResolver resolver
        ) : base(resolver)
        {
            Repository = GetRepository<Payment>();
            PaymentTypeRepository = GetRepository<PaymentType>();
        }

        public override void Handle(UpdatePaymentCommand command)
        {
            var paymentType =
                PaymentTypeRepository.Get(command.PaymentTypeId);

            if (paymentType == null)
            {
                throw new CommandExecutionException(
                    $"Payment type with id {command.PaymentTypeId} doesn't exist");
            }

            Repository.Update(command.Id, new
            {
                Amount = command.Amount,
                Date = command.Date,
                Description = command.Description,
                PaymentTypeId = command.PaymentTypeId,
            });
        }
    }
}