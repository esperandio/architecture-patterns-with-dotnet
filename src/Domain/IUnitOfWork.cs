namespace Domain;

public interface IUnitOfWork
{
    IProductRepository Products { get; }
    Task<int> Commit();
}