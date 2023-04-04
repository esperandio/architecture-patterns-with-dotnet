using Microsoft.EntityFrameworkCore;
using Domain;

namespace Infrastructure.Data;

public class BatchRepository : IBatchRepository
{
    private readonly AppDbContext _dbContext;

    public BatchRepository(AppDbContext appDbContext)
    {
        _dbContext = appDbContext;
    }

    public async Task<IEnumerable<Batch>> FindBySkuAsync(string sku)
    {
        var queryBatches = _dbContext.Batches
            .Include(x => x.Allocations)
            .AsQueryable();

        return await queryBatches
            .Where(x => x.Sku.Equals(sku))
            .ToListAsync();        
    }

    public async Task Add(Batch batch)
    {
        await _dbContext.Batches.AddAsync(batch);
    }

    public async Task<Batch?> Get(string reference)
    {
        return await _dbContext.Batches.FirstOrDefaultAsync(x => x.Reference == reference);
    }
}