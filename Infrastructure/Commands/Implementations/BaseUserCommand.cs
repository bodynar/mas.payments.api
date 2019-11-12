namespace MAS.Payments.Infrastructure.Command
{
    // todo: find better name
    public abstract class BaseUserCommand : ICommand
    {
        public long UserId { get; }

        public BaseUserCommand(long userId)
        {
            UserId = userId;
        }
    }
}