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

    [Fact]
    public void TestPrefersCurrentStockBatchesToShipment()
    {
        var inStockBatch = new Batch("in-stock-batch", "RETRO-CLOCK", 100);
        var shipmentBatch = new Batch("shipment-batch", "RETRO-CLOCK", 100, DateTime.Now.AddDays(1));

        var orderLine = new OrderLine("order-001", "RETRO-CLOCK", 10);

        var product = new Product("RETRO-CLOCK", new List<Batch>() { inStockBatch, shipmentBatch });

        product.Allocate(orderLine);

        Assert.Equal(90, inStockBatch.AvailableQuantity);
        Assert.Equal(100, shipmentBatch.AvailableQuantity);
    }

    [Fact]
    public void TestPrefersEarlierBatches()
    {
        var earliest = new Batch("speedy-batch", "MINIMALIST-SPOON", 100, DateTime.Today);
        var medium = new Batch("normal-batch", "MINIMALIST-SPOON", 100, DateTime.Today.AddDays(1));
        var latest = new Batch("slow-batch", "MINIMALIST-SPOON", 100, DateTime.Today.AddDays(2));

        var orderLine = new OrderLine("order-001", "MINIMALIST-SPOON", 10);

        var product = new Product("MINIMALIST-SPOON", new List<Batch>() { latest, medium, earliest });

        product.Allocate(orderLine);

        Assert.Equal(90, earliest.AvailableQuantity);
        Assert.Equal(100, medium.AvailableQuantity);
        Assert.Equal(100, latest.AvailableQuantity);
    }

    [Fact]
    public void TestRaisesOutOfStockExceptionIfCannotAllocate()
    {
        var batch = new Batch("batch-001", "SMALL-FORK", 10);
        var orderLine = new OrderLine("order-001", "SMALL-FORK", 15);

        var product = new Product("SMALL-FORK", new List<Batch>() { batch });

        Assert.Throws<OutOfStockException>(() => { 
            product.Allocate(orderLine);
        });
    }
}
