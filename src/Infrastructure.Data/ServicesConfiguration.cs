using Microsoft.Extensions.DependencyInjection;
using Domain;
using UseCases;

namespace Infrastructure.Data;

public static class ServicesConfiguration
{
	public static void AddInfrastructureDataService(this IServiceCollection services)
	{
		services.AddDbContext<AppDbContext>();

        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
	}
}