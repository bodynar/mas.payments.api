namespace MAS.Payments.Commands
{
    using System;

    using MAS.Payments.Infrastructure.Command;

    public class UpdateMeterMeasurementCommand : ICommand
    {
        public long Id { get; }

        public double Measurement { get; }

        public string Comment { get; }

        public DateTime Date { get; }

        public long MeterMeasurementTypeId { get; }

        public UpdateMeterMeasurementCommand(
            long id,
            long meterMeasurementTypeId,
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