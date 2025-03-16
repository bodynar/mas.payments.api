namespace MAS.Payments.Commands
{
    using System.Collections.Generic;

    using MAS.Payments.Infrastructure.Command;

    public class RecalculateDiffCommand(
        bool forAll = false
    ) : ICommand
    {
        public bool ForAll { get; } = forAll;

        public IEnumerable<string> Warnings { get; set; } = [];
    }
}
