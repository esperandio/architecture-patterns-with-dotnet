using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder();

        optionsBuilder.UseMySql(
            "server=127.0.0.1; port=3306; database=container; user=root; password=my-secret-pw", 
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