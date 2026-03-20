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

            var handler = resolver.GetInstance(handlerType);

            var method = handlerType.GetMethod(nameof(ICommandHandler<ICommand>.HandleAsync))
                ?? throw new InvalidOperationException($"HandleAsync method not found on {handlerType}");

            await (Task)method.Invoke(handler, [command]);
        }
    }
}
