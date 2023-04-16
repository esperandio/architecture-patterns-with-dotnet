using Microsoft.Extensions.DependencyInjection;
using UseCases;

namespace Infrastructure.Mail;

public static class ServicesConfiguration
{
    public static void AddInfrastructureMailService(this IServiceCollection services)
    {
        services.AddScoped<IMailService, MailKitService>();
    }
}