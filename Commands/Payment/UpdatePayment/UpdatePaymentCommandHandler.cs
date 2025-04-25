namespace MAS.Payments.Commands
{
    using System;
    using System.Threading.Tasks;

    using MAS.Payments.DataBase;
    using MAS.Payments.DataBase.Access;
    using MAS.Payments.Infrastructure;
    using MAS.Payments.Infrastructure.Command;
    using MAS.Payments.Infrastructure.Exceptions;

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

        public override async Task HandleAsync(UpdatePaymentCommand command)
        {
            if (command.Id == default)
            {
                throw new ArgumentException("Payment id is not set", nameof(command));
            }

            _ =
                await PaymentTypeRepository.Get(command.PaymentTypeId)
                ?? throw new CommandExecutionException(CommandType,
                    $"Payment type with id {command.PaymentTypeId} doesn't exist");

            var payment = new PaymentModel
            {
                Amount = command.Amount,
                Date = command.Date,
                Description = command.Description,
                PaymentTypeId = command.PaymentTypeId,
            };

            await Repository.Update(command.Id, payment);
        }
    }

    internal class PaymentModel
    {
        public double Amount { get; set; }
        public DateTime? Date { get; set; }
        public string Description { get; set; }
        public long PaymentTypeId { get; set; }
    }
}