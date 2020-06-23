namespace MAS.Payments.Queries
{
    using System;

    public class GetUserNotificationsQueryResult
    {
        public long? Id { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? HiddenAt { get; set; }

        public bool? IsHidden { get; set; }

        public string Key { get; set; }

        public string Type { get; set; }
    }
}