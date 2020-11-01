namespace MAS.Payments.Commands
{

    using System;

    using MAS.Payments.Infrastructure.Command;

    public class AddMeterMeasurementCommand : ICommand
    {
        public double Measurement { get; }

        public string Comment { get; }

        public DateTime Date { get; }

        public long MeterMeasurementTypeId { get; }

        public AddMeterMeasurementCommand(long meterMeasurementTypeId, DateTime date, double measurement, string comment = null)
        {
            Measurement = measurement;
            Comment = comment;
            Date = date;
            MeterMeasurementTypeId = meterMeasurementTypeId;
        }
    }
}