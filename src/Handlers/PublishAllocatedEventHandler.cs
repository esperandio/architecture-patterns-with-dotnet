using Domain;

namespace Handlers;

class PublishAllocatedEventHandler
{
    private readonly IMessageBroker _messageBroker;

    public PublishAllocatedEventHandler(IMessageBroker messageBroker)
    {
        _messageBroker = messageBroker;
    }

    public void Handle(AllocatedEvent allocatedEvent)
    {
        _messageBroker.Publish("allocated", allocatedEvent);
    }
}