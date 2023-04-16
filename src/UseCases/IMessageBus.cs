using Domain;

namespace UseCases;

public interface IMessageBus
{
    public void DispatchDomainEvent(Event @event);
}