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
            if (!command.Payments.Any())
            {
                return;
            }

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

            PdfDocument receiptFile = null;

            if (command.ReceiptFile?.Length > 0)
            {
                var createCommand = new CreatePdfDocumentCommand(command.ReceiptFile);

                await CommandProcessor.Execute(createCommand);

                receiptFile = createCommand.PdfDocument;
            }

            PdfDocument checkFile = null;

            if (command.Check?.Length > 0)
            {
                var createCommand = new CreatePdfDocumentCommand(command.Check);

                await CommandProcessor.Execute(createCommand);

                checkFile = createCommand.PdfDocument;
            }

            var payments =
                command.Payments
                    .Select(x => new Payment
                    {
                        Date = command.Date,
                        Amount = x.Amount,
                        PaymentTypeId = x.PaymentTypeId,
                        Description = x.Description,

                        Receipt = receiptFile,
                        Check = checkFile,
                    });

            await Repository.AddRange(payments);
        }
    }
}
