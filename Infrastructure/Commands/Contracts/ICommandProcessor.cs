namespace MAS.Payments.Infrastructure.Command
{
    using System.Threading.Tasks;

    public interface ICommandProcessor
    {
        Task Execute<TCommand>(TCommand command)
            where TCommand : ICommand;
    }
}