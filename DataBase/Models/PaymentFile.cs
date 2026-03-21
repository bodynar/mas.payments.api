namespace MAS.Payments.DataBase
{
    public class PaymentFile : Entity
    {
        public string FileName { get; set; }

        public long FileSize { get; set; }

        public string ContentType { get; set; }

        public byte[] Data { get; set; }

        public Guid? PaymentId { get; set; }

        public virtual Payment Payment { get; set; }

        public Guid? PaymentGroupId { get; set; }

        public virtual PaymentGroup PaymentGroup { get; set; }
    }
}
