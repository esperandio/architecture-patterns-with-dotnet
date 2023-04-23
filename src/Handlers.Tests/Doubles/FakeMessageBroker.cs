namespace Handlers.Tests.Doubles;

class FakeMessageBroker : IMessageBroker
{
    public void Publish(string queue, object value)
    {
        return;
    }
}