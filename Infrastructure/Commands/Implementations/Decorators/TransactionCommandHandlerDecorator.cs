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
            var currentTransaction = dbContext.Database.CurrentTransaction;

            var shouldCreateTransaction = currentTransaction == null;

            if (shouldCreateTransaction)
            {
                currentTransaction = await dbContext.Database.BeginTransactionAsync();
            }

            try
            {
                await decorated.HandleAsync(command);
                await dbContext.SaveChangesAsync();

                if (shouldCreateTransaction)
                {
                    await currentTransaction.CommitAsync();

                    currentTransaction.Dispose();
                }
            }
            catch
            {
                if (shouldCreateTransaction)
                {
                    await currentTransaction.RollbackAsync();
                }

                throw;
            }
        }
    }
}