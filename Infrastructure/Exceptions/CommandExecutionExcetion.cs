namespace MAS.Payments.Infrastructure.Exceptions
{
    using System;

    [Serializable]
    public class CommandExecutionException(
        Type commandType,
        string message,
        Exception innerException = null
    ) : Exception(message, innerException)
    {
        public string CommandFullTypeName { get; } = commandType.FullName;

        public string CommandName { get; } = commandType.Name;
    }
}
