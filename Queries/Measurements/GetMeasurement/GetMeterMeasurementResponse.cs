namespace MAS.Payments.Queries
{
    public class GetMeterMeasurementResponse
    {
        public Guid Id { get; set; }

        public double Measurement { get; set; }

        public double? Diff { get; set; }

        public string Comment { get; set; }

        public int DateYear { get; set; }

        public int DateMonth { get; set; }

        public bool IsSent { get; set; }

        public Guid MeterMeasurementTypeId { get; set; }

        public string MeasurementTypeName { get; set; }
    }
}
