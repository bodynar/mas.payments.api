namespace MAS.Payments.Infrastructure.Query
{
    using System;
    using System.Threading.Tasks;

    using MAS.Payments.Infrastructure.Exceptions;

    using Serilog;

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

            try
            {
                return await handler.HandleAsync((dynamic)query);
            }
            catch (QueryExecutionException)
            {
                throw;
            }
            catch (Exception e)
            {
                Log.Error(e, "Error executing query {QueryName}", queryType.Name);

                throw new QueryExecutionException(
                    queryType,
                    $"Error executing query {queryType.Name}: {e.Message}",
                    e);
            }
        }
    }
}
