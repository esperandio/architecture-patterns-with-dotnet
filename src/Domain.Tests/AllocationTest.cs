namespace Domain.Tests;

public class AllocationTest
{
    [Fact]
    public void TestAvailableQuantityIsReducedWhenOrderLineIsAllocated()
    {
        var batch = new Batch("batch-001", "SMALL-TABLE", 20, null);
        var orderLine = new OrderLine("order-001", "SMALL-TABLE", 2);

        batch.allocate(orderLine);

        Assert.Equal(18, batch.AvailableQuantity);
    }


    [Fact]
    public void TestCannotAllocateIfAvailableSmallerThanRequired()
    {
        var batch = new Batch("batch-001", "BLUE-CUSHION", 1, null);
        var orderLine = new OrderLine("order-001", "BLUE-CUSHION", 2);

        Assert.Throws<OutOfStockException>(() => { 
            batch.allocate(orderLine);
        });
    }

    [Fact]
    public void TestCannotAllocateTheSameOrderLineTwice()
    {
        var batch = new Batch("reference-001", "BLUE-VASE", 10, null);
        var orderLine1 = new OrderLine("order-001", "BLUE-VASE", 2);
        var orderLine2 = new OrderLine("order-001", "BLUE-VASE", 2);

        batch.allocate(orderLine1);

        Assert.Throws<AllocateSameLineTwiceException>(() => { 
            batch.allocate(orderLine2);
        });
    }

    [Fact]
    public void TestCannotAllocateIfSkusDoNotMatch()
    {
        var batch = new Batch("reference-001", "UNCOMFORTABLE-CHAIR", 100, null);
        var orderLine = new OrderLine("order-001", "EXPENSIVE-TOASTER", 2);

        Assert.Throws<SkuDoesNotMatchException>(() => { 
            batch.allocate(orderLine);
        });
    }
}
