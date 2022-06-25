using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Persistence;

public static class ServicesConfiguration
{
	public static void AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
	{
        var connectionString = configuration.GetConnectionString(nameof(ApplicationDbContext));

		services.AddDbContext<ApplicationDbContext>(
            options => options.UseMySql(
                connectionString, 
                new MySqlServerVersion(new Version(8, 0, 29))
            )
        );

        services.AddScoped<EntityFrameworkRepository, EntityFrameworkRepository>();
	}
}