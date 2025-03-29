namespace MAS.Payments.Infrastructure.Query
{
    internal interface IQueryHandler<TQuery, out TResult>
        where TQuery : IQuery<TResult>
    {
        TResult Handle(TQuery query);
    }
}