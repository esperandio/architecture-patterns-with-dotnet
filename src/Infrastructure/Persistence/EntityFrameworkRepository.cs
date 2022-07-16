using Microsoft.EntityFrameworkCore;
using Infrastructure.Persistence.Models;
using BatchDomain = Domain.Batch;
using OrderLineDomain = Domain.OrderLine;

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

    public async void AddBatch(Batch batch)
    {
        await _dbContext.AddAsync(batch);
    }

    public void UpdateBatch(Batch batch)
    {
        _dbContext.Update(batch);
    }

    public void Commit()
    {
        _dbContext.SaveChanges();
    }

    public async Task<BatchDomain> GetBatchById(int id)
    {
        var batch = await _dbContext.Batches
            .Include("Allocations.OrderLine")
            .FirstOrDefaultAsync((x) => x.ID == id);

        return MapToBatchDomain(batch);
    }

    public int Count<T>() where T : EntityModel
    {
        IQueryable<T> entityQuery = _dbContext.Set<T>();

        return entityQuery.Count();
    }

    private BatchDomain MapToBatchDomain(Batch batch)
    {
        return new BatchDomain(
            batch.Reference,
            batch.Sku,
            batch.PurchasedQuantity,
            batch.Eta,
            batch.Allocations.Select(x => new OrderLineDomain(
                orderId: x.OrderLine.OrderId, 
                sku: x.OrderLine.Sku, 
                quantity: x.OrderLine.Quantity
            )).ToList()
        );
    }
}