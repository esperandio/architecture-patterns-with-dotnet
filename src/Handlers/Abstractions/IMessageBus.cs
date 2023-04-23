using Domain;

namespace Handlers.Abstractions;

public interface IMessageBus
{
    public IReadOnlyCollection<string> Results {get;}
    public Task Handle(Command command);
}