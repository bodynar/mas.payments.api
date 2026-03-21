namespace MAS.Payments.Commands
{
    using System.Threading.Tasks;

    using MAS.Payments.DataBase;
    using MAS.Payments.DataBase.Access;
    using MAS.Payments.Infrastructure;
    using MAS.Payments.Infrastructure.Command;

    internal class DeletePaymentFileCommandHandler : BaseCommandHandler<DeletePaymentFileCommand>
    {
        private IRepository<PaymentFile> Repository { get; }

        public DeletePaymentFileCommandHandler(
            IResolver resolver
        ) : base(resolver)
        {
            Repository = GetRepository<PaymentFile>();
        }

        public override async Task HandleAsync(DeletePaymentFileCommand command)
        {
            await Repository.Delete(command.FileId);
        }
    }
}
