namespace MAS.Payments.Queries
{
    using System.Collections.Generic;

    public class TypeStatisticsItem
    {
        public long MeasurementTypeId { get; set; }

        public string MeasurementTypeName { get; set; }

        public ICollection<StatisticsDataItem> StatisticsData { get; } = new List<StatisticsDataItem>();
    }
}
