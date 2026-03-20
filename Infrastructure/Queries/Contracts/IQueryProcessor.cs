namespace MAS.Payments.Infrastructure.Query
{
    using System.Threading.Tasks;

    public interface IQueryProcessor
    {
        Task<TResult> Execute<TResult>(IQuery<TResult> query);
    }
}