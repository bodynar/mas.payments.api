using System;
using MAS.Payments.DataBase;
using MAS.Payments.Infrastructure.Command;

namespace MAS.Payments.Commands
{
    public class AuthenticateCommand : ICommand
    {
        public string Login { get; }

        public string PasswordHash { get; }

        public bool RememberMe { get; }

        public UserToken Token { get; set; }

        public AuthenticateCommand(string login, string passwordHash, bool rememberMe = false)
        {
            Login = login ?? throw new ArgumentNullException(nameof(Login));
            PasswordHash = passwordHash ?? throw new ArgumentNullException(nameof(Login));
            RememberMe = rememberMe;
        }
    }
}