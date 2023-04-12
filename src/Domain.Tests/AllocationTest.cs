namespace Domain.Tests;

public class AllocationTest
{
    [Fact]
    public void TestCannotAllocateIfSkusDoNotMatch()
    {
        var product = new Product("SMALL-FORK");

        Assert.Throws<SkuDoesNotMatchException>(() => { 
            product.Allocate("order-001", "EXPENSIVE-TOASTER", 2);
        });
    }

    [Fact]
    public void TestRecordsOutOfStockEventIfCannotAllocate()
    {
        var batch = new Batch("batch001", "SMALL-FORK", 10);

        var product = new Product("SMALL-FORK", new List<Batch>(){ batch });

        product.Allocate("order001", "SMALL-FORK", 10);

        var batchReference = product.Allocate("order001", "SMALL-FORK", 1);

        Assert.Empty(batchReference);
        Assert.Equal(1, product.DomainEvents.Count);
        Assert.Equal(new OutOfStockEvent("SMALL-FORK"), product.DomainEvents.First());
    }
}
