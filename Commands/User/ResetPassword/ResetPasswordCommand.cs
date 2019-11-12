using System;

using MAS.Payments.Infrastructure.Command;

namespace MAS.Payments.Commands
{
    public class ResetPasswordCommand : ICommand
    {
        public string Login { get; }

        public string Email { get; }

        public string Token { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public ResetPasswordCommand(string login, string email)
        {
            Login = login ?? throw new ArgumentNullException(nameof(Login));
            Email = email ?? throw new ArgumentNullException(nameof(Email));
        }
    }
}