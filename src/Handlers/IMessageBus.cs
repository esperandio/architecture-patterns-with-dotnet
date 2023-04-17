using Domain;

namespace Handlers;

public interface IMessageBus
{
    public void DispatchDomainEvent(Event @event);
}