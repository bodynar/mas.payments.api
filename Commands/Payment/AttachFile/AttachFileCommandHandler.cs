namespace MAS.Payments.Commands
{
    using System;
    using System.Threading.Tasks;

    using MAS.Payments.DataBase;
    using MAS.Payments.DataBase.Access;
    using MAS.Payments.Infrastructure;
    using MAS.Payments.Infrastructure.Command;

    internal class AttachFileCommandHandler : BaseCommandHandler<AttachFileCommand>
    {
        private IRepository<Payment> Repository { get; }

        public AttachFileCommandHandler(
            IResolver resolver
        ) : base(resolver)
        {
            Repository = GetRepository<Payment>();
        }

        public override async Task HandleAsync(AttachFileCommand command)
        {
            if (command.File == null || command.File.Length == 0)
            {
                throw new ArgumentException("File is empty");
            }

            if (command.FieldName != nameof(Payment.Receipt) && command.FieldName != nameof(Payment.Check))
            {
                throw new ArgumentException("field name is not presented in Payment entity as file field");
            }

            var createCommand = new CreatePdfDocumentCommand(command.File);

            await CommandProcessor.Execute(createCommand);

            dynamic paymentModel = command.FieldName == nameof(Payment.Receipt)
                ? new { ReceiptId = createCommand.PdfDocument.Id }
                : new { CheckId = createCommand.PdfDocument.Id }
            ;

            await Repository.Update(command.PaymentId, paymentModel);
        }
    }
}
