namespace SocialDBViewer.Models
{
    using System;

    public class Message
    {
        public int AuthorId { get; set; }

        public int MessageId { get; set; }

        public DateTime SendDate { get; set; }

        public string Text { get; set; }
    }
}
