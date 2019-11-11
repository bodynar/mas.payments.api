namespace MAS.Payments.Infrastructure.Command
{
    // todo: find better name
    public abstract class UserCommand : ICommand
    {
        public long UserId { get; }

        public UserCommand(long userId)
        {
            UserId = userId;
        }
    }
}