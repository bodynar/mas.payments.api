namespace MAS.Payments.Infrastructure.Command
{
    using System.Threading.Tasks;

    using MAS.Payments.DataBase;

    internal class TransactionCommandHandlerDecorator<TCommand>(
        ICommandHandler<TCommand> decorated,
        DataBaseContext dbContext,
        IResolver resolver
    ) : BaseCommandHandler<TCommand>(resolver)
        where TCommand : ICommand
    {
        public override async Task HandleAsync(TCommand command)
        {
            using (var scope = await dbContext.Database.BeginTransactionAsync())
            {
                await decorated.HandleAsync(command);

                await scope.CommitAsync();
                await dbContext.SaveChangesAsync();
            }
        }
    }
}