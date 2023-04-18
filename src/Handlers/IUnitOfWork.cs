using Domain;

namespace Handlers;

public interface IUnitOfWork
{
    IProductRepository Products { get; }
    IEnumerable<Event> CollectNewEvents();
    Task<int> Commit();
}