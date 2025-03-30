namespace MAS.Payments.Commands
{
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
            _ =
                await PaymentTypeRepository.Get(command.PaymentTypeId)
                ?? throw new CommandExecutionException(CommandType,
                    $"Payment type with id {command.PaymentTypeId} doesn't exist");

            dynamic updatedModel = new
            {
                command.Amount,
                command.Date,
                command.Description,
                command.PaymentTypeId,
            };

            if (command.ReceiptFile?.Length > 0)
            {
                var createCommand = new CreatePdfDocumentCommand(command.ReceiptFile);

                await CommandProcessor.Execute(createCommand);

                updatedModel.Receipt = createCommand.PdfDocument;
            }

            if (command.CheckFile?.Length > 0)
            {
                var createCommand = new CreatePdfDocumentCommand(command.CheckFile);

                await CommandProcessor.Execute(createCommand);

                updatedModel.Check = createCommand.PdfDocument;
            }

            await Repository.Update(command.Id, updatedModel);
        }
    }
}