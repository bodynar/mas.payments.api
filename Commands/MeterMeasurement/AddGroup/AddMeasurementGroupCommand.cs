namespace MAS.Payments.Commands
{
    using System;
    using System.Collections.Generic;

    using MAS.Payments.Infrastructure.Command;

    public class AddMeasurementGroupCommand : ICommand
    {
        public DateTime Date { get; }

        public IEnumerable<MeasurementGroup> Measurements { get; }

        public AddMeasurementGroupCommand(DateTime date, IEnumerable<MeasurementGroup> measurements)
        {
            if (date == default)
            {
                throw new ArgumentException(null, nameof(date));
            }

            Date = new DateTime(date.Year, date.Month, 20, 0, 0,0, DateTimeKind.Utc);
            Measurements = measurements ?? [];
        }
    }
}
