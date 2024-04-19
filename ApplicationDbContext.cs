using Doan_Web_CK.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Doan_Web_CK
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Nofitication> Nofitications { get; set; }
        public DbSet<Friendship> Friendships { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<ChatRoom> chatRooms { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChatRoom>()
                .HasKey(cr => cr.Id);

            modelBuilder.Entity<ChatRoom>()
                .HasOne(cr => cr.User)
                .WithMany(u => u.Chatrooms)
                .HasForeignKey(cr => cr.UserId)
                .OnDelete(DeleteBehavior.NoAction); // Thiết lập UserId trong ChatRoom thành null khi User bị xóa

            modelBuilder.Entity<ChatRoom>()
                .HasOne(cr => cr.Friend)
                .WithMany()
                .HasForeignKey(cr => cr.FriendId)
                .OnDelete(DeleteBehavior.NoAction); // Thiết lập FriendId trong ChatRoom thành null khi Friend bị xóa

            modelBuilder.Entity<Nofitication>()
                .HasOne(n => n.Blog)
                .WithMany()
                .HasForeignKey(n => n.BlogId)
                .OnDelete(DeleteBehavior.Cascade);

            // friendShip
            modelBuilder.Entity<Friendship>()
                .HasKey(f => f.Id);

            modelBuilder.Entity<Friendship>()
                .HasOne(f => f.User)
                .WithMany(u => u.Friendships)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Friendship>()
                .HasOne(f => f.Friend)
                .WithMany()
                .HasForeignKey(f => f.FriendId)
                .OnDelete(DeleteBehavior.Restrict);


            // nofitication
            modelBuilder.Entity<Nofitication>()
               .HasOne(n => n.RecieveAccount)
               .WithMany(a => a.Nofitications)
               .HasForeignKey(n => n.RecieveAccountId)
               .IsRequired(false);

            modelBuilder.Entity<Nofitication>()
                .HasOne(n => n.SenderAccount)
                .WithMany()
                .HasForeignKey(n => n.SenderAccountId)
                .IsRequired(false);

            // Các cài đặt khác của modelBuilder
            base.OnModelCreating(modelBuilder);
        }
    }
}
