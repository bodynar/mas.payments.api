namespace MAS.Payments.Models
{
    public class GetMeterMeasurementRequest
    {
        public byte Month { get; set; }
        
        public long MeasurementTypeId { get; set; }
    }
}