namespace MAS.Payments.Queries
{
    public class GetMeterMeasurementsQueryResponse
    {
        public long Id { get; set; }

        public int DateYear { get; set; }

        public int DateMonth { get; set; }

        public double Measurement { get; set; }

        public double? Diff { get; set; }

        public string Comment { get; set; }

        public bool IsSent { get; set; }

        public long MeterMeasurementTypeId { get; set; }

        public string MeasurementTypeColor { get; set; }

        public string MeasurementTypeName { get; set; }
    }
}
