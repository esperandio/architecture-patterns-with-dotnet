using Domain;

namespace Handlers;

public interface IUnitOfWork
{
    IProductRepository Products { get; }
    Task<int> Commit();
}