namespace MAS.Payments.Infrastructure.Command
{
    internal interface ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        void Handle(TCommand command);
    }
}