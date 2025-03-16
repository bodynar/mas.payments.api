namespace MAS.Payments.Notifications
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using MAS.Payments.DataBase;
    using MAS.Payments.Infrastructure;
    using MAS.Payments.Queries;
    using MAS.Payments.Utilities;

    internal class MeasurementNotificator(
        IResolver resolver
    ) : BaseNotificator(resolver)
    {
        public override IEnumerable<UserNotification> GetNotifications()
        {
            var today = DateTime.Today;

            var displayNotificationSetting = QueryProcessor.Execute(new GetNamedUserSettingQuery(DefaultUserSettings.DisplayMeasurementsNotification.ToString()));
            var displayNotificationSettingValue = UserSettingUtilities.GetTypedSettingValue<bool>(displayNotificationSetting);

            if (displayNotificationSettingValue && today.Day >= 20)
            {
                var wasNotificationFormed = CheckWasNotificationFormed($"MeasurementNotificationFor{today.Year}{today.Month}");

                if (!wasNotificationFormed)
                {
                    var measurements =
                        QueryProcessor.Execute(
                            new GetGroupedMeterMeasurementsQuery((byte?)today.Month, year: today.Year)
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
                            measurementTypes.Where(x => notFilledIds.Contains(x.Id)).Select(x => x.Name);

                        var type = today.Day <= 23 ? UserNotificationType.Info : UserNotificationType.Warning;

                        yield return new UserNotification
                        {
                            Type = (short)type,
                            Title = "Measurement not added",
                            Key = $"MeasurementNotificationFor{today.Year}{today.Month}",
                            Text = $"Measurement for [{today.ToString("MMMM yyyy")}] not added for next types: {string.Join(", ", measurementTypesWithoutMeasurement)}"
                        };
                    }
                }
            }
        }
    }
}