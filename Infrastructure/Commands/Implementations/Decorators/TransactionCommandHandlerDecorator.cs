namespace MAS.Payments.Infrastructure.Command
{
    using MAS.Payments.DataBase;

    internal class TransactionCommandHandlerDecorator<TCommand>(
        ICommandHandler<TCommand> decorated,
        DataBaseContext dbContext,
        IResolver resolver
    ) : BaseCommandHandler<TCommand>(resolver)
        where TCommand : ICommand
    {
        public override void Handle(TCommand command)
        {
            using (var scope = dbContext.Database.BeginTransaction())
            {
                decorated.Handle(command);

                scope.Commit();
                dbContext.SaveChanges();
            }
        }
    }
}