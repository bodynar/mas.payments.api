namespace MAS.Payments.Infrastructure.Query
{
    using System.Threading.Tasks;

    internal interface IQueryHandler<TQuery, TResult>
        where TQuery : IQuery<TResult>
    {
        Task<TResult> HandleAsync(TQuery query);
    }
}