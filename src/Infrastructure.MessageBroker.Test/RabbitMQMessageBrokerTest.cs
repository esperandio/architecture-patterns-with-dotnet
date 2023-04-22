namespace Infrastructure.MessageBroker.Test;

public class RabbitMQMessageBrokerTest
{
    [Fact]
    public void TestConnection()
    {
        string host = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "";
        int port = Int32.Parse(Environment.GetEnvironmentVariable("RABBITMQ_PORT") ?? "0");
        string username = Environment.GetEnvironmentVariable("RABBITMQ_USERNAME") ?? "";
        string password = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD") ?? "";

        var messageBroker = new RabbitMQMessageBroker(host, port, username, password);

        Assert.IsType<RabbitMQMessageBroker>(messageBroker);
    }

    [Fact]
    public void TestPublish()
    {
        string host = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "";
        int port = Int32.Parse(Environment.GetEnvironmentVariable("RABBITMQ_PORT") ?? "0");
        string username = Environment.GetEnvironmentVariable("RABBITMQ_USERNAME") ?? "";
        string password = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD") ?? "";

        var messageBroker = new RabbitMQMessageBroker(host, port, username, password);

        messageBroker.Publish("hello", "Hello World!");
    }
}
