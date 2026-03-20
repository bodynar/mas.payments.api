namespace MAS.Payments.Commands
{
    using System.Linq;
    using System.Threading.Tasks;

    using MAS.Payments.DataBase;
    using MAS.Payments.DataBase.Access;
    using MAS.Payments.Infrastructure;
    using MAS.Payments.Infrastructure.Command;
    using MAS.Payments.Infrastructure.Exceptions;

    public class AddPaymentGroupCommandHandler : BaseCommandHandler<AddPaymentGroupCommand>
    {
        private IRepository<Payment> Repository { get; }

        private IRepository<PaymentType> PaymentTypeRepository { get; }

        public AddPaymentGroupCommandHandler(IResolver resolver)
            : base(resolver)
        {
            Repository = GetRepository<Payment>();
            PaymentTypeRepository = GetRepository<PaymentType>();
        }

        public override async Task HandleAsync(AddPaymentGroupCommand command)
        {
            var paymentTypesIds = command.Payments.Select(x => x.PaymentTypeId).ToArray();

            var paymentTypes =
                PaymentTypeRepository
                    .Where(new CommonSpecification.IdIn<PaymentType>(paymentTypesIds))
                    .Select(x => x.Id)
                    .ToArray();

            var notValidTypes = paymentTypesIds.Except(paymentTypes);

            if (notValidTypes.Any())
            {
                throw new CommandExecutionException(CommandType, $"Payment types with ids [{string.Join(",", notValidTypes)}] doesn't exists");
            }

            var payments =
                command.Payments
                    .Select(x => new Payment
                    {
                        Date = command.Date,
                        Amount = x.Amount,
                        PaymentTypeId = x.PaymentTypeId,
                        Description = x.Description,
                    });

            await Repository.AddRange(payments);
        }
    }
}
