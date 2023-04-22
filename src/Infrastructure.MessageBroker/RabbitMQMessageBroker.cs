using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

namespace Infrastructure.MessageBroker;

public class RabbitMQMessageBroker
{
    private readonly IModel _channel;

    public RabbitMQMessageBroker(string host, int port, string username, string password)
    {
        var factory = new ConnectionFactory 
        { 
            HostName = host,
            UserName = username,
            Password = password
        };

        var connection = factory.CreateConnection();

        _channel = connection.CreateModel();
    }

    public void Publish(string queue, object value)
    {
        _channel.QueueDeclare(
            queue: queue,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );

        var message = JsonSerializer.Serialize(value);

        var body = Encoding.UTF8.GetBytes(message);

        _channel.BasicPublish(
            exchange: string.Empty,
            routingKey: queue,
            basicProperties: null,
            body: body
        );
    }
}
