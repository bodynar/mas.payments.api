namespace MAS.Payments.Commands
{
    using System.Threading.Tasks;

    using MAS.Payments.DataBase;
    using MAS.Payments.DataBase.Access;
    using MAS.Payments.Infrastructure;
    using MAS.Payments.Infrastructure.Command;
    using MAS.Payments.Infrastructure.Specification;

    internal class DeletePaymentCommandHandler : BaseCommandHandler<DeletePaymentCommand>
    {
        private IRepository<Payment> Repository { get; }

        private IRepository<PdfDocument> PdfDocumentRepository { get; }

        public DeletePaymentCommandHandler(
            IResolver resolver
        ) : base(resolver)
        {
            Repository = GetRepository<Payment>();
            PdfDocumentRepository = GetRepository<PdfDocument>();
        }

        public override async Task HandleAsync(DeletePaymentCommand command)
        {
            var payment = await Repository.Get(command.PaymentId);

            if (payment == default)
            {
                return;
            }

            if (payment.ReceiptId.HasValue)
            {
                var hasOtherLinks = await Repository.Any(
                    new CommonSpecification<Payment>(
                        x =>
                            x.ReceiptId == payment.ReceiptId.Value
                            && x.Id != command.PaymentId
                    )
                );

                if (!hasOtherLinks)
                {
                    await PdfDocumentRepository.Delete(payment.ReceiptId.Value);
                }
            }

            if (payment.CheckId.HasValue)
            {
                var hasOtherLinks = await Repository.Any(
                    new CommonSpecification<Payment>(
                        x =>
                            x.CheckId == payment.CheckId.Value
                            && x.Id != command.PaymentId
                    )
                );

                if (!hasOtherLinks)
                {
                    await PdfDocumentRepository.Delete(payment.CheckId.Value);
                }
            }
            
            await Repository.Delete(command.PaymentId);
        }
    }
}