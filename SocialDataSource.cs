namespace SocialDBViewer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using SocialDBViewer.Models;
    using SocialDBViewer.Data;

    public class SocialDataSource
    {
        private readonly List<User> _users;

        private readonly List<Friend> _friends;

        private readonly List<Message> _messages;

        private readonly List<Like> _likes;

        public SocialDataSource(DataContext dataContext)
        {
            _users = dataContext.Users.ToList();
            _friends = dataContext.Friends.ToList();
            _messages = dataContext.Messages.ToList();
            _likes = dataContext.Likes.ToList();
        }

        public UserContext GetUserContext(string userName)
        {
            var userContext = new UserContext();

            if (!_users.Exists(x => x.Name == userName))
            {
                throw new ArgumentNullException();
            }

            userContext.User = _users.Find(x => x.Name == userName);

            userContext.Friends = GetUserFriends(userContext.User);

            userContext.OnlineFriends = userContext.Friends.Where(x => x.Online).ToList();

            userContext.FriendshipOffers = GetUserFriendshipOffers(userContext.User);

            userContext.Subscribers = GetUserSubscribers(userContext.User)
                .Where(x => !userContext.Friends.Exists(a => a.UserId == x.UserId))
                .ToList(); 

            userContext.News = GetUserNews(userContext.User);

            return userContext;
        }

        private List<UserInformation> GetUserFriends(User user)
        {
            var userId = user.UserId;

            var friendsIdsPart1 = _friends                
                .Where(x => x.FromUserId == userId && x.Status == 2)
                .Select(f => f.ToUserId)                
                .ToList();

            var friendsIdsPart2 = _friends
                .Where(x => x.ToUserId == userId && x.Status == 2)
                .Select(f => f.FromUserId)
                .ToList();

            var friendsIdsPart3 = new List<int>();

            friendsIdsPart3 = _friends
                .Aggregate(friendsIdsPart3, (current, friend) => current
                    .Concat(_friends
                        .Where(x => x.FromUserId == friend.ToUserId 
                                    && x.ToUserId == friend.FromUserId 
                                    && x.Status != 3 && friend.Status != 3 
                                    && (x.FromUserId == user.UserId || x.ToUserId == user.UserId))
                        .Select(f => f.ToUserId)
                        .Where(u => u != user.UserId))
                    .ToList());

            var friendsIds = friendsIdsPart1
                .Concat(friendsIdsPart2)
                .Concat(friendsIdsPart3)
                .Distinct()
                .ToList();
            
            return _users
                .Where(f => friendsIds.Contains(f.UserId))
                .Select(u => new UserInformation
                {
                    Name = u.Name, 
                    Online = u.Online, 
                    UserId = u.UserId
                })
                .ToList();
        }

        private List<UserInformation> GetUserFriendshipOffers(User user)
        { 
            var newOffersFromIds = _friends                
                .Where(a => a.ToUserId == user.UserId && user.LastVisit < a.SendDate) 
                .Select(f => f.FromUserId)                
                .ToList();

            return _users
                .Where(x => newOffersFromIds.Contains(x.UserId))
                .Select(u => new UserInformation
                {
                    Name = u.Name, 
                    Online = u.Online, 
                    UserId = u.UserId
                })
                .ToList();   
        }

        private List<UserInformation> GetUserSubscribers(User user) 
        { 
            var subscribersIds = _friends                
                .Where(a => a.ToUserId == user.UserId && a.Status < 2) 
                .Select(f => f.FromUserId)                
                .ToList();

            return _users
                .Where(x => subscribersIds.Contains(x.UserId))
                .Select(u => new UserInformation
                {
                    Name = u.Name, 
                    Online = u.Online, 
                    UserId = u.UserId
                })
                .ToList(); 
        }

        private List<News> GetUserNews(User user) 
        {            
            var userFriends = GetUserFriends(user);

            var userFriendsIds = userFriends
                .Select(u => u.UserId)
                .ToList();

            return _messages
                .Where(m => userFriendsIds.Contains(m.AuthorId) && user.LastVisit < m.SendDate) 
                .Select(m => new News 
                {
                    AuthorId = m.AuthorId,
                    AuthorName = userFriends.Find(f => f.UserId == m.AuthorId).Name,
                    Likes = _likes
                        .Where(l => l.MessageId == m.MessageId)
                        .Select(u => u.UserId)
                        .ToList(),
                    Text = m.Text
                })
                .ToList();
        }
    }
}
