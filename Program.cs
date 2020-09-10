namespace SocialDBViewer
{
    using System;
    using SocialDBViewer.Data;

    public class Program
    {
        public static void Main()
        {
            Console.WriteLine("Enter username:");
            var name = Console.ReadLine();

            if (string.IsNullOrEmpty(name))
            {
                Console.WriteLine("Username was not set!");
                return;
            }

            var dataContext = new DataContext();

            var socialDataSource = new SocialDataSource(dataContext);

            var userContext = new UserContext();

            try
            {
                userContext = socialDataSource.GetUserContext(name);
            }
            catch (ArgumentNullException)
            {
                Console.WriteLine("User was not found!");
                return;
            }

            Show(userContext);
        }

        public static void Show(UserContext userContext)
        {
            Console.WriteLine($"Hello, {userContext.User.Name}!");

            var age = DateTime.Now.Year - userContext.User.DateOfBirth.Year;

            if (DateTime.Now.DayOfYear < userContext.User.DateOfBirth.DayOfYear)
            {
                age--;
            }

            Console.WriteLine($"Age: {age}");

            Console.BackgroundColor = ConsoleColor.DarkMagenta;
            Console.Write("Friends:");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine($" {userContext.Friends.Count}");

            foreach (var friend in userContext.Friends)
            {
                Console.WriteLine(friend.Name);
            }

            Console.BackgroundColor = ConsoleColor.DarkMagenta;
            Console.Write("Friends online:");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine($" {userContext.OnlineFriends.Count}");

            foreach (var onlineFriend in userContext.OnlineFriends)
            {
                Console.WriteLine(onlineFriend.Name);
            }

            Console.BackgroundColor = ConsoleColor.DarkMagenta;
            Console.Write("Subscribers:");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine($" {userContext.Subscribers.Count}");

            foreach (var subscriber in userContext.Subscribers)
            {
                Console.WriteLine(subscriber.Name);
            }

            Console.BackgroundColor = ConsoleColor.DarkMagenta;
            Console.Write("New friendship offers:");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine($" {userContext.FriendshipOffers.Count}");

            foreach (var friendshipOffer in userContext.FriendshipOffers)
            {
                Console.WriteLine(friendshipOffer.Name);
            }

            Console.BackgroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine("News:");

            Console.BackgroundColor = ConsoleColor.Black;
            foreach (var news in userContext.News)
            {
                Console.WriteLine(news.AuthorName);
                Console.WriteLine(news.Text);
                Console.WriteLine($"Likes: {news.Likes.Count}");
                Console.WriteLine("-------");
            }
        }
    }
}
