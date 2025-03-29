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
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var handlerType = typeof(ICommandHandler<>).MakeGenericType(command.GetType());

            dynamic handler = resolver.GetInstance(handlerType);

            await handler.HandleAsync((dynamic)command);
        }
    }
}