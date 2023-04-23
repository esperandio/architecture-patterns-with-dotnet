using Microsoft.Extensions.DependencyInjection;
using Handlers.Abstractions;

namespace Infrastructure.MessageBroker;

public static class ServicesConfiguration
{
    public static void AddInfrastructureMessageBrokerService(this IServiceCollection services)
    {
        string host = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "";
        int port = Int32.Parse(Environment.GetEnvironmentVariable("RABBITMQ_PORT") ?? "0");
        string username = Environment.GetEnvironmentVariable("RABBITMQ_USERNAME") ?? "";
        string password = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD") ?? "";

        services.AddScoped<IMessageBroker>(x => new RabbitMQMessageBroker(host, port, username, password));
    }
}