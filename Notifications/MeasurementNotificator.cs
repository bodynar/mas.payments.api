using System;
using System.Linq;
using System.Collections.Generic;

using MAS.Payments.Infrastructure;
using MAS.Payments.Queries;

namespace MAS.Payments.Notifications
{
    internal class MeasurementNotificator : BaseUserNotificator
    {
        public MeasurementNotificator(
            IResolver resolver
        ) : base(resolver)
        { }

        public override IEnumerable<Notification> GetNotifications(long userId)
        {
            var today = DateTime.Today;

            if (today.Day >= 20)
            {
                var measurements =
                    QueryProcessor.Execute(
                        new GetMeterMeasurementsQuery(userId, (byte?)today.Month, year: today.Year)
                    )
                    .Select(x => x.MeterMeasurementTypeId);

                var measurementTypes =
                    QueryProcessor.Execute(new GetMeterMeasurementTypesQuery());

                if (measurementTypes.Count() != measurements.Count())
                {
                    var notFilledIds =
                        measurementTypes
                            .Select(x => x.Id)
                            .Except(measurements)
                            .ToArray();

                    var measurementTypesWithoutMeasurement =
                        measurementTypes.Where(x => notFilledIds.Contains(x.Id));

                    foreach (var measurementType in measurementTypesWithoutMeasurement)
                    {
                        var type = today.Day <= 23 ? NotificationType.Info : NotificationType.Warning;

                        yield return new Notification
                        {
                            Type = type,
                            Name = "Measurement not added",
                            Description = $"Measurenemt for {measurementType.Name} for [{today.ToString("MMMM yyyy")}] not added"
                        };
                    }
                }
            }
        }
    }
}