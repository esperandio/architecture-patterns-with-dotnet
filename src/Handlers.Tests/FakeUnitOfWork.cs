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

    public Task<int> Commit()
    {
        return Task.Run(() => 1);
    }
}