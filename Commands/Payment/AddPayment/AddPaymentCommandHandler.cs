namespace MAS.Payments.Commands
{
    using System.Threading.Tasks;

    using MAS.Payments.DataBase;
    using MAS.Payments.DataBase.Access;
    using MAS.Payments.Infrastructure;
    using MAS.Payments.Infrastructure.Command;
    using MAS.Payments.Infrastructure.Exceptions;

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

        public override async Task HandleAsync(AddPaymentCommand command)
        {
            _ =
                await PaymentTypeRepository.Get(command.PaymentTypeId)
                ?? throw new CommandExecutionException(CommandType,
                    $"Payment type with id {command.PaymentTypeId} doesn't exist");

            var payment = new Payment
            {
                Amount = command.Amount,
                Date = command.Date,
                Description = command.Description,
                PaymentTypeId = command.PaymentTypeId,
            };

            if (command.ReceiptFile?.Length > 0)
            {
                var createCommand = new CreatePdfDocumentCommand(command.ReceiptFile);

                await CommandProcessor.Execute(createCommand);

                payment.Receipt = createCommand.PdfDocument;
            }

            if (command.Check?.Length > 0)
            {
                var createCommand = new CreatePdfDocumentCommand(command.Check);

                await CommandProcessor.Execute(createCommand);

                payment.Check = createCommand.PdfDocument;
            }

            await Repository.Add(payment);
        }
    }
}