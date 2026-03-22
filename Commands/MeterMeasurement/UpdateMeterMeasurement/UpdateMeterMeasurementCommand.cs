namespace MAS.Payments.Commands
{
    using System;

    using MAS.Payments.Infrastructure.Command;

    public class UpdateMeterMeasurementCommand : ICommand
    {
        public Guid Id { get; }

        public double Measurement { get; }

        public string Comment { get; }

        public DateTime Date { get; }

        public Guid MeterMeasurementTypeId { get; }

        public UpdateMeterMeasurementCommand(
            Guid id,
            Guid meterMeasurementTypeId,
            DateTime date,
            double measurement,
            string comment = null
        )
        {
            if (id == default)
            {
                throw new ArgumentException(null, nameof(id));
            }

            if (meterMeasurementTypeId == default)
            {
                throw new ArgumentException(null, nameof(meterMeasurementTypeId));
            }

            if (date == default)
            {
                throw new ArgumentException(null, nameof(date));
            }

            if (measurement == default)
            {
                throw new ArgumentException(null, nameof(measurement));
            }

            Id = id;
            Measurement = measurement;
            Comment = comment;
            Date = new DateTime(date.Year, date.Month, 20, 0, 0, 0, DateTimeKind.Utc);
            MeterMeasurementTypeId = meterMeasurementTypeId;
        }
    }
}
