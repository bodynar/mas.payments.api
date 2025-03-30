namespace MAS.Payments.Commands
{
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using MAS.Payments.DataBase;
    using MAS.Payments.DataBase.Access;
    using MAS.Payments.DataBase.Models;
    using MAS.Payments.Infrastructure;
    using MAS.Payments.Infrastructure.Command;
    using MAS.Payments.Infrastructure.Exceptions;

    using Microsoft.AspNetCore.Http;

    public class AddPaymentGroupCommandHandler : BaseCommandHandler<AddPaymentGroupCommand>
    {
        private IRepository<Payment> Repository { get; }

        private IRepository<PaymentType> PaymentTypeRepository { get; }

        private IRepository<PdfDocument> PdfDocumentRepository { get; }

        public AddPaymentGroupCommandHandler(IResolver resolver)
            : base(resolver)
        {
            Repository = GetRepository<Payment>();
            PaymentTypeRepository = GetRepository<PaymentType>();
            PdfDocumentRepository = GetRepository<PdfDocument>();
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

            var receiptFile = command.ReceiptFile?.Length > 0
                ? await AttachFileAsync(command.ReceiptFile)
                : null;

            var checkFile = command.Check?.Length > 0
                ? await AttachFileAsync(command.Check)
                : null;

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
