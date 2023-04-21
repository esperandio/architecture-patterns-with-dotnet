using Domain;

namespace Handlers;

public interface IUnitOfWork
{
    IProductRepository Products { get; }
    IEnumerable<IMessage> CollectNewMessages();
    Task<int> Commit();
}