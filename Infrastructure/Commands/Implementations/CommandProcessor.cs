namespace MAS.Payments.Infrastructure.Command
{
    using System;
    using System.Threading.Tasks;

    using MAS.Payments.Infrastructure.Exceptions;

    using Serilog;

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

            try
            {
                await handler.HandleAsync((dynamic)command);
            }
            catch (CommandExecutionException)
            {
                throw;
            }
            catch (Exception e)
            {
                Log.Error(e, "Error executing command {CommandName}", commandType.Name);

                throw new CommandExecutionException(
                    commandType,
                    $"Error executing command {commandType.Name}: {e.Message}",
                    e);
            }
        }
    }
}
