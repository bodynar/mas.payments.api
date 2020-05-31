using System;
using System.Collections.Generic;

using MAS.Payments.Infrastructure.Command;

namespace MAS.Payments.Commands
{
    public class SendMeasurementsCommand : ICommand
    {
        public string Recipient { get; }
        
        public IEnumerable<long> MeterMeasurementIdentifiers { get; }

        public SendMeasurementsCommand(string recipient, IEnumerable<long> meterMeasurementIdentifiers)
        {
            Recipient = recipient ?? throw new ArgumentException(nameof(recipient));
            MeterMeasurementIdentifiers = meterMeasurementIdentifiers;
        }
    }
}