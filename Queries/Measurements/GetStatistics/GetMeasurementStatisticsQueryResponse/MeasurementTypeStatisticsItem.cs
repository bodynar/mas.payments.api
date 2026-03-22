namespace MAS.Payments.Queries
{
    using System.Collections.Generic;

    public class MeasurementTypeStatisticsItem
    {
        public Guid MeasurementTypeId { get; set; }

        public string MeasurementTypeName { get; set; }

        public ICollection<MeasurementStatisticsDataItem> StatisticsData { get; } = [];
    }
}
