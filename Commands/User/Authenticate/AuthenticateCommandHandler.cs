using System;
using MAS.Payments.DataBase;
using MAS.Payments.DataBase.Access;
using MAS.Payments.Infrastructure;
using MAS.Payments.Infrastructure.Command;
using MAS.Payments.Infrastructure.Exceptions;
using MAS.Payments.Infrastructure.Specification;

namespace MAS.Payments.Commands
{
    internal class AuthenticateCommandHandler : BaseCommandHandler<AuthenticateCommand>
    {
        public IRepository<User> UserRepository { get; }

        private IRepository<UserToken> UserTokenRepository { get; }

        public AuthenticateCommandHandler(
            IResolver resolver
        ) : base(resolver)
        {
            UserRepository = GetRepository<User>();
            UserTokenRepository = GetRepository<UserToken>();
        }

        public override void Handle(AuthenticateCommand command)
        {
            var user =
                UserRepository.Get(
                    new CommonSpecification<User>(
                        x => x.Login == command.Login && x.PasswordHash == command.PasswordHash));

            if (user == null)
            {
                throw new CommandExecutionException("Login or password incorrect");
            }

            if (!user.IsActive)
            {
                throw new CommandExecutionException("User is not active");
            }

            var token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            var tokenLiveTime = command.RememberMe ? 24 : 12;

            var authToken = new UserToken
            {
                CreatedAt = DateTime.UtcNow,
                ActiveTo = DateTime.UtcNow.AddHours(tokenLiveTime),
                UserTokenTypeId = (long)UserTokenTypeEnum.Auth,
                User = user,
                UserId = user.Id, 
                Token = token
            };

            UserTokenRepository.Add(authToken);

            command.Token = token;
        }
    }
}