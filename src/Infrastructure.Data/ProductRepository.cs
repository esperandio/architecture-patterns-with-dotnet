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

    public async Task Add(Product product)
    {
        await _dbContext.Products.AddAsync(product);
    }

    public async Task<Product?> Get(string sku)
    {
        return await _dbContext.Products.FirstOrDefaultAsync(x => x.Sku == sku);
    }
}