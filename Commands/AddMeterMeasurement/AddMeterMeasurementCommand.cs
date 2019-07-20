
using System;
using MAS.Payments.Infrastructure.Command;

namespace MAS.Payments.Commands
{
    public class AddMeterMeasurementCommand : ICommand
    {
        public double Measurement { get; set; }

        public string Comment { get; set; }

        public DateTime Date { get; set; }

        public long MeterMeasurementTypeId { get; set; }

        public AddMeterMeasurementCommand(long meterMeasurementTypeId, DateTime date, double measurement, string comment = null)
        {
            Measurement = measurement;
            Comment = comment;
            Date = date;
            MeterMeasurementTypeId = meterMeasurementTypeId;
        }
    }
}