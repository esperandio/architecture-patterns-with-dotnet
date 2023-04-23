using Microsoft.EntityFrameworkCore;
using Domain;
using Infrastructure.Data.Configurations;

namespace Infrastructure.Data;

public class AppDbContext : DbContext
{
    public DbSet<Product> Products => Set<Product>();

    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        string connectionString = Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING") ?? "";
        var serverVersion = new MySqlServerVersion(new Version(8, 0, 29));

        options.UseMySql(connectionString, serverVersion);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ProductAggregateConfiguration());
    }
}