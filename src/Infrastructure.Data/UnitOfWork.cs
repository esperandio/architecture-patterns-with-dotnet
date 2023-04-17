using Domain;
using Handlers;

namespace Infrastructure.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _dbContext;
    private readonly IMessageBus _messageBus;

    public IProductRepository Products { get; }

    public UnitOfWork(AppDbContext appDbContext, IMessageBus messageBus, IProductRepository productRepository)
    {
        _dbContext = appDbContext;
        _messageBus = messageBus;

        Products = productRepository;
    }

    public async Task<int> Commit()
    {
        var domainEntities = _dbContext.ChangeTracker
            .Entries<Product>()
            .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any());

        var domainEvents = domainEntities
            .SelectMany(x => x.Entity.DomainEvents)
            .ToList();

        foreach (var domainEvent in domainEvents)
            _messageBus.DispatchDomainEvent(domainEvent);

        return await _dbContext.SaveChangesAsync();
    }
}