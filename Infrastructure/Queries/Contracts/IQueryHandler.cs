namespace MAS.Payments.Infrastructure.Query
{
    internal interface IQueryHandler<TQuery, TResult>
        where TQuery : IQuery<TResult>
    {
        TResult Handle(TQuery query);
    }
}