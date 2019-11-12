
using MAS.Payments.Infrastructure.Command;

namespace MAS.Payments.Commands
{
    public class AddPaymentTypeCommand : BaseUserCommand
    {
        public string Name { get; }

        public string Description { get; }

        public string Company { get; }

        public AddPaymentTypeCommand(long userId, string name, string description, string company)
            : base(userId)
        {
            Name = name;
            Description = description;
            Company = company;
        }
    }
}