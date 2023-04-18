using Domain;

namespace Handlers;

public interface IMessageBus
{
    public Task Handle(Event @event);
}