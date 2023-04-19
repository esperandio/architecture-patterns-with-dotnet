namespace Domain;

public interface IProductRepository
{
    Task<Product?> Get(string sku);
    Task<Product?> GetByBatchReference(string reference);
    Task Add(Product product);
}