namespace MAS.Payments.Infrastructure.Query
{
    // todo: find better name
    public abstract class BaseUserQuery<TResult> : IQuery<TResult>
    {
        public long UserId { get; }

        public BaseUserQuery(long userId)
        {
            UserId = userId;
        }
    }
}