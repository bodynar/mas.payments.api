namespace MAS.Payments.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class DeleteRecordsRequest
    {
        [Required]
        public IEnumerable<Guid> Ids { get; set; } = new List<Guid>();
    }
}
