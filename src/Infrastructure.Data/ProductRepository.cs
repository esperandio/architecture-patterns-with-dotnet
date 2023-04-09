using Microsoft.EntityFrameworkCore;
using Domain;

namespace Infrastructure.Data;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _dbContext;

    public ProductRepository(AppDbContext appDbContext)
    {
        _dbContext = appDbContext;
    }

    public Task Add(Product product)
    {
        throw new NotImplementedException();
    }

    public Task<Product?> Get(string sku)
    {
        throw new NotImplementedException();
    }
}