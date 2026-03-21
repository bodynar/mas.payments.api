namespace MAS.Payments.Commands
{
    using System.Linq;
    using System.Threading.Tasks;

    using MAS.Payments.DataBase;
    using MAS.Payments.DataBase.Access;
    using MAS.Payments.Infrastructure;
    using MAS.Payments.Infrastructure.Command;

    using Microsoft.EntityFrameworkCore;

    internal class DeletePaymentFilesCommandHandler : BaseCommandHandler<DeletePaymentFilesCommand>
    {
        private IRepository<PaymentFile> Repository { get; }

        public DeletePaymentFilesCommandHandler(
            IResolver resolver
        ) : base(resolver)
        {
            Repository = GetRepository<PaymentFile>();
        }

        public override async Task HandleAsync(DeletePaymentFilesCommand command)
        {
            var files = await Repository
                .Where(new CommonSpecification.IdIn<PaymentFile>(command.FileIds))
                .ToListAsync();

            await Repository.DeleteRange(files);
        }
    }
}
