using Domain;

namespace Handlers.Tests;

class FakeProductRepository : IProductRepository
{
    private List<Product> _products;

    public FakeProductRepository()
    : this(new List<Product>())
    {
    }

    public FakeProductRepository(List<Product> products)
    {
        _products = products;
    }

    public Task<Product?> Get(string sku)
    {
        return Task.Run(() => _products.FirstOrDefault(x => x.Sku == sku));
    }

    public Task<Product?> GetByBatchReference(string reference)
    {
        return Task.Run(() => 
            _products.FirstOrDefault(
                x => x.Batches.Where(y => y.Reference == reference).Count() > 0
            )
        );
    }

    public Task Add(Product product)
    {
        return Task.Run(() => _products.Add(product));
    }
}