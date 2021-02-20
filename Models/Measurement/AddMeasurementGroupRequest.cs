namespace MAS.Payments.Models
{
    using System;
    using System.Collections.Generic;

    public class AddMeasurementGroupRequest
    {
        public DateTime Date { get; set; }

        public IEnumerable<MeasurementGroupRequestModel> Measurements { get; set; }
    }

    public class MeasurementGroupRequestModel
    {
        public long MeasurementTypeId { get; set; }

        public double Measurement { get; set; }

        public string Comment { get; set; }
    }
}
