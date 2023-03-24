using Microsoft.EntityFrameworkCore;
using Domain;
using Infrastructure.Data.Configurations;

namespace Infrastructure.Data;

public class AppDbContext : DbContext
{
    public DbSet<Batch> Batches => Set<Batch>();

    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        var connectionString = "server=db;user=root;password=my-secret-pw;database=app";
        var serverVersion = new MySqlServerVersion(new Version(8, 0, 29));

        options.UseMySql(connectionString, serverVersion);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new BatchConfiguration());
    }
}