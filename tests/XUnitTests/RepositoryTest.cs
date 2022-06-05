using Infrastructure.Persistence;
using Infrastructure.Persistence.Models;

namespace XUnitTests;

public class RepositoryTest
{
    [Fact]
    public void TestRepositoryCanSaveToABatch()
    {
        var batch = new Batch()
        {
            Reference = "batch1",
            Sku = "RUSTY-SOAPDISH",
            PurchasedQuantity = 30,
            Eta = DateTime.Today
        };

        var dbContext = new ApplicationDbContextFactory().CreateInMemoryDbContext();
        var repository = new EntityFrameworkRepository<Batch>(dbContext);

        repository.add(batch);

        dbContext.SaveChanges();

        var databaseBatch = repository.get(1);

        Assert.NotNull(databaseBatch);
    }
}