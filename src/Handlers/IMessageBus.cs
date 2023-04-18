using Domain;

namespace Handlers;

public interface IMessageBus
{
    public void Handle(Event @event);
}