namespace MAS.Payments.Infrastructure.Command
{
    using MAS.Payments.DataBase;

    internal class TransactionCommandHandlerDecorator<TCommand> : BaseCommandHandler<TCommand>
        where TCommand : ICommand
    {
        private readonly ICommandHandler<TCommand> decorated;

        private readonly DataBaseContext dataBaseContext;

        public TransactionCommandHandlerDecorator(
            ICommandHandler<TCommand> decorated,
            DataBaseContext dbContext,
            IResolver resolver
        ) : base(resolver)
        {
            this.decorated = decorated;
            dataBaseContext = dbContext;
        }

        public override void Handle(TCommand command)
        {
            using (var scope = dataBaseContext.Database.BeginTransaction())
            {
                decorated.Handle(command);

                scope.Commit();
                dataBaseContext.SaveChanges();
            }
        }
    }
}