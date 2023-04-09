using Domain;

namespace Infrastructure.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _dbContext;

    public IProductRepository Products { get; }

    public UnitOfWork(AppDbContext appDbContext, IProductRepository productRepository)
    {
        _dbContext = appDbContext;
        Products = productRepository;
    }

    public async Task<int> Commit()
    {
        return await _dbContext.SaveChangesAsync();
    }
}