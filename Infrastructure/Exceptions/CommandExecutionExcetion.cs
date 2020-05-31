using System;

namespace MAS.Payments.Infrastructure.Exceptions
{
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