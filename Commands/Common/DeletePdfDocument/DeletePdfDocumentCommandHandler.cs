﻿namespace MAS.Payments.Commands
{
    using System.Threading.Tasks;

    using MAS.Payments.DataBase;
    using MAS.Payments.DataBase.Access;
    using MAS.Payments.Infrastructure;
    using MAS.Payments.Infrastructure.Command;
    using MAS.Payments.Infrastructure.Exceptions;
    using MAS.Payments.Infrastructure.Specification;

    internal class DeletePdfDocumentCommandHandler : BaseCommandHandler<DeletePdfDocumentCommand>
    {
        private IRepository<Payment> Repository { get; }

        private IRepository<PdfDocument> PdfDocumentRepository { get; }

        public DeletePdfDocumentCommandHandler(
            IResolver resolver
        ) : base(resolver)
        {
            Repository = GetRepository<Payment>();
            PdfDocumentRepository = GetRepository<PdfDocument>();
        }

        public override async Task HandleAsync(DeletePdfDocumentCommand command)
        {
            if (command.Target == DeletePdfDocumentTarget.None)
            {
                throw new CommandExecutionException(CommandType, "Target cannot be None!");
            }

            var fileSpecificcation = command.Target == DeletePdfDocumentTarget.Receipt
                ? new CommonSpecification<Payment>(x => x.ReceiptId == command.DocumentId)
                : new CommonSpecification<Payment>(x => x.CheckId == command.DocumentId);

            var paymentIdSpecification = new CommonSpecification<Payment>(x => x.Id == command.PaymentId);

            var hasOtherLinks = await Repository.Any(fileSpecificcation && paymentIdSpecification);

            if (!hasOtherLinks)
            {
                await PdfDocumentRepository.Delete(command.DocumentId);
            }
        }
    }
}
