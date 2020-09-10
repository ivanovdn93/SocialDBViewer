namespace SocialDBViewer.Models
{
    using System;

    public class User
    {
        public int UserId { get; set; }

        public DateTime DateOfBirth { get; set; }

        public int Gender { get; set; }

        public DateTime LastVisit { get; set; }

        public string Name { get; set; }

        public bool Online { get; set; }
    }
}
