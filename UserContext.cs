namespace SocialDBViewer
{
    using System.Collections.Generic;
    using SocialDBViewer.Models;

    public class UserContext
    {
        public User User { get; set; }

        public List<UserInformation> Friends { get; set; }

        public List<UserInformation> OnlineFriends { get; set; }

        public List<UserInformation> FriendshipOffers { get; set; }

        public List<UserInformation> Subscribers { get; set; }

        public List<News> News { get; set; }
    }
}
