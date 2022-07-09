using Microsoft.EntityFrameworkCore;
using Infrastructure.Persistence.Models;

namespace Infrastructure.Persistence;

public class EntityFrameworkRepository
{
    private readonly ApplicationDbContext _dbContext;

    public EntityFrameworkRepository(ApplicationDbContext applicationDbContext)
    {
        _dbContext = applicationDbContext;
    }

    public void Add<T>(T entityModel) where T : EntityModel
    {
        _dbContext.Add(entityModel);
    }

    public T Get<T>(int id) where T : EntityModel
    {
        return _dbContext.Find<T>(id);
    }

    public async Task<Batch> GetBatchById(int id)
    {
        var batch = await _dbContext.Batches
            .Include("Allocations.OrderLine")
            .FirstAsync((x) => x.ID == id);

        return batch;
    }

    public int Count<T>() where T : EntityModel
    {
        IQueryable<T> entityQuery = _dbContext.Set<T>();

        return entityQuery.Count();
    }
}