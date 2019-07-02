namespace MAS.Payments.Infrastructure.Command
{
    public interface ICommandProcessor
    {
        void Execute<TCommand>(TCommand command)
            where TCommand : ICommand;
    }
}