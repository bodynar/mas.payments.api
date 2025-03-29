namespace MAS.Payments.DataBase.Models
{
    using System;

    public class PdfDocument: Entity
    {        
        public string Name { get; set; } = string.Empty;
        
        public byte[] FileData { get; set; } = [];

        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    }
}
