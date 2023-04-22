namespace Infrastructure.MessageBroker.Test;

class TestObject
{
    public int Property1 { get; set; }
    public string Property2 { get; set; }

    public TestObject(int property1, string property2)
    {
        Property1 = property1;
        Property2 = property2;
    }
}

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

        messageBroker.Publish("hello", new TestObject(1, "test"));
    }
}
