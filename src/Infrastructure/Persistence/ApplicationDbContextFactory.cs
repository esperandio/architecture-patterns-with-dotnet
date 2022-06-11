using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Persistence;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        // Get environment
        string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        // Build config
        IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../../Main"))
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{environment}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

        var connectionString = config.GetConnectionString(nameof(ApplicationDbContext));

        optionsBuilder.UseMySql(
            connectionString, 
            new MySqlServerVersion(new Version(8, 0, 29))
        );

        return new ApplicationDbContext(optionsBuilder.Options);
    }

    public ApplicationDbContext CreateInMemoryDbContext(string databaseName = "InMemoryDb")
    {
        var optionsBuilder = new DbContextOptionsBuilder();

        optionsBuilder.UseInMemoryDatabase(databaseName);

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}