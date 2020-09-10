namespace SocialDBViewer.Data
{
    using SocialDBViewer.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;

    public class DataContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnectionString"));
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Friend> Friends { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<Like> Likes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var usersModel = modelBuilder.Entity<User>();

            usersModel
                .ToTable("users")
                .HasKey(x => x.UserId)
                .HasName("userId");

            usersModel.Property(x => x.UserId).HasColumnName("userId").IsRequired();
            usersModel.Property(x => x.DateOfBirth).HasColumnName("dateOfBirth").IsRequired();
            usersModel.Property(x => x.Gender).HasColumnName("gender").IsRequired();
            usersModel.Property(x => x.LastVisit).HasColumnName("lastVisit");
            usersModel.Property(x => x.Name).HasColumnName("name").HasMaxLength(255).IsRequired();
            usersModel.Property(x => x.Online).HasColumnName("isOnline").IsRequired();

            var friendsModel = modelBuilder.Entity<Friend>();

            friendsModel
                .ToTable("friends")
                .HasKey(x => x.FriendId)
                .HasName("friendId");

            friendsModel.Property(x => x.FriendId).HasColumnName("friendId").IsRequired();
            friendsModel.Property(x => x.FromUserId).HasColumnName("fromUserId").IsRequired();
            friendsModel.Property(x => x.ToUserId).HasColumnName("toUserId");
            friendsModel.Property(x => x.SendDate).HasColumnName("sendDate").IsRequired();
            friendsModel.Property(x => x.Status).HasColumnName("status").IsRequired();

            friendsModel
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(p => p.FromUserId)
                .OnDelete(DeleteBehavior.Cascade);

            friendsModel
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(p => p.ToUserId)
                .OnDelete(DeleteBehavior.NoAction);

            var messagesModel = modelBuilder.Entity<Message>();

            messagesModel
                .ToTable("messages")
                .HasKey(x => x.MessageId)
                .HasName("messageId");

            messagesModel.Property(x => x.MessageId).HasColumnName("messageId").IsRequired();
            messagesModel.Property(x => x.AuthorId).HasColumnName("authorId").IsRequired();
            messagesModel.Property(x => x.SendDate).HasColumnName("sendDate").IsRequired();
            messagesModel.Property(x => x.Text).HasColumnName("text").HasMaxLength(1000).IsRequired();
            
            messagesModel
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(p => p.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);

            var likesModel = modelBuilder.Entity<Like>();

            likesModel
                .ToTable("likes")
                .HasNoKey();

            likesModel.Property(x => x.UserId).HasColumnName("userId").IsRequired();
            likesModel.Property(x => x.MessageId).HasColumnName("messageId");

            likesModel
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            likesModel
                .HasOne<Message>()
                .WithMany()
                .HasForeignKey(p => p.MessageId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
