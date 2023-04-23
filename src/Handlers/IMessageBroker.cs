namespace Handlers;

public interface IMessageBroker
{
    public void Publish(string queue, object value);
}