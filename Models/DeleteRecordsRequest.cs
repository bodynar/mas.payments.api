namespace MAS.Payments.Models
{
    using System.Collections.Generic;

    public class DeleteRecordsRequest
    {
        public IEnumerable<long> Ids { get; set; }
    }
}
