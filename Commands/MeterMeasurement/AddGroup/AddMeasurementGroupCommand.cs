namespace MAS.Payments.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using MAS.Payments.Infrastructure.Command;

    public class AddMeasurementGroupCommand : ICommand
    {
        public DateTime Date { get; }

        public IEnumerable<MeasurementGroup> Measurements { get; }

        public AddMeasurementGroupCommand(DateTime date, IEnumerable<MeasurementGroup> measurements)
        {
            Date = date;
            Measurements = measurements ?? Enumerable.Empty<MeasurementGroup>();
        }
    }
}
