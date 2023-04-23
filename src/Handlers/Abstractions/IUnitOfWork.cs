using Domain;

namespace Handlers.Abstractions;

public interface IUnitOfWork
{
    IProductRepository Products { get; }
    IEnumerable<IMessage> CollectNewMessages();
    Task<int> Commit();
}