namespace MAS.Payments.Infrastructure.Command
{
    using System.Threading.Tasks;

    internal interface ICommandHandler<in TCommand>
        where TCommand : ICommand
    {
        Task HandleAsync(TCommand command);
    }
}