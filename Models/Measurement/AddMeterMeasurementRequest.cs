namespace MAS.Payments.Models
{
    public class AddMeterMeasurementRequest
    {
        public double Measurement { get; set; }

        public string Comment { get; set; }

        public int Year { get; set; }

        public int Month { get; set; }

        public long MeterMeasurementTypeId { get; set; }
    }
}