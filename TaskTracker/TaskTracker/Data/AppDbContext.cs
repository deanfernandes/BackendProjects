using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TaskTracker;
using TaskTracker.Models;

public class AppDbContext : DbContext
{
    public DbSet<TaskItem> Tasks => Set<TaskItem>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var config = new ConfigurationBuilder()
            .AddUserSecrets<Program>()
            .Build();

        optionsBuilder.UseSqlServer(config.GetConnectionString("TaskTrackerDB"));
    }
}
