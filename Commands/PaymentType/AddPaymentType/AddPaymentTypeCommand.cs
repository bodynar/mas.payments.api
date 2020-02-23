
using MAS.Payments.Infrastructure.Command;

namespace MAS.Payments.Commands
{
    public class AddPaymentTypeCommand : ICommand
    {
        public string Name { get; }

        public string Description { get; }

        public string Company { get; }

        public AddPaymentTypeCommand(string name, string description, string company)
        {
            Name = name;
            Description = description;
            Company = company;
        }
    }
}