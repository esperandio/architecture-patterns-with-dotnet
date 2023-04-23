namespace Handlers.Tests;

class FakeMessageBroker : IMessageBroker
{
    public void Publish(string queue, object value)
    {
        return;
    }
}