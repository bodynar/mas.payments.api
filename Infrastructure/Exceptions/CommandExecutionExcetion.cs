namespace MAS.Payments.Infrastructure.Exceptions
{
    using System;

    [Serializable]
    public class CommandExecutionException : Exception
    {
        public string CommandFullTypeName { get; }

        public string CommandName { get; }

        public CommandExecutionException(Type commandType, string message)
            : base(message)
        {
            CommandFullTypeName = commandType.FullName;
            CommandName = commandType.Name;
        }
    }
}