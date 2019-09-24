using MAS.Payments.DataBase;
using MAS.Payments.DataBase.Access;
using MAS.Payments.Infrastructure;
using MAS.Payments.Infrastructure.Command;
using MAS.Payments.Infrastructure.Specification;
using MAS.Payments.Models;

namespace MAS.Payments.Commands
{
    internal class RegisterUserCommandHandler : BaseCommandHandlerWithCheck<RegisterUserCommand>
    {
        private IRepository<User> Repository { get; }

        public RegisterUserCommandHandler(
            IResolver resolver
        ) : base(resolver)
        {
            Repository = GetRepository<User>();
        }

        public override CheckResult Check(RegisterUserCommand command)
        {
            var hasUserWithSameEmail =
                Repository.Any(
                    new CommonSpecification<User>(x => x.Email == command.Email)
                );

            if (hasUserWithSameEmail)
            {
                return CheckResult.Failure($"User with email \"{command.Email}\" is already registered");
            }

            // todo: populate when time come

            return CheckResult.Success();
        }

        public override void HandleChecked(RegisterUserCommand command)
        {
            var user = new User
            {
                Login = command.Login,
                PasswordHash = command.PasswordHash,
                Email = command.Email,
                FirstName = command.FirstName,
                LastName = command.LastName,

                IsActive = false,
                UserSettings = new UserSettings
                {
                    DisplayMeasurementNotification = true
                }
            };

            Repository.Add(user);
        }
    }
}