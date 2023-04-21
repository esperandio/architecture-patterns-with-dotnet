using Domain;

namespace Handlers.Tests;

class FakeUnitOfWork : IUnitOfWork
{
    public IProductRepository Products { get; }

    public FakeUnitOfWork()
    {
        var defaultProducts = new List<Product>()
        {
            new Product("MINIMALIST-SPOON"),
            new Product("SMALL-TABLE")
        };

        Products = new FakeProductRepository(defaultProducts);
    }

    public IEnumerable<IMessage> CollectNewEvents()
    {
        var repository = (FakeProductRepository) Products;

        var domainEntities = repository.Products
            .Where(x => x.DomainEvents != null && x.DomainEvents.Any());

        var domainEvents = domainEntities
            .SelectMany(x => x.DomainEvents)
            .ToList();

        foreach (var domainEvent in domainEvents)
        {
            yield return domainEvent;
        }
    }

    public Task<int> Commit()
    {
        return Task.Run(() => 1);
    }
}