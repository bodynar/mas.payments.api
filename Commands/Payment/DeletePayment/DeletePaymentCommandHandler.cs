namespace MAS.Payments.Commands
{
    using System.Threading.Tasks;

    using MAS.Payments.DataBase;
    using MAS.Payments.DataBase.Access;
    using MAS.Payments.Infrastructure;
    using MAS.Payments.Infrastructure.Command;

    internal class DeletePaymentCommandHandler : BaseCommandHandler<DeletePaymentCommand>
    {
        private IRepository<Payment> Repository { get; }

        public DeletePaymentCommandHandler(
            IResolver resolver
        ) : base(resolver)
        {
            Repository = GetRepository<Payment>();
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
                await CommandProcessor.Execute(new DeletePdfDocumentCommand(payment.ReceiptId.Value, command.PaymentId, DeletePdfDocumentTarget.Receipt));
            }

            if (payment.CheckId.HasValue)
            {
                await CommandProcessor.Execute(new DeletePdfDocumentCommand(payment.CheckId.Value, command.PaymentId, DeletePdfDocumentTarget.Check));
            }
            
            await Repository.Delete(command.PaymentId);
        }
    }
}