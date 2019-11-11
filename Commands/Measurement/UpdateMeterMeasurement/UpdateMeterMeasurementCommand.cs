using System;
using MAS.Payments.Infrastructure.Command;

namespace MAS.Payments.Commands
{
    public class UpdateMeterMeasurementCommand : UserCommand
    {
        public long Id { get; }

        public double Measurement { get; }

        public string Comment { get; }

        public DateTime Date { get; }

        public long MeterMeasurementTypeId { get; }

        public UpdateMeterMeasurementCommand(long userId, 
            long id, long meterMeasurementTypeId, 
            DateTime date, double measurement, string comment = null)
            : base(userId)
        {
            Id = id;
            Measurement = measurement;
            Comment = comment;
            Date = date;
            MeterMeasurementTypeId = meterMeasurementTypeId;
        }
    }
}