using Microsoft.Extensions.DependencyInjection;
using UseCases;

namespace Infrastructure.Mail;

public static class ServicesConfiguration
{
    public static void AddInfrastructureMailService(this IServiceCollection services)
    {
        string host = Environment.GetEnvironmentVariable("SMTP_HOST") ?? "";
        int port = Int32.Parse(Environment.GetEnvironmentVariable("SMTP_PORT") ?? "0");
        string username = Environment.GetEnvironmentVariable("SMTP_CREDENCIAL_USERNAME") ?? "";
        string password = Environment.GetEnvironmentVariable("SMTP_CREDENCIAL_PASSWORD") ?? "";

        services.AddScoped<IMailService>(x => new MailKitService(host, port, username, password));
    }
}