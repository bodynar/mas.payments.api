namespace MAS.Payments.Infrastructure.Exceptions
{
    using System;

    [Serializable]
    public class QueryExecutionException(
        Type queryType,
        string message,
        Exception innerException
    ) : Exception(message, innerException)
    {
        public string QueryFullTypeName { get; } = queryType.FullName;

        public string QueryName { get; } = queryType.Name;
    }
}
