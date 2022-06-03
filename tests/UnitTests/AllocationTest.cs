namespace UnitTests;

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

        var batch3 = GetBatch(quantity: 20, reference: "another-batch-001");

        Assert.Equal(batch1, batch2);
        Assert.NotEqual(batch1, batch3);
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

    [Fact]
    public void TestPrefersCurrentStockBatchesToShipments()
    {
        var inStockBatch = GetBatch(100);
        var shipmentBatch = GetBatch(quantity: 100, eta: DateTime.Today.AddDays(1));

        var orderLine = GetOrderLine(10);

        AllocationService.Allocate(orderLine, new List<Batch> { inStockBatch, shipmentBatch });

        Assert.Equal(90, inStockBatch.AvailableQuantity);
        Assert.Equal(100, shipmentBatch.AvailableQuantity);
    }

    [Fact]
    public void TestPrefersEarlierBatches()
    {
        var earliestShipmentBatch = GetBatch(quantity: 100, eta: DateTime.Today);
        var latestShipmentBatch = GetBatch(quantity: 100, eta: DateTime.Today.AddDays(1));

        var orderLine = GetOrderLine(10);

        AllocationService.Allocate(orderLine, new List<Batch> { earliestShipmentBatch, latestShipmentBatch });

        Assert.Equal(90, earliestShipmentBatch.AvailableQuantity);
        Assert.Equal(100, latestShipmentBatch.AvailableQuantity);
    }

    [Fact]
    public void TestReturnsAllocatedBatchRef()
    {
        var inStockBatch = GetBatch(quantity:100, reference: "in-stock-batch-ref");
        var shipmentBatch = GetBatch(quantity: 100, reference: "shipment-batch-ref", eta: DateTime.Today.AddDays(1));

        var orderLine = GetOrderLine(10);

        var allocation = AllocationService.Allocate(orderLine, new List<Batch> { inStockBatch, shipmentBatch });

        Assert.True(allocation == inStockBatch.Reference);
    }

    [Fact]
    public void TestRaisesOutOfStockExceptionIfCannotAllocate()
    {
        var batch = GetBatch(quantity: 10, eta: DateTime.Today);

        AllocationService.Allocate(GetOrderLine(10), new List<Batch> { batch });

        Assert.Throws<OutOfStockException>(() => { 
            AllocationService.Allocate(GetOrderLine(1), new List<Batch> { batch }); 
        });
    }

    private Batch GetBatch(int quantity, string reference = "batch-001", string sku = "SMALL-TABLE", DateTime? eta = null)
    {
        return new Batch(reference, sku, quantity, eta);
    }

    private OrderLine GetOrderLine(int quantity, string sku = "SMALL-TABLE")
    {
        return new OrderLine("order-ref", sku, quantity);
    }
}