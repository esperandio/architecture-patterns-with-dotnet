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
    public void TestCannotDeallocateUnallocatedOrderLine()
    {
        var allocatedOrderLine = new OrderLine("order-001", "SMALL-TABLE", 2);
        var unallocatedOrderLine = new OrderLine("order-001", "SMALL-TABLE", 4);

        var batch = new Batch("batch-001", "SMALL-TABLE", 20, new List<OrderLine>() { allocatedOrderLine });

        Assert.Throws<UnallocatedOrderLineException>(() => { 
            batch.Deallocate(unallocatedOrderLine);
        });
    }

    [Fact]
    public void TestRaisesOutOfStockExceptionIfCannotAllocate()
    {
        var batch = new Batch("batch-001", "SMALL-FORK", 10);

        var product = new Product("SMALL-FORK", new List<Batch>() { batch });

        Assert.Throws<OutOfStockException>(() => { 
            product.Allocate("order-001", "SMALL-FORK", 15);
        });
    }
}
