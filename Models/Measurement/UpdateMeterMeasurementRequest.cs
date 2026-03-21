namespace MAS.Payments.Models
{
    public class UpdateMeterMeasurementRequest
    {
        public Guid Id { get; set; }

        public double Value { get; set; }

        public string Comment { get; set; }

        public int Year { get; set; }

        public int Month { get; set; }

        public Guid TypeId { get; set; }
    }
}
