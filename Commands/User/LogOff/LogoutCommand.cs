using System;
using MAS.Payments.Infrastructure.Command;

namespace MAS.Payments.Commands
{
    public class LogoutCommand : ICommand
    {
        public string Token { get; }

        public LogoutCommand(string token)
        {
            Token = token ?? throw new ArgumentNullException(nameof(Token));
        }
    }
}