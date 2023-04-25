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
    public void TestCannotAllocateIfDuplicateOrderLine()
    {
        var batch = new Batch("batch001", "SMALL-FORK", 10);

        var product = new Product("SMALL-FORK", new List<Batch>(){ batch });

        product.Allocate("order001", "SMALL-FORK", 10);

        Assert.Throws<DuplicateOrderLineException>(() => { 
            product.Allocate("order001", "SMALL-FORK", 10);
        });
    }

    [Fact]
    public void TestRecordsOutOfStockEventIfAvailableSmallerThanRequired()
    {
        var batch = new Batch("batch001", "SMALL-FORK", 1);

        var product = new Product("SMALL-FORK", new List<Batch>(){ batch });

        product.Allocate("order001", "SMALL-FORK", 2);

        Assert.Equal(new OutOfStockEvent("SMALL-FORK"), product.DomainEvents.Last());
    }
}
