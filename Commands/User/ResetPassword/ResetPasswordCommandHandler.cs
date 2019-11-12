using System;

using MAS.Payments.DataBase;
using MAS.Payments.DataBase.Access;
using MAS.Payments.Infrastructure;
using MAS.Payments.Infrastructure.Command;
using MAS.Payments.Infrastructure.Specification;
using MAS.Payments.Models;

namespace MAS.Payments.Commands
{
    internal class ResetPasswordCommandHandler : BaseCommandHandlerWithCheck<ResetPasswordCommand>
    {
        private IRepository<User> Repository { get; }

        private IRepository<UserToken> UserTokenRepository { get; }

        public ResetPasswordCommandHandler(
            IResolver resolver
        ) : base(resolver)
        {
            Repository = GetRepository<User>();
            UserTokenRepository = GetRepository<UserToken>();
        }

        public override CheckResult Check(ResetPasswordCommand command)
        {
            var hasUser =
                Repository.Any(
                    new CommonSpecification<User>(
                        x => x.Email == command.Email && x.Login == command.Login)
                );

            if (!hasUser)
            {
                return CheckResult.Failure($"Account not found");
            }

            return CheckResult.Success();
        }

        public override void HandleChecked(ResetPasswordCommand command)
        {
            var user = 
                Repository.Get(
                    new CommonSpecification<User>(
                        x => x.Email == command.Email && x.Login == command.Login)
                );

            var token = GenerateToken(user);

            command.Token = token;
            command.FirstName = user.FirstName;
            command.LastName = user.LastName;
        }

        private string GenerateToken(User user)
        {
            var token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());

            var userToken = new UserToken
            {
                CreatedAt = DateTime.UtcNow,
                ActiveTo = DateTime.UtcNow.AddDays(5),
                UserTokenTypeId = (long)UserTokenTypeEnum.PasswordReset,
                User = user, 
                Token = token
            };

            UserTokenRepository.Add(userToken);

            return token;
        }
    }
}