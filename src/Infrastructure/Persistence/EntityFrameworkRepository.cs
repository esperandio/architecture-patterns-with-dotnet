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

    public int Count<T>() where T : EntityModel
    {
        IQueryable<T> entityQuery = _dbContext.Set<T>();

        return entityQuery.Count();
    }
}