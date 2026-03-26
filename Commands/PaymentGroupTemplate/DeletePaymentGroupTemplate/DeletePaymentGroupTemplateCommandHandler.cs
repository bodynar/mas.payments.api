namespace MAS.Payments.Commands
{
    using System.Threading.Tasks;

    using MAS.Payments.DataBase;
    using MAS.Payments.DataBase.Access;
    using MAS.Payments.Infrastructure;
    using MAS.Payments.Infrastructure.Command;

    internal class DeletePaymentGroupTemplateCommandHandler : BaseCommandHandler<DeletePaymentGroupTemplateCommand>
    {
        private IRepository<PaymentGroupTemplate> TemplateRepository { get; }

        public DeletePaymentGroupTemplateCommandHandler(IResolver resolver)
            : base(resolver)
        {
            TemplateRepository = GetRepository<PaymentGroupTemplate>();
        }

        public override async Task HandleAsync(DeletePaymentGroupTemplateCommand command)
        {
            await TemplateRepository.Delete(command.Id);
        }
    }
}
