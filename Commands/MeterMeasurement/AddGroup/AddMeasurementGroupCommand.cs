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
            if (date == default)
            {
                throw new ArgumentException(nameof(date));
            }

            Date = new DateTime(date.Year, date.Month, 20);
            Measurements = measurements ?? Enumerable.Empty<MeasurementGroup>();
        }
    }
}
