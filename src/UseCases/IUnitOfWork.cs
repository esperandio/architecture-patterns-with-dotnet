using Domain;

namespace UseCases;

public interface IUnitOfWork
{
    IProductRepository Products { get; }
    Task<int> Commit();
}