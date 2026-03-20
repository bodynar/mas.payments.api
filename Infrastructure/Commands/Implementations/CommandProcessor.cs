namespace MAS.Payments.Infrastructure.Command
{
    using System;
    using System.Threading.Tasks;

    public class CommandProcessor(
        IResolver resolver
    ) : ICommandProcessor
    {
        public async Task Execute<TCommand>(TCommand command)
            where TCommand : ICommand
        {
            ArgumentNullException.ThrowIfNull(command);

            var commandType = command.GetType();
            var handlerType = typeof(ICommandHandler<>).MakeGenericType(commandType);

            dynamic handler = resolver.GetInstance(handlerType);

            await handler.HandleAsync((dynamic)command);
        }
    }
}
