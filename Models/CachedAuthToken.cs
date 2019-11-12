using System;

namespace MAS.Payments.Models
{
    public class CachedAuthToken
    {
        public string Token { get; set; }

        public DateTime ActiveTo { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime LastChecked { get; set; }

        public long UserId { get; set; }

        public string UserName { get; set; }
    }
}