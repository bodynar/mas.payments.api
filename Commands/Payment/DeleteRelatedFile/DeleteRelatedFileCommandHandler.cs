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

            dynamic updatedModel = new { };

            switch (command.Mode)
            {
                case DeleteRelatedFileMode.Receipt:
                    await CommandProcessor.Execute(new DeletePdfDocumentCommand(payment.ReceiptId.Value, payment.Id, DeletePdfDocumentTarget.Receipent));

                    updatedModel.ReceiptId = null;
                    break;

                case DeleteRelatedFileMode.Check:
                    await CommandProcessor.Execute(new DeletePdfDocumentCommand(payment.CheckId.Value, payment.Id, DeletePdfDocumentTarget.Check));

                    updatedModel.CheckId = null;
                    break;

                case DeleteRelatedFileMode.Both:
                    await CommandProcessor.Execute(new DeletePdfDocumentCommand(payment.ReceiptId.Value, payment.Id, DeletePdfDocumentTarget.Receipent));
                    await CommandProcessor.Execute(new DeletePdfDocumentCommand(payment.CheckId.Value, payment.Id, DeletePdfDocumentTarget.Check));

                    updatedModel.ReceiptId = null;
                    updatedModel.CheckId = null;

                    break;
            }

            await Repository.Update(command.PaymentId, updatedModel);
        }
    }
}
