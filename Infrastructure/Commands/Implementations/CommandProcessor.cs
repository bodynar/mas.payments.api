using System;

namespace MAS.Payments.Infrastructure.Command
{
    public class CommandProcessor : ICommandProcessor
    {
        private IResolver Resolver { get; }

        public CommandProcessor(
            IResolver resolver
        )
        {
            Resolver = resolver;
        }

        public void Execute<TCommand>(TCommand command)
            where TCommand : ICommand
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var handlerType = typeof(ICommandHandler<>).MakeGenericType(command.GetType());

            dynamic handler = Resolver.GetInstance(handlerType);

            handler.Handle((dynamic)command);
        }
    }
}