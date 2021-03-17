namespace MAS.Payments.Commands
{
    using System.Linq;

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

        public override void Handle(AddPaymentGroupCommand command)
        {
            var notValidTypes = 
                command.Payments
                    .Where(x => PaymentTypeRepository.Get(x.PaymentTypeId) == null)
                    .Select(x => x.PaymentTypeId);

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

            Repository.AddRange(payments);
        }
    }
}
