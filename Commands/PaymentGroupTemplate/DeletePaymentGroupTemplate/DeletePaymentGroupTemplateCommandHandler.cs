namespace MAS.Payments.Commands
{
    using System.Linq;
    using System.Threading.Tasks;

    using MAS.Payments.DataBase;
    using MAS.Payments.DataBase.Access;
    using MAS.Payments.Infrastructure;
    using MAS.Payments.Infrastructure.Command;
    using MAS.Payments.Infrastructure.Specification;

    using Microsoft.EntityFrameworkCore;

    internal class DeletePaymentGroupTemplateCommandHandler : BaseCommandHandler<DeletePaymentGroupTemplateCommand>
    {
        private IRepository<PaymentGroupTemplate> TemplateRepository { get; }

        private IRepository<PaymentGroupTemplateItem> TemplateItemRepository { get; }

        public DeletePaymentGroupTemplateCommandHandler(IResolver resolver)
            : base(resolver)
        {
            TemplateRepository = GetRepository<PaymentGroupTemplate>();
            TemplateItemRepository = GetRepository<PaymentGroupTemplateItem>();
        }

        public override async Task HandleAsync(DeletePaymentGroupTemplateCommand command)
        {
            var linkedItems =
                await TemplateItemRepository
                    .Where(new CommonSpecification<PaymentGroupTemplateItem>(
                        x => x.PaymentGroupTemplateId == command.Id))
                    .ToListAsync();

            await TemplateItemRepository.DeleteRange(linkedItems);

            await TemplateRepository.Delete(command.Id);
        }
    }
}
