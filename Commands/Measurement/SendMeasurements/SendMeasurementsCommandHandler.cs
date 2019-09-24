using System;
using System.Collections.Generic;
using System.Linq;
using MAS.Payments.DataBase;
using MAS.Payments.DataBase.Access;
using MAS.Payments.Infrastructure;
using MAS.Payments.Infrastructure.Command;
using MAS.Payments.Infrastructure.Specification;
using MAS.Payments.MailMessages;

namespace MAS.Payments.Commands
{
    internal class SendMeasurementsCommandHandler : BaseCommandHandler<SendMeasurementsCommand>
    {
        private IRepository<MeterMeasurement> Repository { get; }

        public SendMeasurementsCommandHandler(
            IResolver resolver
        ) : base(resolver)
        {
            Repository = GetRepository<MeterMeasurement>();
        }

        public override void Handle(SendMeasurementsCommand command)
        {
            var measurements =
                Repository.Where(
                    new CommonSpecification<MeterMeasurement>(
                        x => command.MeterMeasurementIdentifiers.Contains(x.Id)))
                .ToList();

            var isAllMeasurementsInOneMonth = CheckIsAllMeasurementsInOneMonth(measurements);

            if (!isAllMeasurementsInOneMonth)
            {
                throw new Exception("Selected measurements isn't for one month");
            }

            var mapperMeasurements =
                measurements.Select(x => new MeasurementMailModel(x.MeasurementType.Name, x.Measurement));

            MailProcessor.Send(new SendMeasurementsMail(command.Recipient, new System.DateTime(), mapperMeasurements));

            foreach (var measurement in measurements)
            {
                measurement.IsSent = true;
            }
        }

        private bool CheckIsAllMeasurementsInOneMonth(IEnumerable<MeterMeasurement> measurements)
        {
            return measurements
                .Select(x => x.Date.Month)
                .GroupBy(x => x)
                .Count() == 1;
        }
    }
}