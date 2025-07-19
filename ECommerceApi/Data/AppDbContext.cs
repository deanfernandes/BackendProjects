using Microsoft.EntityFrameworkCore;
using ECommerceApi.Models;
using ECommerceApi.Services;

namespace ECommerceApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(u => u.Email).IsUnique();
            });
        }

        public static void Seed(AppDbContext context, IPasswordService passwordService)
        {
            if (!context.Users.Any())
            {
                context.Users.AddRange(
                    new User
                    {
                        Email = "alice@example.com",
                        PasswordHash = passwordService.HashPassword<User>(null!, "password1"),
                        FirstName = "Alice",
                        LastName = "Johnson"
                    },
                    new User
                    {
                        Email = "bob@example.com",
                        PasswordHash = passwordService.HashPassword<User>(null!, "password2"),
                        FirstName = "Bob",
                        LastName = "Smith"
                    }
                );
                context.SaveChanges();
            }

            if (!context.Products.Any())
            {
                var products = new List<Product>
                {
                    new Product { Name = "Apple iPhone 14", Description = "Latest Apple smartphone", Price = 999.99m, Quantity = 50 },
                    new Product { Name = "Samsung Galaxy S23", Description = "Flagship Samsung phone", Price = 899.99m, Quantity = 40 },
                    new Product { Name = "Sony WH-1000XM5", Description = "Noise cancelling headphones", Price = 349.99m, Quantity = 30 }
                };

                context.Products.AddRange(products);
                context.SaveChanges();
            }
        }
    }
}