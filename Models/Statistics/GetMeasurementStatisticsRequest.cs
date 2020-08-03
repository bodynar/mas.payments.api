namespace MAS.Payments.Models
{
    public class GetMeasurementStatisticsRequest
    {
        public short? Year { get; set; }

        public long? MeasurementTypeId { get; set; }
    }
}