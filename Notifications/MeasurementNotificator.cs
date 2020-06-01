namespace MAS.Payments.Notifications
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MAS.Payments.DataBase;
    using MAS.Payments.Infrastructure;
    using MAS.Payments.Queries;
    using MAS.Payments.Utilities;

    internal class MeasurementNotificator : BaseNotificator
    {
        public MeasurementNotificator(
            IResolver resolver
        ) : base(resolver)
        {
        }

        public override IEnumerable<Notification> GetNotifications()
        {
            var today = DateTime.Today;

            var displayNotificationSetting = QueryProcessor.Execute(new GetNamedUserSettingQuery(DefaultUserSettings.DisplayMeasurementsNotification.ToString()));
            var displayNotificationSettingValue = UserSettingUtilities.GetTypedSettingValue<Boolean>(displayNotificationSetting);

            if (displayNotificationSettingValue && today.Day >= 20)
            {
                var measurements =
                    QueryProcessor.Execute(
                        new GetMeterMeasurementsQuery((byte?)today.Month, year: today.Year)
                    )
                    .SelectMany(x => x.Measurements.Select(y => y.MeterMeasurementTypeId))
                    .Distinct();

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