using System.Collections.Generic;

using MAS.Payments.Infrastructure.Command;

namespace MAS.Payments.Commands
{
    public class SendMeasurementsCommand : ICommand
    {
        public string Recipient { get; }
        
        public IEnumerable<long> MeterMeasurementIdentifiers { get; }

        public SendMeasurementsCommand(IEnumerable<long> meterMeasurementIdentifiers)
        {
            MeterMeasurementIdentifiers = meterMeasurementIdentifiers;
        }
    }
}