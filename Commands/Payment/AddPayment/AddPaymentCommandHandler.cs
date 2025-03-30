namespace MAS.Payments.Commands
{
    using System.IO;
    using System.Threading.Tasks;

    using MAS.Payments.DataBase;
    using MAS.Payments.DataBase.Access;
    using MAS.Payments.Infrastructure;
    using MAS.Payments.Infrastructure.Command;
    using MAS.Payments.Infrastructure.Exceptions;

    using Microsoft.AspNetCore.Http;

    internal class AddPaymentCommandHandler : BaseCommandHandler<AddPaymentCommand>
    {
        private IRepository<Payment> Repository { get; }

        private IRepository<PaymentType> PaymentTypeRepository { get; }

        private IRepository<PdfDocument> PdfDocumentRepository { get; }

        public AddPaymentCommandHandler(
            IResolver resolver
        ) : base(resolver)
        {
            Repository = GetRepository<Payment>();
            PaymentTypeRepository = GetRepository<PaymentType>();
            PdfDocumentRepository = GetRepository<PdfDocument>();
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
                var file = await AttachFileAsync(command.ReceiptFile);

                payment.Receipt = file;
            }

            if (command.Check?.Length > 0)
            {
                var file = await AttachFileAsync(command.Check);

                payment.Receipt = file;
            }

            await Repository.Add(payment);
        }

        private async Task<PdfDocument> AttachFileAsync(IFormFile file)
        {
            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);

            var document = new PdfDocument
            {
                Name = file.FileName,
                FileData = memoryStream.ToArray()
            };

            await PdfDocumentRepository.Add(document);

            return document;
        }
    }
}