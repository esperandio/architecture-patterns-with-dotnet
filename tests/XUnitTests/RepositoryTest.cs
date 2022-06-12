using Infrastructure.Persistence;
using Infrastructure.Persistence.Models;

namespace XUnitTests;

public class RepositoryTest
{
    [Fact]
    public void TestRepositoryCanSaveToABatch()
    {
        var dbContext = new ApplicationDbContextFactory().CreateInMemoryDbContext("TestRepositoryCanSaveToABatch");
        var repository = new EntityFrameworkRepository(dbContext);

        var batch = new Batch()
        {
            Reference = "batch1",
            Sku = "RUSTY-SOAPDISH",
            PurchasedQuantity = 30,
            Eta = DateTime.Today
        };

        repository.Add(batch);

        dbContext.SaveChanges();

        var retrievedBatch = repository.Get<Batch>(1);

        Assert.NotNull(retrievedBatch);
        Assert.Equal(1, repository.Count<Batch>());
    }

    [Fact]
    public void TestRepositoryCanRetrieveABatchWithAllocations()
    {
        var dbContext = new ApplicationDbContextFactory().CreateInMemoryDbContext("TestRepositoryCanRetrieveABatchWithAllocations");
        var repository = new EntityFrameworkRepository(dbContext);

        var batch = new Batch()
        {
            Reference = "batch1",
            Sku = "RUSTY-SOAPDISH",
            PurchasedQuantity = 30,
            Eta = DateTime.Today
        };

        var orderLine1 = GetOrderLine("order1", 10);
        var orderLine2 = GetOrderLine("order2", 5);
        var orderLine3 = GetOrderLine("order3", 10);

        var orderLines = new List<OrderLine>()
        {
            orderLine1,
            orderLine2,
            orderLine3
        };

        var allocations = new List<Allocation>()
        {
            GetAllocation(batch, orderLine1),
            GetAllocation(batch, orderLine2),
            GetAllocation(batch, orderLine3)
        };

        repository.Add(batch);
        orderLines.ForEach(x => repository.Add(x));
        allocations.ForEach(x => repository.Add(x));

        dbContext.SaveChanges();

        Assert.Equal(1, repository.Count<Batch>());
        Assert.Equal(3, repository.Count<OrderLine>());
        Assert.Equal(3, repository.Count<Allocation>());

        var retrievedBatch = repository.Get<Batch>(1);

        Assert.Equal(3, retrievedBatch.Allocations.Count());
    }

    private OrderLine GetOrderLine(string orderId, int quantity, string sku = "GENERIC-SOFA")
    {
        return new OrderLine()
        {
            OrderId = orderId,
            Quantity = quantity,
            Sku = sku
        };
    }

    private Allocation GetAllocation(Batch batch, OrderLine orderLine)
    {
        return new Allocation()
        {
            Batch = batch,
            OrderLine = orderLine
        };
    }
}