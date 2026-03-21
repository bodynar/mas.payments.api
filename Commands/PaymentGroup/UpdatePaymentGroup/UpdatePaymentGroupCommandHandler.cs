namespace MAS.Payments.Commands
{
    using System.Threading.Tasks;

    using MAS.Payments.DataBase;
    using MAS.Payments.DataBase.Access;
    using MAS.Payments.Infrastructure;
    using MAS.Payments.Infrastructure.Command;

    internal class UpdatePaymentGroupCommandHandler : BaseCommandHandler<UpdatePaymentGroupCommand>
    {
        private IRepository<DataBase.PaymentGroup> Repository { get; }

        public UpdatePaymentGroupCommandHandler(IResolver resolver)
            : base(resolver)
        {
            Repository = GetRepository<DataBase.PaymentGroup>();
        }

        public override async Task HandleAsync(UpdatePaymentGroupCommand command)
        {
            await Repository.Update(command.Id, new
            {
                command.PaymentDate,
                command.Month,
                command.Year,
                command.Comment,
            });
        }
    }
}
