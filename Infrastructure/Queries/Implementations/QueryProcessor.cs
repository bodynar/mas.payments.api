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

            var handler = resolver.GetInstance(handlerType);

            var method = handlerType.GetMethod(nameof(IQueryHandler<IQuery<TResult>, TResult>.HandleAsync))
                ?? throw new InvalidOperationException($"HandleAsync method not found on {handlerType}");

            return await (Task<TResult>)method.Invoke(handler, [query]);
        }
    }
}
