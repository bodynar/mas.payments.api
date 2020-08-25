
using MAS.Payments.Infrastructure.Command;

namespace MAS.Payments.Commands
{
    public class AddPaymentTypeCommand : ICommand
    {
        public string Name { get; }

        public string Description { get; }

        public string Company { get; }
        
        public string Color { get; set; }

        public AddPaymentTypeCommand(string name, string description, string company, string color)
        {
            Name = name;
            Description = description;
            Company = company;
            Color = color;
        }
    }
}