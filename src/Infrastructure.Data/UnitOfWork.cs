using Domain;
using Handlers;

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

    public IEnumerable<IMessage> CollectNewEvents()
    {
        var domainEntities = _dbContext.ChangeTracker
            .Entries<Product>()
            .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any());

        var domainEvents = domainEntities
            .SelectMany(x => x.Entity.DomainEvents)
            .ToList();

        foreach (var domainEvent in domainEvents)
        {
            yield return domainEvent;
        }
    }

    public async Task<int> Commit()
    {
        return await _dbContext.SaveChangesAsync();
    }
}