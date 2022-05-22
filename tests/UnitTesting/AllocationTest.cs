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

    [Fact]
    public void TestBatchEqualsByReference()
    {
        var batch1 = GetBatch(20);
        var batch2 = GetBatch(20);

        Assert.Equal(batch1, batch2);
    }

    [Fact]
    public void TestOrderLineEqualsByValues()
    {
        var orderLine1 = GetOrderLine(20);
        var orderLine2 = GetOrderLine(20);

        var orderLine3 = GetOrderLine(30);

        Assert.Equal(orderLine1, orderLine2);
        Assert.NotEqual(orderLine1, orderLine3);
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