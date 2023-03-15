namespace Domain.Tests;

public class AllocationTest
{
    [Fact]
    public void TestAvailableQuantityIsReducedWhenOrderLineIsAllocated()
    {
        var batch = new Batch("batch-001", "SMALL-TABLE", 20);
        var orderLine = new OrderLine("order-001", "SMALL-TABLE", 2);

        batch.Allocate(orderLine);

        Assert.Equal(18, batch.AvailableQuantity);
    }

    [Fact]
    public void TestCannotAllocateIfAvailableSmallerThanRequired()
    {
        var batch = new Batch("batch-001", "BLUE-CUSHION", 1);
        var orderLine = new OrderLine("order-001", "BLUE-CUSHION", 2);

        Assert.Throws<RequiresQuantityGreaterThanAvailableException>(() => { 
            batch.Allocate(orderLine);
        });
    }

    [Fact]
    public void TestCannotAllocateTheSameOrderLineTwice()
    {
        var batch = new Batch("reference-001", "BLUE-VASE", 10);
        var orderLine1 = new OrderLine("order-001", "BLUE-VASE", 2);
        var orderLine2 = new OrderLine("order-001", "BLUE-VASE", 2);

        batch.Allocate(orderLine1);

        Assert.Throws<DuplicateOrderLineException>(() => { 
            batch.Allocate(orderLine2);
        });
    }

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
}
