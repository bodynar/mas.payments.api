namespace MAS.Payments.Infrastructure.Command
{
    using System;

    public class CommandProcessor(
        IResolver resolver
    ) : ICommandProcessor
    {
        public void Execute<TCommand>(TCommand command)
            where TCommand : ICommand
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var handlerType = typeof(ICommandHandler<>).MakeGenericType(command.GetType());

            dynamic handler = resolver.GetInstance(handlerType);

            handler.Handle((dynamic)command);
        }
    }
}