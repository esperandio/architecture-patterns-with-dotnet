namespace Domain;

public interface IProductRepository
{
    Task<Product?> Get(string sku);
    Task Add(Product product);
}