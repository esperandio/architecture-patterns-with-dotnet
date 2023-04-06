using Microsoft.Extensions.DependencyInjection;
using Domain;

namespace Infrastructure.Data;

public static class ServicesConfiguration
{
	public static void AddInfrastructureDataService(this IServiceCollection services)
	{
		services.AddDbContext<AppDbContext>();

        services.AddScoped<IBatchRepository, BatchRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
	}
}