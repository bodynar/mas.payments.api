using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using MAS.Payments.DataBase;
using MAS.Payments.DataBase.Access;
using MAS.Payments.Infrastructure;
using MAS.Payments.Infrastructure.Command;
using MAS.Payments.Infrastructure.Exceptions;
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

            var isAlreadySentMeasurements = measurements.Where(x => x.IsSent).Select(x => $"{x.MeasurementType.Name} {x.Measurement}");

            if (isAlreadySentMeasurements.Any())
            {
                throw new CommandExecutionException("Some of measurements marked as sent. Measurements: " + string.Join(", ", isAlreadySentMeasurements));
            }

            var isAllMeasurementsInOneMonth = CheckIsAllMeasurementsInOneMonth(measurements);

            if (!isAllMeasurementsInOneMonth)
            {
                throw new Exception("Selected measurements isn't for one month");
            }

            var mappedMeasurements =
                measurements.Select(x => $"{x.MeasurementType.Name}: {x.Measurement}");
            
            var date = DateTime.Today.ToString("MMMM yyyy", CultureInfo.GetCultureInfo("ru-RU"));

            var messageModel =
                new MeasurementMailModel(date, string.Join("\n", mappedMeasurements));

            MailProcessor.Send(new SendMeasurementsMail(command.Recipient, date, messageModel));

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