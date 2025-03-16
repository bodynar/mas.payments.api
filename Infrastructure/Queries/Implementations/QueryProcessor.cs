namespace MAS.Payments.Infrastructure.Query
{
    using System;

    public class QueryProcessor(
        IResolver resolver
    ) : IQueryProcessor
    {
        public TResult Execute<TResult>(IQuery<TResult> query)
        {
            ArgumentNullException.ThrowIfNull(query);

            var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));

            dynamic handler = resolver.GetInstance(handlerType);

            return handler.Handle((dynamic)query);
        }
    }
}