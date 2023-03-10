namespace Domain.Tests;

public class BatchTest
{
    [Fact]
    public void TestAvailableQuantityIsReducedWhenWeAllocated()
    {
        var batch = new Batch("reference-001", "SMALL-TABLE", 20, null);
        var orderLine = new OrderLine("SMALL-TABLE", 2);

        batch.allocate(orderLine);

        Assert.Equal(18, batch.AvailableQuantity);
    }
}