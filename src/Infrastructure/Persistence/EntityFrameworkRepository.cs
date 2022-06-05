using Infrastructure.Persistence.Models;

namespace Infrastructure.Persistence;

public class EntityFrameworkRepository<T> where T : EntityModel
{
    private readonly ApplicationDbContext _dbContext;

    public EntityFrameworkRepository(ApplicationDbContext applicationDbContext)
    {
        _dbContext = applicationDbContext;
    }

    public void add(T entityModel)
    {
        _dbContext.Add(entityModel);
    }

    public T get(int id)
    {
       return _dbContext.Find<T>(id);
    }
}