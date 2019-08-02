using Microsoft.AspNetCore.Mvc;

namespace MAS.Payments.Models
{
    public class FieldValueFilter
    {
        public double Exact { get; set; }

        public double Min { get; set; }

        public double Max { get; set; }
    }

    public class GetPaymentsRequest
    {
        public byte? Month { get; set; }

        public long? PaymentTypeId { get; set; }

        [FromQuery]
        public FieldValueFilter Amount { get; set; }
    }
}