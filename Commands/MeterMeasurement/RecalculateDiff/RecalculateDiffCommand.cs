using System.Linq;
namespace MAS.Payments.Commands
{
    using System.Collections.Generic;

    using MAS.Payments.Infrastructure.Command;

    public class RecalculateDiffCommand : ICommand
    {
        public bool ForAll { get; }

        public IEnumerable<string> Warnings { get; set; } = Enumerable.Empty<string>();

        public RecalculateDiffCommand(bool forAll = false)
        {
            ForAll = forAll;
        }
    }
}
