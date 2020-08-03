namespace MAS.Payments.DataBase
{
    using System;

    public class UserNotification : Entity
    {
        public string Title { get; set; }

        public string Text { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? HiddenAt { get; set; }

        public bool IsHidden { get; set; }

        public short Type { get; set; }

        public string Key { get; set; }
    }
}