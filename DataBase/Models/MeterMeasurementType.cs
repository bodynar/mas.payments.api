namespace MAS.Payments.DataBase
{
    public class MeterMeasurementType : Entity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public long PaymentTypeId { get; set; }

        public PaymentType PaymentType { get; set; }
    }
}