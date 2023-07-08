namespace MAS.Payments.Models
{
    using System.Collections.Generic;

    public class AddMeasurementGroupRequest
    {
        public short Year { get; set; }

        public byte Month { get; set; }

        public IEnumerable<MeasurementGroupRequestModel> Measurements { get; set; }
    }

    public class MeasurementGroupRequestModel
    {
        public long TypeId { get; set; }

        public double Value { get; set; }

        public string Comment { get; set; }
    }
}
