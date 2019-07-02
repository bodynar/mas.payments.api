namespace MAS.Payments.Infrastructure.Query
{
    public interface IQueryProcessor
    {
        TResult Execute<TResult>(IQuery<TResult> query);
    }
}