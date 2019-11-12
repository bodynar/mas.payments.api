using System;
using MAS.Payments.Infrastructure.Command;

namespace MAS.Payments.Commands
{
    public class UpdateMeterMeasurementCommand : ICommand
    {
        public long Id { get; }

        public double Measurement { get; }

        public string Comment { get; }

        public DateTime Date { get; }

        public long MeterMeasurementTypeId { get; }

        public UpdateMeterMeasurementCommand(long id, long meterMeasurementTypeId, DateTime date, double measurement, string comment = null)
        {
            Id = id;
            Measurement = measurement;
            Comment = comment;
            Date = date;
            MeterMeasurementTypeId = meterMeasurementTypeId;
        }
    }
}