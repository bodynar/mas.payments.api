namespace MAS.Payments.Models
{
    using Microsoft.AspNetCore.Mvc;

    public class FieldValueFilter
    {
        public double? Exact { get; set; }

        public double? Min { get; set; }

        public double? Max { get; set; }
    }

    public class GetPaymentsRequest
    {
        public byte? Month { get; set; }

        public short? Year { get; set; }

        public long? PaymentTypeId { get; set; }

        [FromQuery]
        public FieldValueFilter Amount { get; set; }
    }
}