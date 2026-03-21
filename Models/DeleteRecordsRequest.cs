namespace MAS.Payments.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class DeleteRecordsRequest
    {
        [Required]
        public IEnumerable<long> Ids { get; set; } = new List<long>();
    }
}
