using Domain;

namespace Infrastructure.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _dbContext;
    public IBatchRepository Batches { get; }

    public UnitOfWork(AppDbContext appDbContext, IBatchRepository batchRepository)
    {
        _dbContext = appDbContext;
        Batches = batchRepository;
    }

    public async Task<int> Commit()
    {
        return await _dbContext.SaveChangesAsync();
    }
}