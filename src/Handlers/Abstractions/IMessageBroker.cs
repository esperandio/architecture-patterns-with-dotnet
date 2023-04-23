namespace Handlers.Abstractions;

public interface IMessageBroker
{
    public void Publish(string queue, object value);
}