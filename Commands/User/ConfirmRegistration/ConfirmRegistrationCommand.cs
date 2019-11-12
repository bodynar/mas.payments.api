using System;

using MAS.Payments.Infrastructure.Command;

namespace MAS.Payments.Commands
{
    public class ConfirmRegistrationCommand : ICommand
    {
        public string Token { get; set; }

        public ConfirmRegistrationCommand(string token)
        {
            Token = token ?? throw new ArgumentNullException(nameof(Token));
        }
    }
}