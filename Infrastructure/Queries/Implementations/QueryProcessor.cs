namespace MAS.Payments.Infrastructure.Query
{
    using System;
    using System.Threading.Tasks;

    public class QueryProcessor(
        IResolver resolver
    ) : IQueryProcessor
    {
        public async Task<TResult> Execute<TResult>(IQuery<TResult> query)
        {
            ArgumentNullException.ThrowIfNull(query);

            var queryType = query.GetType();
            var handlerType = typeof(IQueryHandler<,>).MakeGenericType(queryType, typeof(TResult));

            dynamic handler = resolver.GetInstance(handlerType);

            return await handler.HandleAsync((dynamic)query);
        }
    }
}
