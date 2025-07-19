using Microsoft.EntityFrameworkCore;
using ECommerceApi.Models;

namespace ECommerceApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(u => u.Email).IsUnique();
            });
        }

        public static void Seed(AppDbContext context)
        {
            if (!context.Users.Any())
            {
                context.Users.AddRange(
                    new User
                    {
                        Email = "alice@example.com",
                        PasswordHash = "hashedpassword1",
                        FirstName = "Alice",
                        LastName = "Johnson"
                    },
                    new User
                    {
                        Email = "bob@example.com",
                        PasswordHash = "hashedpassword2",
                        FirstName = "Bob",
                        LastName = "Smith"
                    }
                );
                context.SaveChanges();
            }
        }
    }
}