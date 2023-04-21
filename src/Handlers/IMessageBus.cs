using Domain;

namespace Handlers;

public interface IMessageBus
{
    public IReadOnlyCollection<string> Results {get;}
    public Task Handle(Command command);
}