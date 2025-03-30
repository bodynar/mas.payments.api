namespace MAS.Payments.Commands
{
    using System.Threading.Tasks;

    using MAS.Payments.DataBase;
    using MAS.Payments.DataBase.Access;
    using MAS.Payments.Infrastructure;
    using MAS.Payments.Infrastructure.Command;
    using MAS.Payments.Infrastructure.Exceptions;

    internal class DeleteRelatedFileCommandHandler : BaseCommandHandler<DeleteRelatedFileCommand>
    {
        private IRepository<Payment> Repository { get; }

        public DeleteRelatedFileCommandHandler(
            IResolver resolver
        ) : base(resolver)
        {
            Repository = GetRepository<Payment>();
        }

        public override async Task HandleAsync(DeleteRelatedFileCommand command)
        {
            if (command.Mode == DeleteRelatedFileMode.None)
            {
                throw new CommandExecutionException(CommandType, "Mode cannot be None!");
            }

            var payment = await Repository.Get(command.PaymentId);

            var updatedModel = new PaymentFileDeleteData
            {
                ReceiptId = payment.ReceiptId,
                CheckId = payment.CheckId,
            };

            if (command.Mode == DeleteRelatedFileMode.Both
                || command.Mode == DeleteRelatedFileMode.Receipt
            )
            {
                var isReceiptDeleted = await DeleteFile(
                    payment.Id, payment.ReceiptId, DeletePdfDocumentTarget.Receipt
                );

                if (isReceiptDeleted)
                {
                    updatedModel.ReceiptId = null;
                }
            }

            if (command.Mode == DeleteRelatedFileMode.Both
                || command.Mode == DeleteRelatedFileMode.Check
            )
            {
                var isCheckDeleted = await DeleteFile(
                    payment.Id, payment.CheckId, DeletePdfDocumentTarget.Check
                );

                if (isCheckDeleted)
                {
                    updatedModel.CheckId = null;
                }
            }

            if (updatedModel.ReceiptId != null && updatedModel.CheckId != null)
            {
                return;
            }

            await Repository.Update(command.PaymentId, updatedModel);
        }

        private async Task<bool> DeleteFile(long paymentId, long? fileId, DeletePdfDocumentTarget target)
        {
            if (!fileId.HasValue)
            {
                return false;
            }

            await CommandProcessor.Execute(
                new DeletePdfDocumentCommand(
                    fileId.Value, paymentId, target
                )
            );

            return true;
        }

        private sealed class PaymentFileDeleteData
        {
            public long? ReceiptId { get; set; }

            public long? CheckId { get; set; }
        }
    }
}
