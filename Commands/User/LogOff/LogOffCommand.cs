using System;
using MAS.Payments.Infrastructure.Command;

namespace MAS.Payments.Commands
{
    public class LogOffCommand : ICommand
    {
        public string Token { get; }

        public LogOffCommand(string token)
        {
            Token = token ?? throw new ArgumentNullException(nameof(Token));
        }
    }
}