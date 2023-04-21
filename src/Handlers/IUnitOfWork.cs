using Domain;

namespace Handlers;

public interface IUnitOfWork
{
    IProductRepository Products { get; }
    IEnumerable<IMessage> CollectNewEvents();
    Task<int> Commit();
}