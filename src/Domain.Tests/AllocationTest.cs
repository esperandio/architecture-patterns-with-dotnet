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
    public void TestCannotAllocateIfOrderLineQuantityIsGreaterThanAvailableQuantity()
    {
        var batch = new Batch("batch-001", "BLUE-CUSHION", 1, null);
        var orderLine = new OrderLine("order-001", "BLUE-CUSHION", 2);

        Assert.Throws<OutOfStockException>(() => { 
            batch.allocate(orderLine);
        });
    }
}