namespace MAS.Payments.DataBase
{
    using System;

    public class Entity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public DateTime CreatedOn { get; set; }
    }
}
