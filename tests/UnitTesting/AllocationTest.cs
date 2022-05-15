namespace UnitTesting;

public class AllocationTest
{
    [Fact]
    public void TestAllocatingToABatchReducesTheAvailableQuantity()
    {
        var batch = GetBatch(20);
        var orderLine = GetOrderLine(2);

        batch.Allocate(orderLine);

        Assert.Equal(18, batch.AvailableQuantity);
    }

    [Fact]
    public void TestCanAllocateIfAvailableGreaterThanRequired()
    {
        var batch = GetBatch(20);
        var orderLine = GetOrderLine(2);

        Assert.True(batch.CanAllocate(orderLine));
    }

    [Fact]
    public void TestCannotAllocateIfAvailableSmallerThanRequired()
    {
        var batch = GetBatch(1);
        var orderLine = GetOrderLine(2);

        Assert.False(batch.CanAllocate(orderLine));
    }

    [Fact]
    public void TestCanAllocateIfAvailableEqualToRequired()
    {
        var batch = GetBatch(2);
        var orderLine = GetOrderLine(2);

        Assert.True(batch.CanAllocate(orderLine));
    }

    [Fact]
    public void TestCannotAllocateIfSkusDoNotMatch()
    {
        var batch = GetBatch(10);
        var orderLine = GetOrderLine(2, "EXPENSIVE-TOASTER");

        Assert.False(batch.CanAllocate(orderLine));
    }

    [Fact]
    public void TestCanOnlyDeallocateAllocatedLines()
    {
        var batch = GetBatch(20);
        var deallocatedLine = GetOrderLine(2);

        batch.Deallocate(deallocatedLine);

        Assert.Equal(20, batch.AvailableQuantity);
    }

    [Fact]
    public void TestAllocationIsIdempotent()
    {
        var batch = GetBatch(20);
        var orderLine = GetOrderLine(2);

        batch.Allocate(orderLine);
        batch.Allocate(orderLine);

        Assert.Equal(18, batch.AvailableQuantity);
    }

    private Batch GetBatch(int quantity, string sku = "SMALL-TABLE")
    {
        return new Batch("batch-001", sku, quantity, DateTime.Now);
    }

    private OrderLine GetOrderLine(int quantity, string sku = "SMALL-TABLE")
    {
        return new OrderLine("order-ref", sku, quantity);
    }
}