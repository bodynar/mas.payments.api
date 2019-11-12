using System;
using System.Collections.Generic;

using MAS.Payments.Infrastructure.Command;

namespace MAS.Payments.Commands
{
    public class SendMeasurementsCommand : BaseUserCommand
    {
        public string Recipient { get; }
        
        public IEnumerable<long> MeterMeasurementIdentifiers { get; }

        public SendMeasurementsCommand(long userId, string recipient, IEnumerable<long> meterMeasurementIdentifiers)
            : base(userId)
        {
            Recipient = recipient ?? throw new ArgumentException(nameof(recipient));
            MeterMeasurementIdentifiers = meterMeasurementIdentifiers;
        }
    }
}