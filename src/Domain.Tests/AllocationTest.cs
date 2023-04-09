namespace Domain.Tests;

public class AllocationTest
{
    [Fact]
    public void TestCannotAllocateIfSkusDoNotMatch()
    {
        var batch = new Batch("reference-001", "UNCOMFORTABLE-CHAIR", 100);
        var orderLine = new OrderLine("order-001", "EXPENSIVE-TOASTER", 2);

        Assert.Throws<SkuDoesNotMatchException>(() => { 
            batch.Allocate(orderLine);
        });
    }

    [Fact]
    public void TestAvailableQuantityIsIncreasedWhenOrderLineIsDeallocated()
    {
        var orderLine = new OrderLine("order-001", "SMALL-TABLE", 2);
        var batch = new Batch("batch-001", "SMALL-TABLE", 20, new List<OrderLine>() { orderLine });

        Assert.Equal(18, batch.AvailableQuantity);

        batch.Deallocate(orderLine);

        Assert.Equal(20, batch.AvailableQuantity);
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
